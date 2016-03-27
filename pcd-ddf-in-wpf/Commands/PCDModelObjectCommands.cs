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
using System.Collections.ObjectModel;
using System.Reflection;
using System.Windows.Input;
using Prism.Commands;

namespace Koinzer.pcdddfinwpf.Commands
{
	/// <summary>
	/// Description of PCDModelObjectCommands.
	/// </summary>
	public class PCDModelObjectCommands
	{
		public PCDModelObjectCommands()
		{
			Up = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDModelObject>(DoUp, c => c != null && c.ContainingCollection != null));
			Down = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDModelObject>(DoDown, c => c != null && c.ContainingCollection != null));
			Remove = new AutoCanExecuteCommandWrapper(new DelegateCommand<Model.PCDModelObject>(DoRemove, c => c != null && c.ContainingCollection != null));
		}
		
		public ICommand Up { get; private set; }
		
		public void DoUp(Model.PCDModelObject obj)
		{
			if (obj != null && obj.ContainingCollection != null) {
				int index = obj.ContainingCollection.IndexOf(obj);
				if (index <= 0)
					return;
				MethodInfo method = obj.ContainingCollection.GetType().GetMethod("Move");
				if (method != null) {
					method.Invoke(obj.ContainingCollection, new object[] {index, index - 1});
				}
			}
		}
		
		public ICommand Down { get; private set; }
		
		public void DoDown(Model.PCDModelObject obj)
		{
			if (obj != null && obj.ContainingCollection != null) {
				int index = obj.ContainingCollection.IndexOf(obj);
				if (index >= obj.ContainingCollection.Count - 1)
					return;
				MethodInfo method = obj.ContainingCollection.GetType().GetMethod("Move");
				if (method != null) {
					method.Invoke(obj.ContainingCollection, new object[] {index, index + 1});
				}
			}
		}
		
		public ICommand Remove { get; private set; }
		
		public void DoRemove(Model.PCDModelObject obj)
		{
			if (obj != null && obj.ContainingCollection != null)
				obj.ContainingCollection.Remove(obj);
		}
	}
}
