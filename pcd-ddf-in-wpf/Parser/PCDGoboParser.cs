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
using System.Linq;
using System.Windows.Media;
using System.Xml;

namespace Koinzer.pcdddfinwpf.Parser
{
	/// <summary>
	/// Description of PCDGoboParser.
	/// </summary>
	public class PCDGoboParser: PCDParser
	{
		public PCDGoboParser()
		{
		}
		
		public override void Parse(XmlDocument doc, Model.PCDDevice device, ParseResults results)
		{
			Parse(doc, device, "gobo1", "gobos");
			Parse(doc, device, "gobo2", "gobos2");
		}
		
		public void Parse(XmlDocument doc, Model.PCDDevice device, String channelName, String xmlNodeName)
		{
			XmlNode gobosNode = GetNode(doc, xmlNodeName);
			if (gobosNode == null)
				return;
			Model.PCDDeviceChannel channel = device.Channels.First(ch => ch.ChannelType.ToLower() == channelName);
			if (channel == null)
				return;
			foreach (XmlNode item in gobosNode.ChildNodes) {
				Model.PCDChannelItem subset;
				subset = new Model.PCDChannelItem(channel);
				subset.SetValue = int.Parse(item.Attributes["value"].Value);
				subset.MinValue = int.Parse(item.Attributes["value"].Value);
				subset.MaxValue = int.Parse(item.Attributes["valueend"].Value);
				subset.Name = item.Attributes["name"].Value;
				subset.IconFileName = item.Attributes["filename"].Value;
				channel.Subsets.Add(subset);
			}
		}
	}
}
