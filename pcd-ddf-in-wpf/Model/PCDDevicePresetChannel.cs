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

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDDevicePresetChannel.
	/// </summary>
	public class PCDDevicePresetChannel: PCDModelObject
	{
		public PCDDevicePresetChannel(PCDDevicePreset parent)
		{
			Parent = parent;
			Value = 0;
			FadeTime = 0;
			DelayTime = 0;
			MessageBoxMessage = "";
		}
		
		protected override System.Collections.IList GetContainingCollection()
		{
			if (Parent == null)
				return null;
			return Parent.Channels;
		}
		
		PCDDeviceChannel channel;
		
		public PCDDeviceChannel Channel {
			get { return channel; }
			set { SetProperty(ref channel, value); }
		}
		
		PCDChannelSubset subset;
		
		public PCDChannelSubset Subset {
			get { return subset; }
			set { SetProperty(ref subset, value); }
		}
		
		int _value;
		
		public int Value {
			get { return _value; }
			set { SetProperty(ref _value, value); }
		}
		
		int fadeTime;
		
		public int FadeTime {
			get { return fadeTime; }
			set { SetProperty(ref fadeTime, value); }
		}
		
		int delayTime;
		
		public int DelayTime {
			get { return delayTime; }
			set { SetProperty(ref delayTime, value); }
		}
		
		String messageBoxMessage;
		
		public String MessageBoxMessage {
			get { return messageBoxMessage; }
			set { SetProperty(ref messageBoxMessage, value); }
		}
		
		PCDDevicePreset parent;
		
		public PCDDevicePreset Parent {
			get { return parent; }
			set { SetProperty(ref parent, value); }
		}
	}
}
