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

namespace Koinzer.pcdddfinwpf.Writer
{
	/// <summary>
	/// Description of PCDPresetWriter.
	/// </summary>
	public class PCDPresetWriter
	{
		public PCDPresetWriter()
		{
		}
		
		public void Write(XElement presetsEl, Model.PCDDevice device, WriteResults results)
		{
			foreach (Model.PCDDevicePreset preset in device.Presets) {
				XElement presetEl = new XElement("preset", 
				                                 new XAttribute("name", preset.Name));
				foreach (Model.PCDDevicePresetChannel channel in preset.Channels) {
					if (channel.Channel == null) {
						results.AddMessage("Preset '{0}' contains an item that is not associated to a channel. It is ignored.");
						continue;
					}
					int subset = -1;
					int value = 0;
					if (channel.Subset != null) {
						subset = channel.Channel.Subsets.IndexOf(channel.Subset);
						if (channel.Subset is Model.PCDChannelItem) {
							value = ((Model.PCDChannelItem)channel.Subset).SetValue;
						} else
							value = channel.Subset.MinValue;
					} else {
						value = channel.Value;
					}
					XElement channelEl = new XElement("presetchannel", 
					                                  new XAttribute("channel", channel.Channel.Channel),
					                                  new XAttribute("subset", subset),
					                                  new XAttribute("value", value),
					                                  new XAttribute("fadetime", channel.FadeTime),
					                                  new XAttribute("delay", channel.DelayTime),
					                                  new XAttribute("message", channel.MessageBoxMessage));
					presetEl.Add(channelEl);
				}
				presetsEl.Add(presetEl);
			}
		}
	}
}
