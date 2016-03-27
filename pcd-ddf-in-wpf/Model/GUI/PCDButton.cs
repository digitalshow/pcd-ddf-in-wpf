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
	/// Description of PCDButton.
	/// </summary>
	public class PCDButton: PCDDeviceElement
	{
		public PCDButton(PCDDevice device): base(device)
		{
			SpecialButtonAction = PCDSpecialButtonAction.None;
		}
		
		public override string GetNodeName()
		{
			return "button";
		}
		
		protected override bool GetHasSize()
		{
			return true;
		}
		
		public string getPostfix()
		{
			return "button";
		}
		
		public virtual String getName()
		{
			if (SpecialButtonAction == PCDSpecialButtonAction.ChangeColorPicker)
				return "changecolorpicker_" + getPostfix();
			if (associatedPreset == null)
				return "unassociated_" + getPostfix();
			return associatedPreset.Name + "_" + getPostfix();
		}
		
		public String Name {
			get {
				return getName();
			}
		}
		
		String caption;
		
		public String Caption {
			get { return caption; }
			set { SetProperty(ref caption, value); }
		}
		
		PCDSpecialButtonAction specialButtonAction;
		
		public PCDSpecialButtonAction SpecialButtonAction {
			get { return specialButtonAction; }
			set { SetProperty(ref specialButtonAction, value); }
		}
		
		Model.PCDDevicePreset associatedPreset;
		
		public Model.PCDDevicePreset AssociatedPreset {
			get { return associatedPreset; }
			set { 
				SetProperty(ref associatedPreset, value);
				OnPropertyChanged("Name");
			}
		}
	}
}
