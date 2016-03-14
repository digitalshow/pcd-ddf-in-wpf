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
			functions.Add("InitDevice(Device: String);", "begin\r\nend;\r\n");
			functions.Add("FormClose();", "begin\r\nend;\r\n");
			functions.Add("FormShow();", "begin\r\n  dontRefresh := false;\r\nend;\r\n");
			
			// FormRefresh
			StringBuilder functionFormRefresh = new StringBuilder();
			functionFormRefresh.AppendLine("var");
			functionFormRefresh.AppendLine("  value: Integer;");
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
				if (!posUpdate && ((controls.Count() == 0) || (channel.Subsets.Count <= 0)))
					continue;
				functionFormRefresh.AppendLine("    "+channel.Channel+": begin // "+channel.Name);
				if (posUpdate)
					functionFormRefresh.AppendLine(CodeRessource.PositionXYUpdate);
				functionFormRefresh.AppendLine("        value := get_channel('"+channel.ChannelType+"');");
				functionFormRefresh.AppendLine("        case value of");
				int subsetIndex = 0;
				bool joinNext = false;
				foreach (Model.PCDChannelSubset subset in channel.Subsets) {
					if (!joinNext) {
						functionFormRefresh.Append("          "+subset.MinValue);
						if (subset.MaxValue > subset.MinValue)
							functionFormRefresh.Append(".."+subset.MaxValue);
						functionFormRefresh.AppendLine(": begin // " + subset.Name);
						foreach (PCDDropdown dropdown in controls.OfType<PCDDropdown>())
							functionFormRefresh.AppendLine("              "+dropdown.Name+".ItemIndex := "+ subsetIndex + ";");
						foreach (PCDSlider slider in controls.OfType<PCDSlider>()) {
							functionFormRefresh.AppendLine("              try");
							functionFormRefresh.AppendLine("                "+slider.Name+".Min := "+ subset.MinValue + ";");
							functionFormRefresh.AppendLine("              except end;");
							functionFormRefresh.AppendLine("              "+slider.Name+".Max := "+ subset.MaxValue + ";");
							functionFormRefresh.AppendLine("              "+slider.Name+".Min := "+ subset.MinValue + ";");
							functionFormRefresh.AppendLine("              "+slider.Name+".Position := value;");
							functionFormRefresh.AppendLine("              "+slider.Name+".Enabled := "+ ((subset is Model.PCDChannelRange)?"true":"false")+";");
						}
						functionFormRefresh.AppendLine("            end;");
						subsetIndex++;
					}
					joinNext = subset.JoinNext;
				}
				functionFormRefresh.AppendLine("        end;");
				functionFormRefresh.AppendLine("      end;");
			}
			functionFormRefresh.AppendLine("  end;");
			functionFormRefresh.AppendLine("  dontRefresh := false;");
			functionFormRefresh.AppendLine("end;");
			functions.Add("FormRefresh(channel: Integer);", functionFormRefresh.ToString());
			
			// Control changed handlers
			foreach (PCDDropdown dropdown in device.GUIElements.OfType<PCDDropdown>()) {
				Model.PCDDeviceChannel channel = dropdown.AssociatedChannel;
				if ((channel == null) || (channel.Subsets.Count <= 0)) {
					functions.Add(dropdown.Name + "_changed();", "begin\r\nend;\r\n");
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
				functions.Add(functionName, functionChanged.ToString());
			}
			
			foreach (PCDSlider slider in device.GUIElements.OfType<PCDSlider>()) {
				Model.PCDDeviceChannel channel = slider.AssociatedChannel;
				if ((channel == null) || (channel.Subsets.Count <= 0))
					continue;
				StringBuilder functionChanged = new StringBuilder();
				functionChanged.AppendLine("begin");
				functionChanged.AppendLine("  dontRefresh := true;");
				functionChanged.Append("  set_channel('"+channel.ChannelType+"', -1, ");
				functionChanged.AppendLine(slider.Name + ".Position, 0);");
				functionChanged.AppendLine("end;");
				functions.Add(slider.Name + "_changed();", functionChanged.ToString());
			}
			
			if (device.GUIElements.OfType<PCDPosition>().Count() > 0) {
				functions.Add("PositionXYChange(Top, Left: Integer);", CodeRessource.PositionXYChange);
			}
			
			// Writing the unit
			StringBuilder code = new StringBuilder();
			code.AppendLine("unit "+device.CodeFriendlyName()+";");
			code.AppendLine();
			code.AppendLine("interface");
			code.AppendLine();
			foreach (KeyValuePair<String, String> function in functions) {
				code.AppendLine("procedure "+function.Key);
			}
			code.AppendLine();
			code.AppendLine("implementation");
			code.AppendLine();
			code.AppendLine("var");
			code.AppendLine("  dontRefresh: Boolean;");
			code.AppendLine();
			foreach (KeyValuePair<String, String> function in functions) {
				code.AppendLine("procedure "+function.Key);
				code.AppendLine(function.Value);
			}
			code.AppendLine("end.");
			codeEl.Add(code.ToString());
		}
	}
}
