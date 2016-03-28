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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Koinzer.pcdddfinwpf.Model;

namespace Koinzer.pcdddfinwpf.Controls
{
	/// <summary>
	/// Interaction logic for FeatureSelector.xaml
	/// </summary>
	public partial class FeatureSelector : UserControl
	{
		public FeatureSelector()
		{
			InitializeComponent();
		}
		
		void BoxNewFeature_Initialized(object sender, EventArgs e)
		{
			((ComboBox)sender).ItemsSource = Model.PCDDeviceFeatures.Instance.Features;
		}
		
		void Button_Click(object sender, RoutedEventArgs e)
		{
			PCDDeviceFeature feature = boxNewFeature.SelectedItem as PCDDeviceFeature;
			if (feature == null)
				return;
			if (Subset.Features.Contains(feature))
				return;
			if (!(feature is PCDDeviceFeatureRange) && (Subset is PCDChannelRange)) {
				MessageBox.Show("FeatureSelector.FeatureRangeErrorText".Localize(), "FeatureSelector.FeatureRangeErrorTitle".Localize(), MessageBoxButton.OK, MessageBoxImage.Error);
				return;
			}
			Subset.Features.Add(feature);
		}
		
		void Button_Click1(object sender, RoutedEventArgs e)
		{
			PCDDeviceFeature feature = ((Button)sender).Tag as PCDDeviceFeature;
			if (feature == null)
				return;
			Subset.Features.Remove(feature);
		}
		
		public static readonly DependencyProperty SubsetProperty =
			DependencyProperty.Register("Subset", typeof(PCDChannelSubset), typeof(FeatureSelector),
			                            new FrameworkPropertyMetadata());
		
		public PCDChannelSubset Subset {
			get { return (PCDChannelSubset)GetValue(SubsetProperty); }
			set { SetValue(SubsetProperty, value); }
		}
	}
}