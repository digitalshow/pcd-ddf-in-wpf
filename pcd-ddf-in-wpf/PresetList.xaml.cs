﻿/* 

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
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace Koinzer.pcdddfinwpf
{
	/// <summary>
	/// Interaction logic for PresetList.xaml
	/// </summary>
	public partial class PresetList : UserControl
	{
		public PresetList()
		{
			InitializeComponent();
		}
		
		void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
		{
			Object selected = ((TreeView)sender).SelectedItem;
			if (selected is Model.PCDDevicePreset)
				SelectedPreset = selected as Model.PCDDevicePreset;
			else if (selected is Model.PCDDevicePresetChannel)
				SelectedPreset = ((Model.PCDDevicePresetChannel)selected).Parent;
			else
				SelectedPreset = null;
			Selected = selected as Model.PCDModelObject;
		}
		
		public static readonly DependencyProperty SelectedPresetProperty =
			DependencyProperty.Register("SelectedPreset", typeof(Model.PCDDevicePreset), typeof(PresetList),
			                            new FrameworkPropertyMetadata());
		
		public Model.PCDDevicePreset SelectedPreset {
			get { return (Model.PCDDevicePreset)GetValue(SelectedPresetProperty); }
			set { SetValue(SelectedPresetProperty, value); }
		}
		
		public static readonly DependencyProperty SelectedProperty =
			DependencyProperty.Register("Selected", typeof(Model.PCDModelObject), typeof(PresetList),
			                            new FrameworkPropertyMetadata());
		
		public Model.PCDModelObject Selected {
			get { return (Model.PCDModelObject)GetValue(SelectedProperty); }
			set { SetValue(SelectedProperty, value); }
		}
	}
}