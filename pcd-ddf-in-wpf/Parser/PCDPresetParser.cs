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

namespace Koinzer.pcdddfinwpf.Parser
{
	/// <summary>
	/// Description of PCDPresetParser.
	/// </summary>
	public class PCDPresetParser: PCDParser
	{
		public PCDPresetParser()
		{
		}
		
		public override void Parse(XmlDocument doc, Model.PCDDevice device, ParseResults results)
		{
			XmlNode node = GetNode(doc, "presets");
			if (node == null)
				return;
			foreach (XmlNode childNode in node.ChildNodes) {
				if (childNode.Name != "preset")
					continue;
				Model.PCDDevicePreset preset = new Koinzer.pcdddfinwpf.Model.PCDDevicePreset(device);
				preset.Name = childNode.Attributes["name"].Value;
				foreach (XmlNode subchildNode in childNode.ChildNodes) {
					if (subchildNode.Name != "presetchannel")
						continue;
					
					Model.PCDDevicePresetChannel channel = new Koinzer.pcdddfinwpf.Model.PCDDevicePresetChannel(preset);
					int channelNum = int.Parse(subchildNode.Attributes["channel"].Value);
					channel.Channel = device.Channels.FirstOrDefault(ch => ch.Channel == channelNum);
					int subsetNum = int.Parse(subchildNode.Attributes["subset"].Value);
					channel.FadeTime = int.Parse(subchildNode.Attributes["fadetime"].Value);
					channel.DelayTime = int.Parse(subchildNode.Attributes["delay"].Value);
					channel.MessageBoxMessage = subchildNode.Attributes["message"].Value;
					if ((channel.Channel != null) &&
					    (subsetNum >= 0) && 
					    (subsetNum < channel.Channel.Subsets.Count))
						channel.Subset = channel.Channel.Subsets[subsetNum];
					
					channel.Value = int.Parse(subchildNode.Attributes["value"].Value);
					preset.Channels.Add(channel);
				}
				device.Presets.Add(preset);
			}
		}
	}
}
