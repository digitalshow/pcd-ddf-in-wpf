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
			
			_features.Add(new PCDDeviceFeature("Shutter".Localize(), "Shutter.Closed".Localize(), "shutter", "CloseValue", 0));
			_features.Add(new PCDDeviceFeature("Shutter".Localize(), "Shutter.Open".Localize(), "shutter", "OpenValue", 255));
			
			_features.Add(new PCDDeviceFeature("Strobe".Localize(), "Off".Localize(), "strobe", "OffValue", 0));
			_features.Add(new PCDDeviceFeatureRange("Strobe".Localize(), "Range Min-Max".Localize(), "strobe", "MinValue", "MaxValue", 0, 255));
			
			_features.Add(new PCDDeviceFeatureRange("Dimmer".Localize(), "Range Off-Max".Localize(), "dimmer", "OffValue", "MaxValue", 0, 255));
			
			_features.Add(new PCDDeviceFeature("Gobo 1 Rotation".Localize(), "Off".Localize(), "gobo1rot", "OffValue", 128));
			_features.Add(new PCDDeviceFeatureRange("Gobo 1 Rotation".Localize(), "Range Left Min-Max".Localize(), "gobo1rot", "LeftMinValue", "LeftMaxValue", 0, 127));
			_features.Add(new PCDDeviceFeatureRange("Gobo 1 Rotation".Localize(), "Range Right Min-Max".Localize(), "gobo1rot", "RightMinValue", "RightMaxValue", 129, 255));
			
			_features.Add(new PCDDeviceFeature("Gobo 2 Rotation".Localize(), "Off".Localize(), "gobo2rot", "OffValue", 128));
			_features.Add(new PCDDeviceFeatureRange("Gobo 2 Rotation".Localize(), "Range Left Min-Max".Localize(), "gobo2rot", "LeftMinValue", "LeftMaxValue", 0, 127));
			_features.Add(new PCDDeviceFeatureRange("Gobo 2 Rotation".Localize(), "Range Right Min-Max".Localize(), "gobo2rot", "RightMinValue", "RightMaxValue", 129, 255));
			
			_features.Add(new PCDDeviceFeature("Prisma Rotation".Localize(), "Off".Localize(), "prismarot", "OffValue", 128));
			_features.Add(new PCDDeviceFeatureRange("Prisma Rotation".Localize(), "Range Left Min-Max".Localize(), "prismarot", "LeftMinValue", "LeftMaxValue", 0, 127));
			_features.Add(new PCDDeviceFeatureRange("Prisma Rotation".Localize(), "Range Right Min-Max".Localize(), "prismarot", "RightMinValue", "RightMaxValue", 129, 255));
			_features.Add(new PCDDeviceFeature("Prisma".Localize(), "Single".Localize(), "prisma", "SingleValue", 0));
			_features.Add(new PCDDeviceFeature("Prisma".Localize(), "Triple".Localize(), "prisma", "TripleValue", 100));
			
			_features.Add(new PCDDeviceFeature("Iris".Localize(), "Iris.Closed".Localize(), "iris", "CloseValue", 0));
			_features.Add(new PCDDeviceFeature("Iris".Localize(), "Iris.Open".Localize(), "iris", "OpenValue", 255));
			_features.Add(new PCDDeviceFeatureRange("Iris".Localize(), "Range Min-Max".Localize(), "iris", "MinValue", "MaxValue", 1, 254));
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
