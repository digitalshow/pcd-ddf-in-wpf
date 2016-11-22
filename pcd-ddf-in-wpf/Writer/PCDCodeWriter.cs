/* 

pcd-ddf-in-wpf: A DDF editor for PC_DIMMER, an open source light 
control software.
Copyright (C) 2016 Ingo Koinzer

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Koinzer.pcdddfinwpf.Model;
using Koinzer.pcdddfinwpf.Model.GUI;

namespace Koinzer.pcdddfinwpf.Writer
{
	/// <summary>
	/// Description of PCDCodeWriter.
	/// </summary>
	public class PCDCodeWriter
	{
		public PCDCodeWriter()
		{
		}
		
		public void Write(XElement codeEl, Model.PCDDevice device, WriteResults results)
		{
			Dictionary<String, String> functions = new Dictionary<string, string>();
			// Empty functions
			functions.Add("procedure InitDevice(Device: String);", "begin\r\nend;\r\n");
			functions.Add("procedure FormClose();", "begin\r\nend;\r\n");
			functions.Add("procedure FormShow();", "begin\r\n  dontRefresh := false;\r\nend;\r\n");
			
			// FormRefresh
			StringBuilder functionFormRefresh = new StringBuilder();
			functionFormRefresh.AppendLine("var");
			functionFormRefresh.AppendLine("  value: Integer;");
			functionFormRefresh.AppendLine("  temp, phi, r: Double;");
			functionFormRefresh.AppendLine("begin");
			functionFormRefresh.AppendLine("  if dontRefresh then begin");
			functionFormRefresh.AppendLine("    dontRefresh := false;");
			functionFormRefresh.AppendLine("    exit;");
			functionFormRefresh.AppendLine("  end;");
			functionFormRefresh.AppendLine("  case channel of");
			foreach (Model.PCDDeviceChannel channel in device.Channels) {
				bool posUpdate = (channel.ChannelType == "pan" || channel.ChannelType == "tilt" || channel.ChannelType == "panfine" || channel.ChannelType == "tiltfine");
				IEnumerable<PCDDeviceControl> controls = 
					device.GUIElements.OfType<PCDDeviceControl>().Where(
						ctrl => ctrl.AssociatedChannel == channel);
				if (!posUpdate && (controls.Count() == 0))
					continue;
				functionFormRefresh.AppendLine("    "+channel.Channel+": begin // "+channel.Name);
				if (posUpdate)
					functionFormRefresh.AppendLine(CodeRessource.PositionXYUpdate);
				functionFormRefresh.AppendLine("        value := get_channel('"+channel.ChannelType+"');");
				if (channel.Subsets.Any()) {
					int subsetIndex = 0;
					bool joinNext = false;
					functionFormRefresh.AppendLine("        case value of");
					foreach (Model.PCDChannelSubset subset in channel.Subsets) {
						if (!joinNext) {
							functionFormRefresh.Append("          "+subset.MinValue);
							int maxValue = subset.MaxValue;
							bool slidable = false;
							for (int i = channel.Subsets.IndexOf(subset); i < channel.Subsets.Count; i++) {
								slidable = slidable || (channel.Subsets[i] is Model.PCDChannelRange);
								maxValue = channel.Subsets[i].MaxValue;
								if (!channel.Subsets[i].JoinNext)
									break;
							}
							if (maxValue > subset.MinValue)
								functionFormRefresh.Append(".."+maxValue);
							functionFormRefresh.AppendLine(": begin // " + subset.Name);
							foreach (PCDDropdown dropdown in controls.OfType<PCDDropdown>())
								functionFormRefresh.AppendLine("              "+dropdown.Name+".ItemIndex := "+ subsetIndex + ";");
							foreach (PCDSlider slider in controls.OfType<PCDSlider>()) {
								functionFormRefresh.AppendLine("              try");
								functionFormRefresh.AppendLine("                "+slider.Name+".Min := "+ subset.MinValue + ";");
								functionFormRefresh.AppendLine("              except end;");
								functionFormRefresh.AppendLine("              "+slider.Name+".Max := "+ maxValue + ";");
								functionFormRefresh.AppendLine("              "+slider.Name+".Min := "+ subset.MinValue + ";");
								functionFormRefresh.AppendLine("              "+slider.Name+".Position := value;");
								functionFormRefresh.AppendLine("              "+slider.Name+".Enabled := "+ ((slidable)?"true":"false")+";");
							}
							functionFormRefresh.AppendLine("            end;");
							subsetIndex++;
						}
						joinNext = subset.JoinNext;
					}
					functionFormRefresh.AppendLine("        end;");
				} else {
					foreach (PCDSlider slider in controls.OfType<PCDSlider>()) {
						functionFormRefresh.AppendLine("        "+slider.Name+".Position := value;");
					}
				}
				functionFormRefresh.AppendLine("      end;");
			}
			functionFormRefresh.AppendLine("  end;");
			functionFormRefresh.AppendLine("  dontRefresh := false;");
			functionFormRefresh.AppendLine("end;");
			functions.Add("procedure FormRefresh(channel: Integer);", functionFormRefresh.ToString());
			
			// Fadetime
			StringBuilder functionGetFadetime = new StringBuilder();
			functionGetFadetime.AppendLine("begin");
			functionGetFadetime.AppendLine("  result := 0;");
			if (device.GUIElements.OfType<PCDFadetimeEdit>().Any()) {
				if (device.GUIElements.OfType<PCDFadetimeCheckbox>().Any()) {
					functionGetFadetime.Append("  if (usefadezeit.Checked) then\r\n  ");
				}
				functionGetFadetime.AppendLine("  result := strtoint(fadezeit.Text);");
			}
			functionGetFadetime.AppendLine("end;");
			functions.Add("function getfadetime: Integer;", functionGetFadetime.ToString());
			
			// Control changed handlers
			// Dropdowns
			foreach (PCDDropdown dropdown in device.GUIElements.OfType<PCDDropdown>()) {
				Model.PCDDeviceChannel channel = dropdown.AssociatedChannel;
				if ((channel == null) || (channel.Subsets.Count <= 0)) {
					functions.Add("procedure " + dropdown.Name + "_changed();", "begin\r\nend;\r\n");
					continue;
				}
				StringBuilder functionChanged = new StringBuilder();
				functionChanged.AppendLine("begin");
				functionChanged.AppendLine("  case "+dropdown.Name+".ItemIndex of");
				int subsetIndex = 0;
				bool joinNext = false;
				foreach (Model.PCDChannelSubset subset in channel.Subsets) {
					if (!joinNext) {
						functionChanged.AppendLine("    "+subsetIndex+": // "+ subset.Name);
						functionChanged.Append("        set_channel('"+channel.ChannelType+"', -1, ");
						if (subset is Model.PCDChannelItem) {
							functionChanged.AppendLine(((Model.PCDChannelItem)subset).SetValue+", 0);");
						} else {
							functionChanged.AppendLine(subset.MinValue+", 0);");
						}
						subsetIndex++;
					}
					joinNext = subset.JoinNext;
				}
				functionChanged.AppendLine("  end;");
				functionChanged.AppendLine("end;");
				String functionName = dropdown.Name + "_changed();";
				if (channel.ChannelType == "color1")
					functionName = "ColorBoxChange();";
				if (channel.ChannelType == "color2")
					functionName = "ColorBoxChange2();";
				functions.Add("procedure "+functionName, functionChanged.ToString());
			}
			
			// Sliders
			foreach (PCDSlider slider in device.GUIElements.OfType<PCDSlider>()) {
				Model.PCDDeviceChannel channel = slider.AssociatedChannel;
				if (channel == null)
					continue;
				if (functions.ContainsKey("procedure "+slider.Name + "_changed();")) {
					results.AddMessage("PCDCodeWriter.MultipleSlidersForChannel".Localize(), channel.Name);
					continue;
				}
				StringBuilder functionChanged = new StringBuilder();
				functionChanged.AppendLine("begin");
				functionChanged.AppendLine("  dontRefresh := true;");
				functionChanged.Append("  set_channel('"+channel.ChannelType+"', -1, ");
				functionChanged.AppendLine(slider.Name + ".Position, 0);");
				functionChanged.AppendLine("end;");
				functions.Add("procedure "+slider.Name + "_changed();", functionChanged.ToString());
			}
			
			// Presets
			int presetIndex = 0;
			foreach (Model.PCDDevicePreset preset in device.Presets) {
				StringBuilder functionCall = new StringBuilder();
				functionCall.AppendLine("begin");
				foreach (Model.PCDDevicePresetChannel channel in preset.Channels) {
					if (channel.Channel == null) {
						results.AddMessage("PCDCodeWriter.FeatureLineNotAssociated".Localize(), preset.Name, preset.Channels.IndexOf(channel));
						continue;
					}
					int value = channel.Value;
					if (channel.Subset != null) {
						if (channel.Subset is Model.PCDChannelItem) {
							value = ((Model.PCDChannelItem)channel.Subset).SetValue;
						} else
							value = channel.Subset.MinValue;
					}
					functionCall.Append("  set_channel('" + channel.Channel.ChannelType + "', -1, ");
					functionCall.AppendLine(value + ", "+channel.FadeTime+");");
					//if (channel.DelayTime > 0)
					//	functionCall.AppendLine("  sleep("+channel.DelayTime*1000+");");
					if (!String.IsNullOrWhiteSpace(channel.MessageBoxMessage))
						functionCall.AppendLine("  ShowMessage('"+channel.MessageBoxMessage.Replace("'", "''")+"');");
				}
				if (device.GUIElements.FirstOrDefault(el => el is PCDPresetSelector) != null) {
					functionCall.AppendLine("  presetSelector.ItemIndex := " + presetIndex + ";");
				}
				functionCall.AppendLine("end;");
				functions.Add("procedure call_preset_"+Model.CodeHelper.MakeCodeFriendly(preset.Name).ToLower()+"();", functionCall.ToString());
				presetIndex++;
			}
			
			// SelectPreset
			if (device.GUIElements.FirstOrDefault(el => el is PCDPresetSelector) != null) {
				StringBuilder functionSelPre = new StringBuilder();
				functionSelPre.AppendLine("begin");
				if (device.Presets.Count > 0) {
					functionSelPre.AppendLine("  case presetSelector.ItemIndex of");
					int i = 0;
					foreach (PCDDevicePreset preset in device.Presets) {
						functionSelPre.AppendLine("    "+(i++)+": call_preset_"+Model.CodeHelper.MakeCodeFriendly(preset.Name).ToLower()+"();");
					}
					functionSelPre.AppendLine("  end;");
				}
				functionSelPre.AppendLine("end;");
				functions.Add("procedure SelectPreset();", functionSelPre.ToString());
			}
			
			// XY
			if (device.GUIElements.OfType<PCDPosition>().Any()) {
				functions.Add("procedure PositionXYChange(Top, Left: Integer);", CodeRessource.PositionXYChange);
			}
			
			// ColorPicker
			if (device.GUIElements.OfType<PCDColorPicker>().Any()) {
				StringBuilder functionColorPickerChange = new StringBuilder();
				functionColorPickerChange.AppendLine("begin");
				functionColorPickerChange.AppendLine("  set_color(R, G, B, getfadetime, 0);");
				functionColorPickerChange.AppendLine("end;");
				functions.Add("procedure ColorPickerChange(R, G, B: Byte);", functionColorPickerChange.ToString());
			}
			
			// ChangeColorPicker
			if (device.GUIElements.OfType<PCDButton>().Any(btn => btn.SpecialButtonAction == PCDSpecialButtonAction.ChangeColorPicker)) {
				functions.Add("procedure onchangecolorpicker;", CodeRessource.ColorPickerChange);
			}
			
			// Writing the unit
			StringBuilder code = new StringBuilder();
			code.AppendLine("unit "+device.CodeFriendlyName()+";");
			code.AppendLine();
			code.AppendLine("interface");
			code.AppendLine();
			foreach (KeyValuePair<String, String> function in functions) {
				code.AppendLine(function.Key);
			}
			code.AppendLine();
			code.AppendLine("implementation");
			code.AppendLine();
			code.AppendLine("var");
			code.AppendLine("  dontRefresh: Boolean;");
			code.AppendLine();
			foreach (KeyValuePair<String, String> function in functions) {
				code.AppendLine(function.Key);
				code.AppendLine(function.Value);
			}
			code.AppendLine("end.");
			codeEl.Add(code.ToString());
		}
	}
}
