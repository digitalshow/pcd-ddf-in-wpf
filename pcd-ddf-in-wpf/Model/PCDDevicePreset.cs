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
using System.Collections.ObjectModel;
using System.Linq;

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDDevicePreset.
	/// </summary>
	public class PCDDevicePreset: PCDModelObject
	{
		public PCDDevicePreset(PCDDevice parent)
		{
			Name = "<Not named>".Localize();
			Parent = parent;
			Channels = new ObservableCollection<PCDDevicePresetChannel>();
		}
		
		public PCDDevicePreset Clone()
		{
			PCDDevicePreset clone = new PCDDevicePreset(parent);
			int i = 1;
			do {
				clone.Name = this.Name + " " + i++;
			} while (Parent.Presets.FirstOrDefault(pr => pr.Name == clone.Name) != null);
			foreach (PCDDevicePresetChannel channel in this.Channels) {
				PCDDevicePresetChannel channelClone = new PCDDevicePresetChannel(clone);
				channelClone.Channel = channel.Channel;
				channelClone.DelayTime = channel.DelayTime;
				channelClone.MessageBoxMessage = channel.MessageBoxMessage;
				channelClone.Subset = channel.Subset;
				channelClone.Value = channel.Value;
				clone.Channels.Add(channelClone);
			}
			return clone;
		}
		
		protected override System.Collections.IList GetContainingCollection()
		{
			if (Parent == null)
				return null;
			return Parent.Presets;
		}
		
		String name;
		
		public String Name {
			get { return name; }
			set { SetProperty(ref name, value); }
		}
		
		public ObservableCollection<PCDDevicePresetChannel> Channels { get; private set; }
		
		PCDDevice parent;
		
		public PCDDevice Parent {
			get { return parent; }
			set { SetProperty(ref parent, value); }
		}
	}
}
