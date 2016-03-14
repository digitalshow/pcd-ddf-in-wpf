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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDDeviceFeatures.
	/// </summary>
	public class PCDDeviceFeatures
	{
		public PCDDeviceFeatures()
		{
			_features = new ObservableCollection<PCDDeviceFeature>();
			Features = new ReadOnlyObservableCollection<PCDDeviceFeature>(_features);
			_features.Add(new PCDDeviceFeature("None", "", "", "", 0));
			
			_features.Add(new PCDDeviceFeature("Shutter", "Closed", "shutter", "CloseValue", 0));
			_features.Add(new PCDDeviceFeature("Shutter", "Open", "shutter", "OpenValue", 255));
			
			_features.Add(new PCDDeviceFeature("Strobe", "Off", "strobe", "OffValue", 0));
			_features.Add(new PCDDeviceFeatureRange("Strobe", "Range Min-Max", "strobe", "MinValue", "MaxValue", 0, 255));
			
			_features.Add(new PCDDeviceFeatureRange("Dimmer", "Range Off-Max", "dimmer", "OffValue", "MaxValue", 0, 255));
			
			_features.Add(new PCDDeviceFeature("Gobo 1 Rotation", "Off", "gobo1rot", "OffValue", 128));
			_features.Add(new PCDDeviceFeatureRange("Gobo 1 Rotation", "Range Left Min-Max", "gobo1rot", "LeftMinValue", "LeftMaxValue", 0, 127));
			_features.Add(new PCDDeviceFeatureRange("Gobo 1 Rotation", "Range Right Min-Max", "gobo1rot", "RightMinValue", "RightMaxValue", 129, 255));
			
			_features.Add(new PCDDeviceFeature("Gobo 2 Rotation", "Off", "gobo2rot", "OffValue", 128));
			_features.Add(new PCDDeviceFeatureRange("Gobo 2 Rotation", "Range Left Min-Max", "gobo2rot", "LeftMinValue", "LeftMaxValue", 0, 127));
			_features.Add(new PCDDeviceFeatureRange("Gobo 2 Rotation", "Range Right Min-Max", "gobo2rot", "RightMinValue", "RightMaxValue", 129, 255));
			
			_features.Add(new PCDDeviceFeature("Prisma Rotation", "Off", "prismarot", "OffValue", 128));
			_features.Add(new PCDDeviceFeatureRange("Prisma Rotation", "Range Left Min-Max", "prismarot", "LeftMinValue", "LeftMaxValue", 0, 127));
			_features.Add(new PCDDeviceFeatureRange("Prisma Rotation", "Range Right Min-Max", "prismarot", "RightMinValue", "RightMaxValue", 129, 255));
			_features.Add(new PCDDeviceFeature("Prisma", "Single", "prisma", "SingleValue", 0));
			_features.Add(new PCDDeviceFeature("Prisma", "Triple", "prisma", "TripleValue", 100));
			
			_features.Add(new PCDDeviceFeature("Iris", "Closed", "iris", "CloseValue", 0));
			_features.Add(new PCDDeviceFeature("Iris", "Open", "iris", "OpenValue", 255));
			_features.Add(new PCDDeviceFeatureRange("Iris", "Range Min-Max", "iris", "MinValue", "MaxValue", 1, 254));
		}
		
		#region Singleton
		
		private static PCDDeviceFeatures _instance;
		
		public static PCDDeviceFeatures Instance {
			get {
				if (_instance == null)
					_instance = new PCDDeviceFeatures();
				return _instance;
			}
		}
		
		#endregion
		
		private ObservableCollection<PCDDeviceFeature> _features;
		
		public ReadOnlyObservableCollection<PCDDeviceFeature> Features { get; private set; }
		
		public IEnumerable<PCDDeviceFeature> RangeFeatures { 
			get {
				return Features.OfType<PCDDeviceFeatureRange>();
			}
		}
	}
}
