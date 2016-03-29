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
using System.Linq;
using System.Xml.Linq;

namespace Koinzer.pcdddfinwpf.Writer
{
	/// <summary>
	/// Description of PCDFeatureWriter.
	/// </summary>
	public class PCDFeatureWriter
	{
		public PCDFeatureWriter()
		{
		}
		
		public void Write(XElement devEl, Model.PCDDevice device, WriteResults results)
		{
			List<Model.PCDDeviceFeature> unusedFeatures = new List<Koinzer.pcdddfinwpf.Model.PCDDeviceFeature>();
			foreach (Model.PCDDeviceFeature feature in Model.PCDDeviceFeatures.Instance.Features) {
				if (String.IsNullOrEmpty(feature.XmlNodeName)) // "None"
					continue;
				XElement featEl = devEl.Element(feature.XmlNodeName);
				if (featEl == null)
					devEl.Add(featEl = new XElement(feature.XmlNodeName));
				int value = feature.InitValue;
				int maxValue = (feature is Model.PCDDeviceFeatureRange) ? ((Model.PCDDeviceFeatureRange)feature).InitMaxValue : 0;
				IEnumerable<Model.PCDChannelSubset> subsets = device.Channels.SelectMany(ch => ch.Subsets.Where(subset => subset.Features.Contains(feature)));
				if (subsets.Count() == 0) {
					unusedFeatures.Add(feature);
					continue;
				} else {
					subsets = subsets.OrderBy(subset => (subset is Model.PCDChannelRange));
					if (subsets.Count() > 1)
						results.AddMessage("PCDFeatureWriter.FeatureAppliedMoreThanOnce".Localize(), feature.ToString());
					if (subsets.First() is Model.PCDChannelItem && !(feature is Model.PCDDeviceFeatureRange)) {
						value = ((Model.PCDChannelItem)subsets.First()).SetValue;
					} else
						value = subsets.First().MinValue;
					if (feature is Model.PCDDeviceFeatureRange)
						maxValue = subsets.First().MaxValue;
					if (featEl.Attribute("ChannelName") == null) {
						featEl.Add(new XAttribute("ChannelName", subsets.First().Parent.ChannelType));
					} else if (featEl.Attribute("ChannelName").Value != subsets.First().Parent.ChannelType) {
						results.AddMessage("PCDFeatureWriter.DifferentChannels".Localize(), feature.ToString());
						value = feature.InitValue;
						maxValue = (feature is Model.PCDDeviceFeatureRange) ? ((Model.PCDDeviceFeatureRange)feature).InitMaxValue : 0;
					}
				}
				featEl.Add(new XAttribute(feature.XmlAttributeName, value));
				if (feature is Model.PCDDeviceFeatureRange)
					featEl.Add(new XAttribute(((Model.PCDDeviceFeatureRange)feature).XmlAttributeMaxName, maxValue));
			}
			foreach (Model.PCDDeviceFeature feature in unusedFeatures) {
				XElement featEl = devEl.Element(feature.XmlNodeName);
				if (featEl == null)
					devEl.Add(featEl = new XElement(feature.XmlNodeName));
				if (featEl.Attribute(feature.XmlAttributeName) != null)
					continue;
				featEl.Add(new XAttribute(feature.XmlAttributeName, feature.InitValue));
				if (feature is Model.PCDDeviceFeatureRange)
					featEl.Add(new XAttribute(
						((Model.PCDDeviceFeatureRange)feature).XmlAttributeMaxName, 
						((Model.PCDDeviceFeatureRange)feature).InitMaxValue));
			}
		}
	}
}
