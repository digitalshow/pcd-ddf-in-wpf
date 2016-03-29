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
	/// Interaction logic for IntegerUpDown.xaml
	/// </summary>
	public partial class IntegerUpDown : UserControl
	{
		public IntegerUpDown()
		{
			InitializeComponent();
		}
		
		void TextBox_MouseWheel(object sender, MouseWheelEventArgs e)
		{
			int delta = (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? 10 : 1;
			InternalValue += (e.Delta > 0) ? delta : -delta;
		}
		
		void Button_Click(object sender, RoutedEventArgs e)
		{
			int delta = (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? 10 : 1;
			InternalValue += delta;
		}
		
		void Button_Click1(object sender, RoutedEventArgs e)
		{
			int delta = (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift)) ? 10 : 1;
			InternalValue -= delta;
		}
		
		void UpdateButtonIsEnabled()
		{
			upBtn.IsEnabled = Maximum > Value;
			downBtn.IsEnabled = Minimum < Value;
		}
		
		bool externalUpdate = false;
		
		static void ValueChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			IntegerUpDown obj = sender as IntegerUpDown;
			if (obj == null)
				return;
			if (obj.InternalValue == (int)e.NewValue)
				return;
			obj.externalUpdate = true;
			obj.InternalValue = (int)e.NewValue;
			obj.externalUpdate = false;
		}
		
		static void InternalValueChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			IntegerUpDown obj = sender as IntegerUpDown;
			if (obj == null)
				return;
			if (obj.externalUpdate)
				return;
			if ((int)e.NewValue > obj.Maximum)
				obj.InternalValue = obj.Maximum;
			if ((int)e.NewValue < obj.Minimum)
				obj.InternalValue = obj.Minimum;
			obj.Value = obj.InternalValue;
			obj.UpdateButtonIsEnabled();
		}
		
		static void MinMaxChanged(Object sender, DependencyPropertyChangedEventArgs e)
		{
			IntegerUpDown obj = sender as IntegerUpDown;
			if (sender == null)
				return;
			obj.UpdateButtonIsEnabled();
		}
		
		void Textbox_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			int delta = e.KeyboardDevice.Modifiers.HasFlag(ModifierKeys.Shift) ? 10 : 1;
			if (e.Key == Key.Down) {
				InternalValue -= delta;
				e.Handled = true;
			}
			if (e.Key == Key.Up) {
				InternalValue += delta;
				e.Handled = true;
			}
		}
		
		public static readonly DependencyProperty ValueProperty =
			DependencyProperty.Register("Value", typeof(int), typeof(IntegerUpDown),
			                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, ValueChanged));
		
		public int Value {
			get { return (int)GetValue(ValueProperty); }
			set {
				SetValue(ValueProperty, value); 
			}
		}
		
		private static readonly DependencyProperty InternalValueProperty =
			DependencyProperty.Register("InternalValue", typeof(int), typeof(IntegerUpDown),
			                            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, InternalValueChanged));
		
		private int InternalValue {
			get { return (int)GetValue(InternalValueProperty); }
			set { SetValue(InternalValueProperty, value); }
		}
		
		public static readonly DependencyProperty MinimumProperty =
			DependencyProperty.Register("Minimum", typeof(int), typeof(IntegerUpDown),
			                            new FrameworkPropertyMetadata(0, MinMaxChanged));
		
		public int Minimum {
			get { return (int)GetValue(MinimumProperty); }
			set { SetValue(MinimumProperty, value); }
		}
		
		public static readonly DependencyProperty MaximumProperty =
			DependencyProperty.Register("Maximum", typeof(int), typeof(IntegerUpDown),
			                            new FrameworkPropertyMetadata(int.MaxValue, MinMaxChanged));
		
		public int Maximum {
			get { return (int)GetValue(MaximumProperty); }
			set { SetValue(MaximumProperty, value); }
		}
	}
}