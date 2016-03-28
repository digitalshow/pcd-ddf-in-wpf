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
	/// Interaction logic for RelativeFileTextBox.xaml
	/// </summary>
	public partial class RelativeFileTextBox : UserControl
	{
		public RelativeFileTextBox()
		{
			InitializeComponent();
		}
		
		void Button_Click(object sender, RoutedEventArgs e)
		{
			Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.Filter = "PngImage".Localize()+" (*.png)|*.png|"+"AllFiles".Localize()+" (*.*)|*.*";
			String folder = PCDInstallationFinder.Instance.InstallationDirectory + RelativeFolder;
			ofd.InitialDirectory = folder;
			if (ofd.ShowDialog().GetValueOrDefault() == true) {
				if (ofd.FileName.ToLower().StartsWith(folder.ToLower())) {
					FileName = ofd.FileName.Substring(folder.Length).TrimStart('\\');
				} else
					FileName = ofd.FileName;
			}				
		}
		
		public static readonly DependencyProperty FileNameProperty =
			DependencyProperty.Register("FileName", typeof(String), typeof(RelativeFileTextBox),
			                            new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
		
		public String FileName {
			get { return (String)GetValue(FileNameProperty); }
			set { SetValue(FileNameProperty, value); }
		}
		
		public static readonly DependencyProperty RelativeFolderProperty =
			DependencyProperty.Register("RelativeFolder", typeof(String), typeof(RelativeFileTextBox),
			                            new FrameworkPropertyMetadata());
		
		public String RelativeFolder {
			get { return (String)GetValue(RelativeFolderProperty); }
			set { SetValue(RelativeFolderProperty, value); }
		}
	}
}