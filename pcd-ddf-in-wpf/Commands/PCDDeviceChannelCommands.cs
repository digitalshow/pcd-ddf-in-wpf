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
using System.Windows.Input;
using Prism.Commands;

namespace Koinzer.pcdddfinwpf.Commands
{
	/// <summary>
	/// Description of PCDDeviceChannelCommands.
	/// </summary>
	public class PCDDeviceChannelCommands
	{
		public PCDDeviceChannelCommands()
		{
			AddItem = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDDeviceChannel>(DoAddItem, c => c != null));
			AddRange = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDDeviceChannel>(DoAddRange, c => c != null));
			InsertItem = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDModelObject>(DoInsertItem, s => s != null && s is Model.PCDChannelSubset));
			InsertRange = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDModelObject>(DoInsertRange, s => s != null && s is Model.PCDChannelSubset));
			Remove = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDModelObject>(DoRemove, s => s != null && s is Model.PCDChannelSubset));
		}
		
		public ICommand AddItem { get; private set; }
		
		public void DoAddItem(Model.PCDDeviceChannel channel)
		{
			channel.Subsets.Add(new Model.PCDChannelItem(channel));
			channel.Subsets[channel.Subsets.Count-1].InitValueBounds();
		}
		
		public ICommand AddRange { get; private set; }
		
		public void DoAddRange(Model.PCDDeviceChannel channel)
		{
			channel.Subsets.Add(new Model.PCDChannelRange(channel));
			channel.Subsets[channel.Subsets.Count-1].InitValueBounds();
		}
		
		public ICommand InsertItem { get; private set; }
		
		public void DoInsertItem(Model.PCDModelObject current)
		{
			Model.PCDChannelSubset subset = current as Model.PCDChannelSubset;
			if (subset == null || subset.Parent == null)
				return;
			if (subset.MinValue == subset.MaxValue)
				throw new ArgumentException("The element selected needs to have space for the new element.");
			Model.PCDChannelItem item = new Koinzer.pcdddfinwpf.Model.PCDChannelItem(subset.Parent);
			item.MinValue = subset.MinValue;
			item.MaxValue = subset.MinValue;
			item.SetValue = subset.MinValue;
			subset.MinValue++;
			if ((subset is Model.PCDChannelItem) && (((Model.PCDChannelItem)subset).SetValue < subset.MinValue))
				((Model.PCDChannelItem)subset).SetValue = subset.MinValue;
			subset.Parent.Subsets.Insert(subset.Parent.Subsets.IndexOf(subset), item);
		}
		
		public ICommand InsertRange { get; private set; }
		
		public void DoInsertRange(Model.PCDModelObject current)
		{
			Model.PCDChannelSubset subset = current as Model.PCDChannelSubset;
			if (subset == null || subset.Parent == null)
				return;
			if (subset.MinValue == subset.MaxValue)
				throw new ArgumentException("The element selected needs to have space for the new element.");
			Model.PCDChannelRange range = new Koinzer.pcdddfinwpf.Model.PCDChannelRange(subset.Parent);
			range.MinValue = subset.MinValue;
			range.MaxValue = subset.MinValue;
			subset.MinValue++;
			if ((subset is Model.PCDChannelItem) && (((Model.PCDChannelItem)subset).SetValue < subset.MinValue))
				((Model.PCDChannelItem)subset).SetValue = subset.MinValue;
			subset.Parent.Subsets.Insert(subset.Parent.Subsets.IndexOf(subset), range);
		}
		
		public ICommand Remove { get; private set; }
		
		public void DoRemove(Model.PCDModelObject item)
		{
			Model.PCDChannelSubset subset = item as Model.PCDChannelSubset;
			if (subset == null || subset.Parent == null)
				return;
			subset.Parent.Subsets.Remove(subset);
		}
	}
}
