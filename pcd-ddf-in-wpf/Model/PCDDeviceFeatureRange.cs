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
	/// Description of PCDDeviceFeatureRange.
	/// </summary>
	public class PCDDeviceFeatureRange: PCDDeviceFeature
	{
		public PCDDeviceFeatureRange(String featureName, String featureItemName, String xmlNodeName, String xmlAttributeMinName, String xmlAttributeMaxName, int initValue, int initMaxValue): base(featureName, featureItemName, xmlNodeName, xmlAttributeMinName, initValue)
		{
			InitMaxValue = initMaxValue;
			XmlAttributeMaxName = xmlAttributeMaxName;
		}
		
		int initMaxValue;
		
		public int InitMaxValue {
			get { return initMaxValue; }
			set { SetProperty(ref initMaxValue, value); }
		}
		
		String xmlAttributeMaxName;
		
		public String XmlAttributeMaxName {
			get { return xmlAttributeMaxName; }
			set { SetProperty(ref xmlAttributeMaxName, value); }
		}
	}
}
