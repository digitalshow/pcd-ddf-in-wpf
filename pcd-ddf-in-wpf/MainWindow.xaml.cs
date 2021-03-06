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
using System.ComponentModel;
using System.Linq.Expressions;
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
	/// Interaction logic for Window1.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Strings.Culture = System.Globalization.CultureInfo.CurrentCulture;
			InitializeComponent();
			this.DataContext = Workspace.Instance;
			this.Closing += MainWindow_Closing;
		}
		
		void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = !Workspace.Instance.CanClose();
		}
	}
}