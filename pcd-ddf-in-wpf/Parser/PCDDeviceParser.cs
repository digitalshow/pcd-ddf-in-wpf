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
using System.Diagnostics;
using System.IO;
using System.Windows.Media;
using System.Xml;

namespace Koinzer.pcdddfinwpf.Parser
{
	/// <summary>
	/// Description of PCDDeviceParser.
	/// </summary>
	public class PCDDeviceParser: PCDParser
	{
		public PCDDeviceParser()
		{
		}
		
		public ParseResults Results { get; private set; }
		
		public Model.PCDDevice Load(String fileName)
		{
			Model.PCDDevice device = new Koinzer.pcdddfinwpf.Model.PCDDevice();
			XmlDocument doc = new XmlDocument();
			String xml = File.ReadAllText(fileName, System.Text.Encoding.GetEncoding("ISO-8859-1"));
			
			// Fix for wrong string concatenation of the original DDF editor
			xml = xml.Replace("\"AmberColorG=\"", "\" AmberColorG=\"");
			
			doc.LoadXml(xml);
			//doc.Load(fileName);
			ParseResults results = new ParseResults();
			Parse(doc, device, results);
			new PCDDeviceAmberParser().Parse(doc, device, results);
			new PCDColorParser().Parse(doc, device, results);
			new PCDGoboParser().Parse(doc, device, results);
			new PCDFeatureParser().Parse(doc, device, results);
			new PCDPresetParser().Parse(doc, device, results);
			new PCDGUIParser().Parse(doc, device, results);
			Results = results;
			return device;
		}
		
		public override void Parse(XmlDocument doc, Model.PCDDevice device, ParseResults results)
		{
			Debug.Assert(doc.DocumentElement.Name == "device");
			device.DeviceImageFileName = doc.DocumentElement.Attributes["image"].Value;
			
			device.Name = GetNodeContent(doc, "name");
			device.Vendor = GetNodeContent(doc, "vendor");
			device.Author = GetNodeContent(doc, "author");
			device.Description = GetNodeContent(doc, "description");
			device.Generator = GetNodeContent(doc, "generator");
			if (device.Generator != "PCD-DDF-in-WPF") {
				results.Messages.Add("PCDDeviceParser.UnknownGenerator".Localize());
			}
			
			XmlNode channels = GetNode(doc, "channels");
			XmlNode initvalues = GetNode(doc, "initvalues");
			Debug.Assert(channels != null);
			Debug.Assert(initvalues != null);
			foreach (XmlNode item in channels.ChildNodes) {
				Model.PCDDeviceChannel chan = new Koinzer.pcdddfinwpf.Model.PCDDeviceChannel(device);
				chan.Channel = int.Parse(item.Attributes["channel"].Value);
				chan.MinValue = int.Parse(item.Attributes["minvalue"].Value);
				chan.MaxValue = int.Parse(item.Attributes["maxvalue"].Value);
				chan.Name = item.Attributes["name"].Value;
				chan.Fade = item.Attributes["fade"].Value == "yes";
				chan.ChannelType = item.Attributes["type"].Value;
				chan.InitValue = int.Parse(initvalues.Attributes["ch"+item.Attributes["channel"].Value].Value);
				foreach (XmlNode subitem in item.ChildNodes) {
					Model.PCDChannelSubset subset;
					if (subitem.Name == "item") {
						Model.PCDChannelItem sitem = new Koinzer.pcdddfinwpf.Model.PCDChannelItem(chan);
						sitem.IconFileName = subitem.Attributes["icon"].Value;
						sitem.SetValue = int.Parse(subitem.Attributes["setvalue"].Value);
						subset = sitem;
					} else {
						Model.PCDChannelRange range = new Koinzer.pcdddfinwpf.Model.PCDChannelRange(chan);
						subset = range;
					}
					subset.Color = (Color)ColorConverter.ConvertFromString(subitem.Attributes["color"].Value);
					subset.JoinNext = subitem.Attributes["joinnext"].Value == "true";
					subset.MinValue = int.Parse(subitem.Attributes["minvalue"].Value);
					subset.MaxValue = int.Parse(subitem.Attributes["maxvalue"].Value);
					subset.Name = subitem.Attributes["name"].Value;
					chan.Subsets.Add(subset);
				}
				device.Channels.Add(chan);
			}
		}
	}
}
