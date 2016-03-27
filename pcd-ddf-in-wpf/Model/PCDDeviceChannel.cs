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

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDDeviceChannel.
	/// </summary>
	public class PCDDeviceChannel: PCDModelObject
	{
		public PCDDeviceChannel(PCDDevice parent): base()
		{
			Subsets = new ObservableCollection<PCDChannelSubset>();
			MinValue = 0;
			MaxValue = 255;
			InitValue = 0;
			ChannelType = "dimmer";
			Name = "<Not named>";
			Parent = parent;
		}
		
		protected override System.Collections.IList GetContainingCollection()
		{
			if (Parent == null)
				return null;
			return Parent.Channels;
		}
		
		public void SortSubsets() 
		{
			for (int i = 0; i < Subsets.Count; i++) {
				int k = i;
				for (int j = i; j < Subsets.Count; j++)
					if (Subsets[k].MinValue > Subsets[j].MinValue)
						k = j;
				if (k > i)
					Subsets.Move(k, i);				
			}
		}
		
		int channel;
		
		public int Channel {
			get { return channel; }
			set { SetProperty(ref channel, value); }
		}
		
		int minValue;
		
		public int MinValue {
			get { return minValue; }
			set { SetProperty(ref minValue, value); }
		}
		
		int maxValue;
		
		public int MaxValue {
			get { return maxValue; }
			set { SetProperty(ref maxValue, value); }
		}
		
		int initValue;
		
		public int InitValue {
			get { return initValue; }
			set { SetProperty(ref initValue, value); }
		}
		
		String name;
		
		public String Name {
			get { return name; }
			set { SetProperty(ref name, value); }
		}
		
		bool fade;
		
		public bool Fade {
			get { return fade; }
			set { SetProperty(ref fade, value); }
		}
		
		String channelType;
		
		public String ChannelType {
			get { return channelType; }
			set {
				if (Name == "<Not named>")
					Name = value.Substring(0,1).ToUpper() + value.Substring(1).ToLower();
				SetProperty(ref channelType, value.ToLower());
			}
		}
		
		public PCDDevice Parent { get; private set; }
		
		public ObservableCollection<PCDChannelSubset> Subsets { get; private set; }
	}
}
