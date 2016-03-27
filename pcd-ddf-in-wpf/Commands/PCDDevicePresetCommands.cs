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
using System.Windows.Input;
using Prism.Commands;

namespace Koinzer.pcdddfinwpf.Commands
{
	/// <summary>
	/// Description of PCDDevicePresetCommands.
	/// </summary>
	public class PCDDevicePresetCommands
	{
		public PCDDevicePresetCommands()
		{
			AddPreset = new DelegateCommand<Model.PCDDevice>(DoAddPreset);
			AddPresetChannel = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDDevicePreset>(DoAddPresetChannel, pr => pr != null));
			Clone = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDDevicePreset>(DoClone, pr => pr != null));
			Remove = new DelegateCommand<Model.PCDModelObject>(DoRemove);
		}
		
		public ICommand AddPreset { get; private set; }
		
		public void DoAddPreset(Model.PCDDevice device)
		{
			device.Presets.Add(new Model.PCDDevicePreset(device));
		}
		
		public ICommand AddPresetChannel { get; private set; }
		
		public void DoAddPresetChannel(Model.PCDDevicePreset preset)
		{
			preset.Channels.Add(new Model.PCDDevicePresetChannel(preset));
		}
		
		public ICommand Clone { get; private set; }
		
		public void DoClone(Model.PCDDevicePreset preset)
		{
			preset.ContainingCollection.Add(preset.Clone());
		}
		
		public ICommand Remove { get; private set; }
		
		public void DoRemove(Model.PCDModelObject item)
		{
			if (item is Model.PCDDevicePreset) {
				Model.PCDDevicePreset preset = (Model.PCDDevicePreset)item;
				preset.Parent.Presets.Remove(preset);
			}
			if (item is Model.PCDDevicePresetChannel) {
				Model.PCDDevicePresetChannel channel = (Model.PCDDevicePresetChannel)item;
				channel.Parent.Channels.Remove(channel);
			}
		}
	}
}
