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
using System.Collections.Specialized;
using System.Collections.ObjectModel;
using System.Windows.Media;

namespace Koinzer.pcdddfinwpf.Model
{
	/// <summary>
	/// Description of PCDChannelSubset.
	/// </summary>
	public abstract class PCDChannelSubset: PCDModelObject
	{
		public PCDChannelSubset(PCDDeviceChannel parent): base()
		{
			Parent = parent;
			MinValue = 0;
			MaxValue = 255;
			JoinNext = false;
			Color = Colors.Transparent;
			Features = new ObservableCollection<PCDDeviceFeature>();
			Features.CollectionChanged += Features_CollectionChanged;
		}

		void Features_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add) {
				foreach (object o in e.NewItems) {
					PCDDeviceFeature feature = o as PCDDeviceFeature;
					if (feature == null) continue;
					if (!(feature is PCDDeviceFeatureRange) && (this is PCDChannelRange)) {
						throw new ArgumentException("This feature is not a range and can only be applied to items.");
					}
					if (Name == "<Not named>")
						Name = feature.FeatureItemName;
					}
			}
		}
		
		protected override System.Collections.IList GetContainingCollection()
		{
			if (Parent == null)
				return null;
			return Parent.Subsets;
		}
		
		public virtual void InitValueBounds()
		{
			if (Parent == null)
				return;
			int index = Parent.Subsets.IndexOf(this);
			if (index < 0)
				return;
			if (index > 0) {
				PCDChannelSubset previous = Parent.Subsets[index - 1];
				if (previous.MaxValue >= this.MinValue) {
					this.minValue = previous.MaxValue + 1;
				}
			}
			if (index < Parent.Subsets.Count - 1) {
				PCDChannelSubset next = Parent.Subsets[index + 1];
				if (next.MinValue <= this.MaxValue) {
					this.maxValue = next.MinValue - 1;
				}
			}
			CheckValueBounds();
		}
		
		public virtual void CheckValueBounds()
		{
			if ((this.MinValue < 0) || (this.MaxValue < 0)) {
				this.MinValue = 0;
				this.MaxValue = 0;
			}
			if ((this.MinValue > 255) || (this.MaxValue > 255)) {
				this.MinValue = 255;
				this.MaxValue = 255;
			}
			if (Parent == null)
				return;
			int index = Parent.Subsets.IndexOf(this);
			if (index < 0)
				return;
			if (index > 0) {
				PCDChannelSubset previous = Parent.Subsets[index - 1];
				if (previous.MaxValue >= this.MinValue) {
					if (this.MinValue - 1 < index - 1) {
						this.MinValue = previous.MinValue + 1;
					}
					previous.MaxValue = this.MinValue - 1;
				}
			}
			if (index < Parent.Subsets.Count - 1) {
				PCDChannelSubset next = Parent.Subsets[index + 1];
				if (next.MinValue <= this.MaxValue) {
					if (this.MaxValue + 1 > 255 - (Parent.Subsets.Count - index)) {
						this.MaxValue = next.MaxValue - 1;
					}
					next.MinValue = this.MaxValue + 1;
				}
			}
		}
		
		public bool InRange(int value)
		{
			return (minValue <= value) && (maxValue >= value);
		}
		
		int minValue;
		
		public int MinValue {
			get { return minValue; }
			set { 
				SetProperty(ref minValue, value);
				if (MinValue > MaxValue)
					MaxValue = MinValue;
				CheckValueBounds();
			}
		}
		
		int maxValue;
		
		public int MaxValue {
			get { return maxValue; }
			set { 
				SetProperty(ref maxValue, value);
				if (MinValue > MaxValue)
					MinValue = MaxValue;
				CheckValueBounds();
			}
		}
		
		String name;
		
		public String Name {
			get { return name; }
			set { SetProperty(ref name, value); }
		}
		
		Color color;
		
		public Color Color {
			get { return color; }
			set { SetProperty(ref color, value); }
		}
		
		bool joinNext;
		
		public bool JoinNext {
			get { return joinNext; }
			set { SetProperty(ref joinNext, value); }
		}
		
		ObservableCollection<PCDDeviceFeature> features;
		
		public ObservableCollection<PCDDeviceFeature> Features {
			get { return features; }
			set { SetProperty(ref features, value);	}
		}
		
		public PCDDeviceChannel Parent { get; private set; }
	}
}
