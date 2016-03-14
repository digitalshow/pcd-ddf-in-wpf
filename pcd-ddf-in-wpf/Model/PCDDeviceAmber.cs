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

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDDeviceAmber.
	/// </summary>
	public class PCDDeviceAmber: PCDModelObject
	{
		public PCDDeviceAmber()
		{
			UseAmberMixing = true;
			CompensateRG = true;
			CompensateBlue = true;
			AmberColorR = 255;
			AmberColorG = 191;
		}
		
		bool useAmberMixing;
		
		public bool UseAmberMixing {
			get { return useAmberMixing; }
			set { SetProperty(ref useAmberMixing, value); }
		}
		
		bool compensateRG;
		
		public bool CompensateRG {
			get { return compensateRG; }
			set { SetProperty(ref compensateRG, value); }
		}
		
		bool compensateBlue;
		
		public bool CompensateBlue {
			get { return compensateBlue; }
			set { SetProperty(ref compensateBlue, value); }
		}
		
		int amberColorR;
		
		public int AmberColorR {
			get { return amberColorR; }
			set { SetProperty(ref amberColorR, value); }
		}
		
		int amberColorG;
		
		public int AmberColorG {
			get { return amberColorG; }
			set { SetProperty(ref amberColorG, value); }
		}
	}
}
