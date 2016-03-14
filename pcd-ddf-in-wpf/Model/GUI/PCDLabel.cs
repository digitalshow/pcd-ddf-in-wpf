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

namespace Koinzer.pcdddfinwpf.Model.GUI
{
	/// <summary>
	/// Description of PCDLabel.
	/// </summary>
	public class PCDLabel: PCDDeviceElement
	{
		public PCDLabel(PCDDevice device): base(device)
		{
			Caption = "Label";
		}
		
		public override string GetNodeName()
		{
			return "label";
		}
		
		String caption;
		
		public String Caption {
			get { return caption; }
			set { SetProperty(ref caption, value); }
		}
		
		public String Name {
			get {
				// TODO Updating
				return "label" + Parent.GUIElements.IndexOf(this);
			}
		}
	}
}
