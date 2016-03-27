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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDDevice.
	/// </summary>
	public class PCDDevice: PCDModelObject
	{
		public PCDDevice(): base()
		{
			Amber = new PCDDeviceAmber();
			Channels = new ObservableCollection<PCDDeviceChannel>();
			Channels.CollectionChanged += new NotifyCollectionChangedEventHandler(Channels_CollectionChanged);
			GUIElements = new ObservableCollection<Koinzer.pcdddfinwpf.Model.GUI.PCDDeviceElement>();
			Presets = new ObservableCollection<PCDDevicePreset>();
			
			Name = "New Device";
			Vendor = "Generic";
			Author = System.Environment.UserName;
			Description = "";
			Generator = "PCD-DDF-in-WPF";
			DeviceImageFileName = "";
			FormWidth = 300;
			FormHeight = 400;
			
			GUIElements.Add(new GUI.PCDDeviceImage(this) { Left = 10, Top = 10 });
			GUIElements.Add(new GUI.PCDDeviceName(this) { Left = 80, Top = 8 });
			GUIElements.Add(new GUI.PCDDeviceAddress(this) { Left = 80, Top = 24 });
			GUIElements.Add(new GUI.PCDDeviceDipSwitch(this) { Left = 80, Top = 40 });
		}

		void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			for (int i = 0; i < Channels.Count; i++)
				Channels[i].Channel = i;
		}
		
		public String CodeFriendlyName()
		{
			return (Vendor+"_"+Name).MakeCodeFriendly();
		}
		
		String name;
		
		public String Name {
			get { return name; }
			set { SetProperty(ref name, value); }
		}
		
		String vendor;
		
		public String Vendor {
			get { return vendor; }
			set { SetProperty(ref vendor, value); }
		}
		
		String author;
		
		public String Author {
			get { return author; }
			set { SetProperty(ref author, value); }
		}
		
		String description;
		
		public String Description {
			get { return description; }
			set { SetProperty(ref description, value); }
		}
		
		String generator;
		
		public String Generator {
			get { return generator; }
			set { SetProperty(ref generator, value); }
		}
		
		String deviceImageFileName;
		
		public String DeviceImageFileName {
			get { return deviceImageFileName; }
			set { SetProperty(ref deviceImageFileName, value); }
		}
		
		public ObservableCollection<PCDDeviceChannel> Channels { get; private set; }
		
		public PCDDeviceAmber Amber { get; private set; }
		
		int formWidth;
		
		public int FormWidth {
			get { return formWidth; }
			set { SetProperty(ref formWidth, value); }
		}
		
		int formHeight;
		
		public int FormHeight {
			get { return formHeight; }
			set { SetProperty(ref formHeight, value); }
		}
		
		public ObservableCollection<GUI.PCDDeviceElement> GUIElements { get; private set; }
		
		public ObservableCollection<PCDDevicePreset> Presets { get; private set; }
	}
}
