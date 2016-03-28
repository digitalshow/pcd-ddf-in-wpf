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
using System.Windows.Media;

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDChannelItem.
	/// </summary>
	public class PCDChannelItem: PCDChannelSubset
	{
		public PCDChannelItem(PCDDeviceChannel channel): base(channel)
		{
			Name = "<Not named>".Localize();
			IconFileName = "";
		}
		
		public override void CheckValueBounds()
		{
			base.CheckValueBounds();
			if (MaxValue < SetValue)
				SetValue = MaxValue;
			if (MinValue > SetValue)
				SetValue = MinValue;
		}
		
		int setValue;
		
		public int SetValue {
			get { return setValue; }
			set { 
				SetProperty(ref setValue, value);
			}
		}
		
		String iconFileName;
		
		public String IconFileName {
			get { return iconFileName; }
			set { SetProperty(ref iconFileName, value); }
		}
	}
}
