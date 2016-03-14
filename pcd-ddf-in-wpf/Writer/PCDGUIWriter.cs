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
using System.Xml.Linq;
using Koinzer.pcdddfinwpf.Model.GUI; 

namespace Koinzer.pcdddfinwpf.Writer
{
	/// <summary>
	/// Description of PCDGUIWriter.
	/// </summary>
	public class PCDGUIWriter
	{
		public PCDGUIWriter()
		{
		}
		
		public void Write(XElement formEl, Model.PCDDevice device, WriteResults results)
		{
			foreach (PCDDeviceElement element in device.GUIElements) {
				XElement elEl = new XElement(element.GetNodeName());
				elEl.Add(
					new XAttribute("top", element.Top),
					new XAttribute("left", element.Left));
				if (element.HasSize)
					elEl.Add(
						new XAttribute("width", element.Width),
						new XAttribute("height", element.Height));
				bool isColorBox = ((element is PCDDropdown) && 
				                   ((((PCDDropdown)element).AssociatedChannel.ChannelType == "color1") || 
				                    (((PCDDropdown)element).AssociatedChannel.ChannelType == "color2")));
				if (element is PCDDeviceControl) {
					PCDDeviceControl control = (PCDDeviceControl)element;
					elEl.Add(new XAttribute("name", control.Name));
					if (!isColorBox)
						elEl.Add(new XAttribute("action", control.Name + "_changed"));
				}
				if (element is PCDLabel) {
					elEl.Add(new XAttribute("caption", ((PCDLabel)element).Caption));
					elEl.Add(new XAttribute("name", ((PCDLabel)element).Name));
				}
				if (element is PCDSlider) {
					PCDSlider slider = (PCDSlider)element;
					int startValue = 0;
					int endValue = 255;
					int defaultValue = 0;
					if (slider.AssociatedChannel != null) {
						defaultValue = slider.AssociatedChannel.InitValue;
						foreach (Model.PCDChannelSubset subset in slider.AssociatedChannel.Subsets) {
							if (subset.InRange(defaultValue)) {
								startValue = subset.MinValue;
								endValue = subset.MaxValue;
							}
						}
					}
					elEl.Add(new XAttribute("startvalue", startValue), new XAttribute("endvalue", endValue), new XAttribute("default", defaultValue));
				}
				if ((element is PCDDropdown) && !isColorBox) {
					PCDDropdown dropdown = (PCDDropdown)element;
					if (dropdown.AssociatedChannel != null) {
						bool joinNext = false;
						foreach (Model.PCDChannelSubset subset in dropdown.AssociatedChannel.Subsets) {
							if (!joinNext) {
								String picture = "";
								if (subset is Model.PCDChannelItem)
									picture = ((Model.PCDChannelItem)subset).IconFileName;
								elEl.Add(new XElement("item",
								                      new XAttribute("caption", subset.Name),
								                      new XAttribute("value", 0),
								                      new XAttribute("picture", picture)));
							}
							joinNext = subset.JoinNext;
						}
					}
				}
				formEl.Add(elEl);
			}
		}
	}
}
