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
using System.Linq;
using System.Xml;
using Koinzer.pcdddfinwpf.Model;

namespace Koinzer.pcdddfinwpf.Parser
{
	/// <summary>
	/// Description of PCDGUIParser.
	/// </summary>
	public class PCDGUIParser: PCDParser
	{
		public PCDGUIParser()
		{
		}
		
		public override void Parse(XmlDocument doc, Model.PCDDevice device, ParseResults results)
		{
			XmlNode formNode = GetNode(doc, "form");
			if (formNode == null)
				return;
			device.FormWidth = int.Parse(formNode.Attributes["width"].Value);
			device.FormHeight = int.Parse(formNode.Attributes["height"].Value);
			device.GUIElements.Clear();
			foreach (XmlNode node in formNode.ChildNodes) {
				Model.GUI.PCDDeviceElement el = null;
				switch (node.Name) {
					case "deviceimage":
						el = new Model.GUI.PCDDeviceImage(device);
						goto case "_wh";
					case "devicename":
						el = new Model.GUI.PCDDeviceName(device);
						goto case "_lt";
					case "deviceadress":
						el = new Model.GUI.PCDDeviceAddress(device);
						goto case "_lt";
					case "devicedipswitch":
						el = new Model.GUI.PCDDeviceDipSwitch(device);
						goto case "_lt";
					case "position":
						el = new Model.GUI.PCDPosition(device);
						goto case "_wh";
					case "colorbox":
						var colorbox = new Model.GUI.PCDDropdown(device);
						el = colorbox;
						colorbox.AssociatedChannel = device.Channels.FirstOrDefault(c => c.ChannelType == "color1");
						goto case "_wh";
					case "colorbox2":
						var colorbox2 = new Model.GUI.PCDDropdown(device);
						el = colorbox2;
						colorbox2.AssociatedChannel = device.Channels.FirstOrDefault(c => c.ChannelType == "color2");
						goto case "_wh";
					case "colorpicker":
						el = new Model.GUI.PCDColorPicker(device);
						goto case "_lt";
					case "slider":
						var slider = new Model.GUI.PCDSlider(device);
						el = slider;
						AssociateChannelByAction(node, slider);
						goto case "_wh";
					case "label":
						var label = new Model.GUI.PCDLabel(device);
						el = label;
						label.Caption = node.Attributes["caption"].Value;
						goto case "_wh";
					case "button":
						var button = new Model.GUI.PCDButton(device);
						el = button;
						AssociatePresetByAction(node, button);
						if (button.SpecialButtonAction == Model.GUI.PCDSpecialButtonAction.ChangeColorPicker)
							button.Caption = node.Attributes["caption"].Value;
						goto case "_wh";
					case "dropdown":
						if (node.Attributes["name"].Value == "presetSelector") {
							el = new Model.GUI.PCDPresetSelector(device);
							goto case "_wh";
						}
						var dropdown = new Model.GUI.PCDDropdown(device);
						el = dropdown;
						AssociateChannelByAction(node, dropdown);
						goto case "_wh";
					case "edit":
						if (node.Attributes["name"].Value == "fadezeit") {
							var edit = new Model.GUI.PCDFadetimeEdit(device);
							el = edit;
							edit.Value = int.Parse(node.Attributes["text"].Value);
							goto case "_wh";
						}
						goto default;
					case "checkbox":
						if (node.Attributes["name"].Value == "usefadezeit") {
							var checkbox = new Model.GUI.PCDFadetimeCheckbox(device);
							el = checkbox;
							checkbox.Checked = (node.Attributes["checked"].Value == "true");
							checkbox.Caption = node.Attributes["caption"].Value;							
							goto case "_wh";
						}
						goto default;
					default:
						System.Diagnostics.Debug.WriteLine("Unknown form node: " + node.Name);
						results.Messages.Add("The type '"+node.Name+"' is not supported yet.");
						break;
					case "_wh": // For all sized nodes
						el.Width = int.Parse(node.Attributes["width"].Value);
						el.Height = int.Parse(node.Attributes["height"].Value);
						goto case "_lt";
					case "_lt": // For all positioned nodes
						el.Left = int.Parse(node.Attributes["left"].Value);
						el.Top = int.Parse(node.Attributes["top"].Value);
						break;
				}
				if (el != null) {
					device.GUIElements.Add(el);
				}
			}
		}
		
		protected void AssociateChannelByAction(XmlNode node, Model.GUI.PCDDeviceControl ctrl)
		{
			String actionName = node.Attributes["action"].Value;
			Model.PCDDeviceChannel associatedChannel = null;
			int matchLength = 0;
			foreach (Model.PCDDeviceChannel channel in ctrl.Parent.Channels) {
				if (actionName.ToLower().Contains(channel.ChannelType) &&
				    (matchLength < channel.ChannelType.Length)) {
					associatedChannel = channel;
					matchLength = channel.ChannelType.Length;
				}
			}
			ctrl.AssociatedChannel = associatedChannel;			
		}
		
		protected void AssociatePresetByAction(XmlNode node, Model.GUI.PCDButton ctrl)
		{
			String actionName = node.Attributes["action"].Value;
			if (actionName == "onchangecolorpicker") {
				ctrl.SpecialButtonAction = Model.GUI.PCDSpecialButtonAction.ChangeColorPicker;
				return;
			}
			if (actionName.Length <= "call_preset_".Length)
				return;
			actionName = actionName.Substring("call_preset_".Length);
			ctrl.AssociatedPreset = ctrl.Parent.Presets.FirstOrDefault(pr => Model.CodeHelper.MakeCodeFriendly(pr.Name).ToLower() == actionName);
		}
	}
}
