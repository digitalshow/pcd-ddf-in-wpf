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

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDChannelTypes.
	/// </summary>
	public class PCDChannelTypes
	{
		public PCDChannelTypes()
		{
			ChannelTypes = new String[] {
				"pan",
				"tilt",
				"panfine",
				"tiltfine",
				"moves", 
				"speed", 
				"gobo1", 
				"gobo1rot", 
				"gobo2", 
				"gobo2rot", 
				"gobo3", 
				"extra", 
				"color1", 
				"color2", 
				"r", 
				"g", 
				"b", 
				"iris", 
				"shutter", 
				"dimmer", 
				"zoom", 
				"focus", 
				"prisma", 
				"prismarot", 
				"frost", 
				"special1", 
				"special2", 
				"special3", 
				"special4", 
				"special5", 
				"special6", 
				"special7", 
				"special8", 
				"special9", 
				"special10", 
				"file", 
				"option", 
				"volume", 
				"picture", 
				"position", 
				"w", 
				"a", 
				"cyan", 
				"magenta", 
				"yellow", 
				"uv", 
				"fog"
			};
		}
		
		public String[] ChannelTypes { get; private set; }
	}
}
