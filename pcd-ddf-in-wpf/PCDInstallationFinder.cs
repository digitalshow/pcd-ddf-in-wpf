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
using System.IO;
using Microsoft.Win32;

namespace Koinzer.pcdddfinwpf
{
	/// <summary>
	/// Description of PCDInstallationFinder.
	/// </summary>
	public class PCDInstallationFinder
	{
		public PCDInstallationFinder()
		{
		}
		
		#region Singleton
		
		private static PCDInstallationFinder _instance;
		
		public static PCDInstallationFinder Instance {
			get {
				if (_instance == null)
					_instance = new PCDInstallationFinder();
				return _instance;
			}
		}
		
		#endregion
		
		private String FindInstallationDirectory()
		{
			String path = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) +
				"\\PHOENIXstudios\\PC_DIMMER";
			String cmd = Registry.GetValue("HKEY_CLASSES_ROOT\\PC_DIMMER_File\\shell\\open\\command", "", null) as String;
			if (cmd != null) {
				path = cmd.Replace(" \"%1\"", "").Replace("\"", "");
				FileInfo fi = new FileInfo(path);
				path = fi.DirectoryName;
			}
			if (Directory.Exists(path))
				return path;
			return null;
		}
		
		String installationDirectory = null;
		
		public String InstallationDirectory {
			get {
				if (installationDirectory == null) {
					installationDirectory = FindInstallationDirectory();
				}
				return installationDirectory;
			}
		}
	}
}
