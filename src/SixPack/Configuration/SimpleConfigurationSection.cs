// SimpleConfigurationSection.cs 
//
//  Copyright (C) 2011 Antoine Aubry
//  Author: Antoine Aubry
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//

using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SixPack.Configuration
{
	/// <summary>
	/// Implements a configuration section handler that uses XML serialization to read the content.
	/// </summary>
	/// <example>
	///		&lt;section name="test" type="SixPack.Configuration.SimpleConfigurationSection`1[[MyProject.MySection, MyProject]], SixPack" /&gt;
	/// </example>
	/// <typeparam name="TConfiguration">The type of the configuration.</typeparam>
	public class SimpleConfigurationSection<TConfiguration> : IConfigurationSectionHandler
	{
		#region IConfigurationSectionHandler Members

		/// <summary>
		/// Creates a configuration section handler.
		/// </summary>
		/// <param name="parent">Parent object.</param>
		/// <param name="configContext">Configuration context object.</param>
		/// <param name="section">Section XML node.</param>
		/// <returns>The created section handler object.</returns>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			var serializer = new XmlSerializer(typeof(TConfiguration), new XmlRootAttribute(section.LocalName));
			return serializer.Deserialize(new StringReader(section.OuterXml));
		}

		#endregion
	}
}
