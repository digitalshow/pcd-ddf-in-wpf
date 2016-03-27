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
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;

namespace Koinzer.pcdddfinwpf
{
	/// <summary>
	/// Tracks changes of objects and referenced objects.
	/// Keeps track of changes with INotifyPropertyChanged
	/// and INotifyCollectionChanged interfaces. Keeps track
	/// of references. It might not always be accurate keeping
	/// track of referencing because of the events the 
	/// interfaces provide.
	/// </summary>
	public class ChangesTracker: Prism.Mvvm.BindableBase
	{
		public ChangesTracker()
		{
			Changed = false;
			References = new Dictionary<object, int>();
			PropertyValues = new Dictionary<Tuple<object, string>, object>();
			Parents = new Dictionary<object, object[]>();
		}
		
		public bool Attach(Object child)
		{
			return Attach(child, new Object[0]);
		}
		
		public bool Attach(Object child, Object[] parents)
		{
			if ((child == null) || (child is String) || (child is int))
				return false;
			if ((parents.Length > 0) && (child == _observed))
				return false;
			if (References.ContainsKey(child)) {
				References[child]++;
				return true;
			}
			
			Object[] newParents = parents.Concat(new Object[] {child}).ToArray();
			
			bool isAttached = false;
			if (child is INotifyPropertyChanged) {
				AttachPropertyChanged((INotifyPropertyChanged)child);
				isAttached = true;
			}
			if (child is INotifyCollectionChanged) {
				AttachCollectionChanged((INotifyCollectionChanged)child);
				isAttached = true;
			}
			if (child is IEnumerable) {
				foreach (Object item in (IEnumerable)child)
					Attach(item, newParents);
			}
			foreach (PropertyInfo pi in child.GetType().GetProperties()) {
				if (pi.GetIndexParameters().Any())
					continue;
				Object value = pi.GetValue(child);
				if (value == null) continue;
				if (newParents.Contains(value)) continue;
				if (Attach(value, newParents)) {
					isAttached = true;
					PropertyValues[Tuple.Create(child, pi.Name)] = value;
				}
			}
			
			if (isAttached) {
				References[child] = 1;
				Parents[child] = newParents;
			}
			return isAttached;
		}
		
		public void AttachPropertyChanged(INotifyPropertyChanged child)
		{
			child.PropertyChanged += Observed_PropertyChanged;
		}
		
		public void AttachCollectionChanged(INotifyCollectionChanged child)
		{
			child.CollectionChanged += Observed_CollectionChanged;
		}

		void Observed_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			Changed = true;
			var key = Tuple.Create(sender, e.PropertyName);
			if (PropertyValues.ContainsKey(key)) {
				Object oldValue = PropertyValues[key];
				Detach(oldValue, Parents[sender]);
			}
			PropertyInfo pi = sender.GetType().GetProperty(e.PropertyName);
			if ((pi != null) && (pi.GetIndexParameters().Length == 0))
				Attach(pi.GetValue(sender), Parents[sender]);
		}

		void Observed_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			Changed = true;
			switch (e.Action) {
				case NotifyCollectionChangedAction.Add:
					foreach (Object item in e.NewItems)
						Attach(item, Parents[sender]);
					break;
				case NotifyCollectionChangedAction.Replace:
					foreach (Object item in e.NewItems)
						Attach(item, Parents[sender]);
					foreach (Object item in e.OldItems)
						Detach(item, Parents[sender]);
					break;
				case NotifyCollectionChangedAction.Remove:
					foreach (Object item in e.OldItems)
						Detach(item, Parents[sender]);
					break;
			}
		}
		
		public void Detach(Object child)
		{
			Detach(child, new Object[0]);
		}
		
		public void Detach(Object child, Object[] parents)
		{
			if (child == null)
				return;
			if (!References.ContainsKey(child))
				return;
			References[child]--;
			if (References[child] < 0)
				throw new ApplicationException("Reference counter for object " + child.ToString() + " dropped below 0!");
			if (References[child] > 0)
				return;
			References.Remove(child);
			Parents.Remove(child);
			
			Object[] newParents = parents.Concat(new Object[] {child}).ToArray();
			
			if (child is INotifyPropertyChanged)
				DetachPropertyChanged((INotifyPropertyChanged)child);
			if (child is INotifyCollectionChanged)
				DetachCollectionChanged((INotifyCollectionChanged)child);
			if (child is IEnumerable) {
				foreach (Object item in (IEnumerable)child)
					Detach(item, newParents);
			}
			foreach (PropertyInfo pi in child.GetType().GetProperties()) {
				if (pi.GetIndexParameters().Any())
					continue;
				Object value = pi.GetValue(child);
				if (PropertyValues.ContainsKey(Tuple.Create(child, pi.Name)))
					PropertyValues.Remove(Tuple.Create(child, pi.Name));
				if (value == null) continue;
				if (newParents.Contains(value)) continue;
				Detach(value, newParents);
			}
		}
		
		public void DetachPropertyChanged(INotifyPropertyChanged child)
		{
			child.PropertyChanged -= Observed_PropertyChanged;
		}
		
		public void DetachCollectionChanged(INotifyCollectionChanged child)
		{
			child.CollectionChanged -= Observed_CollectionChanged;
		}
		
		public void Unchanged()
		{
			Changed = false;
		}
		
		private void Reset()
		{
			while (References.Count > 0) {
				Object key = References.First().Key;
				References[key] = 1;
				Detach(key);
			}
			Unchanged();
		}
		
		private Dictionary<Object, int> References { get; set; }
		
		private Dictionary<Tuple<Object, String>, Object> PropertyValues { get; set; }
		
		private Dictionary<Object, Object[]> Parents { get; set; }
		
		Object _observed;
		
		public Object Observed { 
			get { return _observed; }
			set {
				Detach(_observed);
				System.Diagnostics.Debug.WriteLine("ChangesTracker: {0} hanging references.", References.Count);
				System.Diagnostics.Debug.WriteLine("ChangesTracker: {0} hanging property values.", PropertyValues.Count);
				Reset(); 
				_observed = value;
				Attach(_observed);
			}
		}
		
		bool changed;
		
		public bool Changed {
			get { return changed; }
			private set { SetProperty(ref changed, value); }
		}
	}
}
