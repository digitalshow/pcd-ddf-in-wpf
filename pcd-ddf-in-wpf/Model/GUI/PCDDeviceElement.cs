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
using Prism.Mvvm;

namespace Koinzer.pcdddfinwpf.Model.GUI
{
	/// <summary>
	/// Description of PCDDeviceElement.
	/// </summary>
	public abstract class PCDDeviceElement: PCDModelObject
	{
		public PCDDeviceElement(PCDDevice parent)
		{
			Parent = parent;
			Left = 10;
			Top = 10;
			Width = 180;
			Height = 25;
		}
		
		public abstract String GetNodeName();
		
		protected virtual bool GetHasSize()
		{
			return true;
		}
		
		public bool HasSize {
			get { return GetHasSize(); }
		}
		
		int left;
		
		public int Left {
			get { return left; }
			set { SetProperty(ref left, value); }
		}
		
		int top;
		
		public int Top {
			get { return top; }
			set { SetProperty(ref top, value); }
		}
		
		int width;
		
		public int Width {
			get { return width; }
			set { SetProperty(ref width, value); }
		}
		
		int height;
		
		public int Height {
			get { return height; }
			set { SetProperty(ref height, value); }
		}
		
		public PCDDevice Parent { get; private set; }
	}
}
