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
using System.Xml.Linq;
using Koinzer.pcdddfinwpf.Model;

namespace Koinzer.pcdddfinwpf.Writer
{
	/// <summary>
	/// Description of PCDGobosWriter.
	/// </summary>
	public class PCDGobosWriter
	{
		public PCDGobosWriter()
		{
		}
		
		public void Write(XElement devEl, PCDDevice device, WriteResults results)
		{
			WriteGobos("gobo1", "gobos", "gobo", devEl, device, results);
			WriteGobos("gobo2", "gobos2", "gobo2", devEl, device, results);
		}
		
		public void WriteGobos(String channelName, String nodeName, String subNodeName, XElement devEl, PCDDevice device, WriteResults results)
		{
			PCDDeviceChannel channel = device.Channels.FirstOrDefault(ch => ch.ChannelType == channelName);
			if (channel == null)
				return;
			XElement coEl = new XElement(nodeName);
			
			foreach (PCDChannelSubset subset in channel.Subsets) {
				coEl.Add(new XElement(subNodeName, 
				                      new XAttribute("name", subset.Name),
				                      new XAttribute("value", subset.MinValue),
				                      new XAttribute("valueend", subset.MaxValue),
				                      new XAttribute("filename", (subset is PCDChannelItem) ? ((PCDChannelItem)subset).IconFileName : "")));
			}
			
			devEl.Add(coEl);
		}
	}
}
