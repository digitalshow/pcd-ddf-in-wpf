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
using System.Diagnostics;
using System.Xml;

namespace Koinzer.pcdddfinwpf.Parser
{
	/// <summary>
	/// Description of PCDDeviceAmberParser.
	/// </summary>
	public class PCDDeviceAmberParser: PCDParser
	{
		public PCDDeviceAmberParser()
		{
		}
		
		public override void Parse(XmlDocument doc, Model.PCDDevice device, ParseResults results)
		{
			Model.PCDDeviceAmber amber = device.Amber;
			XmlNode node = GetNode(doc, "amber");
			if (node == null)
				return;
			amber.UseAmberMixing = node.Attributes["UseAmberMixing"].Value == "yes";
			amber.CompensateRG = node.Attributes["AmberMixingCompensateRG"].Value == "yes";
			amber.CompensateRG = node.Attributes["AmberMixingCompensateBlue"].Value == "yes";
			amber.AmberColorR = int.Parse(node.Attributes["AmberColorR"].Value);
			amber.AmberColorG = int.Parse(node.Attributes["AmberColorG"].Value);
		}
	}
}
