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
using System.Linq;
using System.Xml;
using Koinzer.pcdddfinwpf.Model;

namespace Koinzer.pcdddfinwpf.Parser
{
	/// <summary>
	/// Description of PCDFeatureParser.
	/// </summary>
	public class PCDFeatureParser: PCDParser
	{
		public PCDFeatureParser()
		{
		}
		
		public override void Parse(XmlDocument doc, Model.PCDDevice device, ParseResults results)
		{
			foreach (Model.PCDDeviceFeature feature in Model.PCDDeviceFeatures.Instance.Features) 
			{
				XmlNode node = GetNode(doc, feature.XmlNodeName);
				if (node == null)
					continue;
				if (node.Attributes[feature.XmlAttributeName] == null)
					continue;
				
				String channelName = feature.FeatureName;
				if (node.Attributes["ChannelName"] != null)
					channelName = node.Attributes["ChannelName"].Value;
				Model.PCDDeviceChannel channel = device.Channels.FirstOrDefault(ch => ch.ChannelType.ToLower() == channelName.ToLower());
				if (channel == null)
					continue;
				
				int featureValue = int.Parse(node.Attributes[feature.XmlAttributeName].Value);
				if (feature is Model.PCDDeviceFeatureRange) {
					Model.PCDDeviceFeatureRange rangeFeature = (Model.PCDDeviceFeatureRange)feature;
					Model.PCDChannelRange subset = new Model.PCDChannelRange(channel);
					subset.Name = feature.FeatureItemName;
					subset.Features.Add(feature);
					subset.MinValue = featureValue;
					subset.MaxValue = int.Parse(node.Attributes[rangeFeature.XmlAttributeMaxName].Value);
					if (node.Attributes[rangeFeature.XmlAttributeMaxName] == null)
						continue;
					
					// Bestehendes überschneidendes Subset suchen
					PCDChannelSubset collidingSubset;
					collidingSubset = channel.Subsets.FirstOrDefault(ch => ch.InRange(subset.MinValue) || ch.InRange(subset.MaxValue));
					bool doAdd = true;
					while (collidingSubset != null) {
						if (collidingSubset.MinValue == subset.MinValue && collidingSubset.MaxValue == subset.MaxValue) {
							collidingSubset.Features.Add(feature);
							doAdd = false;
							break;
						} else if (collidingSubset.MinValue == subset.MinValue && collidingSubset.MaxValue == subset.MinValue && subset.MaxValue > subset.MinValue) {
							subset.MinValue++;
							collidingSubset.JoinNext = true;
						} else if (collidingSubset.MinValue == subset.MaxValue && collidingSubset.MaxValue == subset.MaxValue && subset.MaxValue > subset.MinValue) {
							subset.MaxValue--;
							subset.JoinNext = true;
						} else {
							results.AddMessage("Featurerange of feature '{0}' collides with subset '{1}'.", feature.ToString(), collidingSubset.Name);
							doAdd = false;
							break;
						}
						collidingSubset = channel.Subsets.FirstOrDefault(ch => ch.InRange(subset.MinValue) || ch.InRange(subset.MaxValue));
					}
					if (doAdd) {
						channel.Subsets.Add(subset);
						channel.SortSubsets();
					}
				} else {
					Model.PCDChannelItem item = new Model.PCDChannelItem(channel);
					item.Features.Add(feature);
					item.MinValue = featureValue;
					item.MaxValue = featureValue;
					item.SetValue = featureValue;
					item.Name = feature.FeatureItemName;
					
					// Bestehendes überschneidendes Subset suchen
					PCDChannelSubset collidingSubset;
					collidingSubset = channel.Subsets.FirstOrDefault(ch => ch.InRange(item.MinValue) || ch.InRange(item.MaxValue));
					bool doAdd = true;
					while (collidingSubset != null) {
						if ((collidingSubset is Model.PCDChannelItem) && (((Model.PCDChannelItem)collidingSubset).SetValue == item.SetValue)) {
							collidingSubset.Features.Add(feature);
							doAdd = false;
							break;
						} else if (collidingSubset.MinValue == item.MinValue && collidingSubset.MaxValue == item.MinValue && item.MaxValue > item.MinValue) {
							item.MinValue++;
							collidingSubset.JoinNext = true;
						} else if (collidingSubset.MinValue == item.MaxValue && collidingSubset.MaxValue == item.MaxValue && item.MaxValue > item.MinValue) {
							item.MaxValue--;
							item.JoinNext = true;
						} else {
							results.AddMessage("Featurerange of feature '{0}' collides with subset '{1}'.", feature.ToString(), collidingSubset.Name);
							doAdd = false;
							break;
						}
						collidingSubset = channel.Subsets.FirstOrDefault(ch => ch.InRange(item.MinValue) || ch.InRange(item.MaxValue));
					} 
					if (doAdd) {
						channel.Subsets.Add(item);
						channel.SortSubsets();
					}
				}
			}
		}
	}
}
