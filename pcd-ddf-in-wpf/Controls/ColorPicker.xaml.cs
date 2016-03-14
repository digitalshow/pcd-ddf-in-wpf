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

namespace Koinzer.pcdddfinwpf.Controls
{
	/// <summary>
	/// Interaction logic for ColorPicker.xaml
	/// </summary>
	public partial class ColorPicker : UserControl
	{
		public ColorPicker()
		{
			InitializeComponent();
		}
		
		void Button_Click(object sender, RoutedEventArgs e)
		{
			if (SelectedColor == Colors.Transparent)
				SelectedColor = Colors.Black;
		}
		
		static void RGBChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker obj = sender as ColorPicker;
			if (obj == null)
				return;
			if (obj.updating)
				return;
			obj.updating = true;
			obj.SelectedColor = Color.FromRgb((byte)obj.R, (byte)obj.G, (byte)obj.B);
			obj.updating = false;
		}
		
		static void SelectedColorChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			ColorPicker obj = sender as ColorPicker;
			if (obj == null)
				return;
			if (obj.updating)
				return;
			Color color = (Color)e.NewValue;
			obj.updating = true;
			obj.R = color.R;
			obj.G = color.G;
			obj.B = color.B;
			obj.updating = false;
		}
		
		bool updating = false;
		
		public static readonly DependencyProperty RProperty =
			DependencyProperty.Register("R", typeof(int), typeof(ColorPicker),
			                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RGBChanged));
		
		public int R {
			get { return (int)GetValue(RProperty); }
			set { SetValue(RProperty, value); }
		}
		
		public static readonly DependencyProperty GProperty =
			DependencyProperty.Register("G", typeof(int), typeof(ColorPicker),
			                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RGBChanged));
		
		public int G {
			get { return (int)GetValue(GProperty); }
			set { SetValue(GProperty, value); }
		}
		
		public static readonly DependencyProperty BProperty =
			DependencyProperty.Register("B", typeof(int), typeof(ColorPicker),
			                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, RGBChanged));
		
		public int B {
			get { return (int)GetValue(BProperty); }
			set { SetValue(BProperty, value); }
		}
		
		public static readonly DependencyProperty SelectedColorProperty =
			DependencyProperty.Register("SelectedColor", typeof(Color), typeof(ColorPicker),
			                            new FrameworkPropertyMetadata(Colors.Black, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, SelectedColorChanged));
		
		public Color SelectedColor {
			get { return (Color)GetValue(SelectedColorProperty); }
			set { SetValue(SelectedColorProperty, value); }
		}
	}
}