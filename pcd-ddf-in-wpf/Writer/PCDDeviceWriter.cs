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
using System.Xml.Linq;

namespace Koinzer.pcdddfinwpf.Writer
{
	/// <summary>
	/// Description of PCDDeviceWriter.
	/// </summary>
	public class PCDDeviceWriter
	{
		public PCDDeviceWriter()
		{
		}
		
		public WriteResults Save(Model.PCDDevice device, String fileName)
		{
			WriteResults results = new WriteResults();
			XDocument doc = new XDocument();
			doc.Declaration = new XDeclaration("1.0", "ISO-8859-1", "yes");
			XElement nodeDevice = new XElement("device", new XAttribute("image", device.DeviceImageFileName));
			nodeDevice.Add(
				new XElement("information", 
				             new XAttribute("id", "PC_DIMMER"),
				             new XElement("name", device.Name),
				             new XElement("vendor", device.Vendor),
				             new XElement("author", device.Author),
				             new XElement("description", device.Description),
				             new XElement("generator", "PCD-DDF-in-WPF")));
			XElement channels = new XElement("channels");
			XElement initvalues = new XElement("initvalues");
			foreach (Model.PCDDeviceChannel channel in device.Channels)
			{
				XElement function = new XElement("function",
					             new XAttribute("channel", channel.Channel),
					             new XAttribute("minvalue", channel.MinValue),
					             new XAttribute("maxvalue", channel.MaxValue),
					             new XAttribute("name", channel.Name),
					             new XAttribute("fade", channel.Fade ? "yes" : "no"),
					             new XAttribute("type", channel.ChannelType));
				if (channel.ChannelType != "color1"
				    && channel.ChannelType != "color2"
				    && channel.ChannelType != "gobo1"
				    && channel.ChannelType != "gobo2") {
					foreach (Model.PCDChannelSubset subset in channel.Subsets) {
						if (subset is Model.PCDChannelItem) {
							Model.PCDChannelItem item = (Model.PCDChannelItem)subset;
							function.Add(new XElement("item",
							                          new XAttribute("minvalue", item.MinValue),
							                          new XAttribute("maxvalue", item.MaxValue),
							                          new XAttribute("setvalue", item.SetValue),
							                          new XAttribute("joinnext", item.JoinNext),
							                          new XAttribute("color", item.Color.ToString()),
							                          new XAttribute("icon", item.IconFileName),
							                          new XAttribute("name", item.Name)));
						} else {
							Model.PCDChannelRange range = (Model.PCDChannelRange)subset;
							function.Add(new XElement("range",
							                          new XAttribute("minvalue", range.MinValue),
							                          new XAttribute("maxvalue", range.MaxValue),
							                          new XAttribute("joinnext", range.JoinNext),
							                          new XAttribute("color", range.Color.ToString()),
							                          new XAttribute("name", range.Name)));						
						}
					}
				}
				channels.Add(function);
				initvalues.Add(new XAttribute("ch"+channel.Channel, channel.InitValue));
			}
			nodeDevice.Add(channels);
			nodeDevice.Add(initvalues);
			
			new PCDColorsWriter().Write(nodeDevice, device, results);
			new PCDGobosWriter().Write(nodeDevice, device, results);
			new PCDFeatureWriter().Write(nodeDevice, device, results);
			
			XElement presets = new XElement("presets");
			new PCDPresetWriter().Write(presets, device, results);
			nodeDevice.Add(presets);
			
			XElement form = new XElement("form",
			                             new XAttribute("width", device.FormWidth),
			                             new XAttribute("height", device.FormHeight));
			new PCDGUIWriter().Write(form, device, results);
			nodeDevice.Add(form);
			
			XElement code = new XElement("code");
			new PCDCodeWriter().Write(code, device, results);
			nodeDevice.Add(code);
			
			doc.Add(nodeDevice);
			doc.Save(fileName);
			return results;
		}
	}
}
