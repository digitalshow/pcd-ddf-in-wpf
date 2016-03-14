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
	/// Description of PCDGUICommands.
	/// </summary>
	public class PCDGUICommands
	{
		public PCDGUICommands()
		{
			AddDeviceAddress = new DelegateCommand<Model.PCDDevice>(DoAddDeviceAddress);
			AddDeviceDipswitch = new DelegateCommand<Model.PCDDevice>(DoAddDeviceDipswitch);
			AddDeviceImage = new DelegateCommand<Model.PCDDevice>(DoAddDeviceImage);
			AddDeviceName = new DelegateCommand<Model.PCDDevice>(DoAddDeviceName);
			AddColorPicker = new DelegateCommand<Model.PCDDevice>(DoAddAddColorPicker);
			AddDropdown = new DelegateCommand<Model.PCDDevice>(DoAddDropdown);
			AddLabel = new DelegateCommand<Model.PCDDevice>(DoAddLabel);
			AddPosition = new DelegateCommand<Model.PCDDevice>(DoAddPosition);
			AddSlider = new DelegateCommand<Model.PCDDevice>(DoAddSlider);
			Remove = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.GUI.PCDDeviceElement>(DoRemove, e => e != null));
		}
		
		public ICommand AddDeviceAddress { get; private set; }
		
		public ICommand AddDeviceDipswitch { get; private set; }
		
		public ICommand AddDeviceImage { get; private set; }
		
		public ICommand AddDeviceName { get; private set; }
		
		public ICommand AddColorPicker { get; private set; }
		
		public ICommand AddDropdown { get; private set; }
		
		public ICommand AddLabel { get; private set; }
		
		public ICommand AddPosition { get; private set; }
		
		public ICommand AddSlider { get; private set; }
		
		public ICommand Remove { get; private set; }
		
		public void DoAddDeviceAddress(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDDeviceAddress(device));
		}
		
		public void DoAddDeviceDipswitch(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDDeviceDipSwitch(device));
		}
		
		public void DoAddDeviceImage(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDDeviceImage(device));
		}
		
		public void DoAddDeviceName(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDDeviceName(device));
		}
		
		public void DoAddAddColorPicker(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDColorPicker(device));
		}
		
		public void DoAddDropdown(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDDropdown(device));
		}
		
		public void DoAddLabel(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDLabel(device));
		}
		
		public void DoAddPosition(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDPosition(device));
		}
		
		public void DoAddSlider(Model.PCDDevice device)
		{
			device.GUIElements.Add(new Model.GUI.PCDSlider(device));
		}
		
		public void DoRemove(Model.GUI.PCDDeviceElement element)
		{
			if (element.Parent == null)
				return;
			element.Parent.GUIElements.Remove(element);
		}
	}
}
