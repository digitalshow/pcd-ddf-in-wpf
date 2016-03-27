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
using System.Collections;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Prism.Mvvm;

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDModelObject.
	/// </summary>
	public class PCDModelObject: BindableBase
	{
		public PCDModelObject()
		{
		}
		
		protected virtual IList GetContainingCollection() 
		{
			return null;
		}
		
		public IList ContainingCollection { get { return GetContainingCollection(); } }
	}
}
