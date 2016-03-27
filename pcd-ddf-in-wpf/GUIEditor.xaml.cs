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
	/// Interaction logic for GUIEditor.xaml
	/// </summary>
	public partial class GUIEditor : UserControl
	{
		public GUIEditor()
		{
			InitializeComponent();
		}
		
		Point offset;
		Point startPoint;
		bool moving = false;
		
		public void ElementClicked(Object sender, MouseButtonEventArgs e)
		{
			if (sender as FrameworkElement != null && ((FrameworkElement)sender).Tag as Model.GUI.PCDDeviceElement != null) {
				list.SelectedItem = ((FrameworkElement)sender).Tag;
				offset = e.GetPosition(sender as IInputElement);
				startPoint = e.GetPosition(itemsControl);
				moving = true;
			}
		}
		
		void ItemsControl_PreviewMouseMove(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed && moving) {
				Point point = e.GetPosition(sender as IInputElement);
				if ((point - startPoint).X < 5 && (point - startPoint).Y < 5)
					return;
				startPoint = new Point(-1000000, -1000000);
				Model.GUI.PCDDeviceElement element = list.SelectedItem as Model.GUI.PCDDeviceElement;
				if (element != null) {
					element.Left = ((int)(point.X - offset.X) / 5) * 5;
					element.Top = ((int)(point.Y - offset.Y) / 5) * 5;
				}
			} else {
				moving = false;
			}
		}
	}
}