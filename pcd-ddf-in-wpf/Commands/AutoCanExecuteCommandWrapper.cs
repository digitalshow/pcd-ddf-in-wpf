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

namespace Koinzer.pcdddfinwpf.Commands
{
	// http://stackoverflow.com/questions/7350845/canexecute-logic-for-delegatecommand
	public class AutoCanExecuteCommandWrapper : ICommand
	{
	    public ICommand WrappedCommand { get; private set; }
	
	    public AutoCanExecuteCommandWrapper(ICommand wrappedCommand)
	    {
	        if (wrappedCommand == null) 
	        {
	            throw new ArgumentNullException("wrappedCommand");
	        }
	
	        WrappedCommand = wrappedCommand;
	    }
	
	    public void Execute(object parameter)
	    {
	        WrappedCommand.Execute(parameter);
	    }
	
	    public bool CanExecute(object parameter)
	    {
	        return WrappedCommand.CanExecute(parameter);
	    }
	
	    public event EventHandler CanExecuteChanged
	    {
	        add { CommandManager.RequerySuggested += value; }
	        remove { CommandManager.RequerySuggested -= value; }
	    }
	}
}
