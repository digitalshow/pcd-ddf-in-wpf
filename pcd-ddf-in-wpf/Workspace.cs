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
using System.Windows;
using System.Windows.Input;
using Prism.Mvvm;
using Prism.Commands;

namespace Koinzer.pcdddfinwpf
{
	/// <summary>
	/// Description of Workspace.
	/// </summary>
	public class Workspace: BindableBase
	{
		public Workspace()
		{
			New = new DelegateCommand(DoNew);
			LoadFromFile = new DelegateCommand(DoLoadFromFile);
			SaveToFile = new DelegateCommand(DoSaveToFile);
			CurrentFileName = "";
			if (PCDInstallationFinder.Instance.InstallationDirectory == null)
				MessageBox.Show("PC_DIMMER installation could not be found.", "PC_DIMMER not found", MessageBoxButton.OK, MessageBoxImage.Error);
		}
		
		#region Singleton
		
		private static Workspace _instance;
		
		public static Workspace Instance {
			get {
				if (_instance == null)
					_instance = new Workspace();
				return _instance;
			}
		}
		
		#endregion
		
		#region Commands
		
		public ICommand New { get; private set; }
		
		public void DoNew()
		{
			CurrentDevice = new Koinzer.pcdddfinwpf.Model.PCDDevice();
		}
		
		public ICommand LoadFromFile { get; private set; }
		
		public void DoLoadFromFile()
		{
			Microsoft.Win32.OpenFileDialog ofd = new Microsoft.Win32.OpenFileDialog();
			ofd.Filter = "PC_DIMMER DDF (*.pcddevc)|*.pcddevc|All files (*.*)|*.*";
			if (ofd.ShowDialog().GetValueOrDefault() == true)
				LoadDeviceFromFile(ofd.FileName);
		}
		
		public ICommand SaveToFile { get; private set; }
		
		public void DoSaveToFile()
		{
			Microsoft.Win32.SaveFileDialog sfd = new Microsoft.Win32.SaveFileDialog();
			sfd.Filter = "PC_DIMMER DDF (*.pcddevc)|*.pcddevc|All files (*.*)|*.*";
			String fileName = CurrentFileName;
			if (String.IsNullOrEmpty(CurrentFileName)) {
				fileName = CurrentDevice.CodeFriendlyName();
			}
			System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
			sfd.InitialDirectory = fi.DirectoryName;
			sfd.FileName = CurrentDevice.CodeFriendlyName();
			if (sfd.ShowDialog().GetValueOrDefault() == true)
				SaveDeviceToFile(sfd.FileName);
		}
		
		#endregion
		
		public void LoadDeviceFromFile(String fileName)
		{
			try {
				Parser.PCDDeviceParser parser = new Parser.PCDDeviceParser();
				CurrentDevice = parser.Load(fileName);
				if (parser.Results.Messages.Count > 0)
					MessageBox.Show("While loading, the following warnings were generated:\r\n\r\n- " +
					                String.Join("\r\n- ", parser.Results.Messages));
				CurrentFileName = fileName;
			} catch (Exception e) {
				MessageBox.Show("File could not be loaded. \r\n\r\n " + e, "Error loading file", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		
		public void SaveDeviceToFile(String fileName)
		{
			try {
				Writer.PCDDeviceWriter writer = new Koinzer.pcdddfinwpf.Writer.PCDDeviceWriter();
				Writer.WriteResults results = writer.Save(CurrentDevice, fileName);
				if (results.Messages.Count > 0)
					MessageBox.Show("While saving, the following warnings were generated:\r\n\r\n- " +
					                String.Join("\r\n- ", results.Messages));
				CurrentFileName = fileName;
			} catch (Exception e) {
				MessageBox.Show("File could not be saved. \r\n\r\n " + e, "Error saving file", MessageBoxButton.OK, MessageBoxImage.Error);
			}
		}
		
		Model.PCDDevice currentDevice;
		
		public Model.PCDDevice CurrentDevice {
			get { return currentDevice; }
			set { SetProperty(ref currentDevice, value); }
		}
		
		String currentFileName;
		
		public String CurrentFileName {
			get { return currentFileName; }
			set { SetProperty(ref currentFileName, value); }
		}
	}
}
