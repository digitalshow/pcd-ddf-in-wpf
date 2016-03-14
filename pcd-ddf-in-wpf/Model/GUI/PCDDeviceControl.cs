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
using Prism.Mvvm;

namespace Koinzer.pcdddfinwpf.Model.GUI
{
	/// <summary>
	/// Description of PCDDeviceControl.
	/// </summary>
	public abstract class PCDDeviceControl: PCDDeviceElement
	{
		public PCDDeviceControl(PCDDevice device): base(device)
		{
		}
		
		public virtual String getName()
		{
			if (associatedChannel == null)
				return "unassociated_" + getPostfix();
			return associatedChannel.ChannelType + "_" + getPostfix();
		}
		
		public abstract String getPostfix();
		
		public String Name {
			get {
				return getName();
			}
		}
		
		Model.PCDDeviceChannel associatedChannel;
		
		public Model.PCDDeviceChannel AssociatedChannel {
			get { return associatedChannel; }
			set { 
				SetProperty(ref associatedChannel, value);
				OnPropertyChanged("Name");
			}
		}
	}
}
