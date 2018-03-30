using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace dotnetdoc
{
	/// <summary>
	///     Provides access to the documentation of a particular .NET type, typically
	///     extracted from the assembly's XML documentation file.
	/// </summary>
	public sealed class TypeDocumentation
		: MemberDocumentation
	{
		private readonly List<PropertyDocumentation> _properties;
		public readonly string FullTypeName;
		public readonly IReadOnlyList<PropertyDocumentation> Properties;
		public readonly Type Type;

		public TypeDocumentation(Type type, string fullTypeName, string summary, IReadOnlyList<string> remarks) :
			base(summary, remarks)
		{
			Type = type;
			FullTypeName = fullTypeName;
			Properties = _properties = new List<PropertyDocumentation>();
		}

		public static TypeDocumentation CreateFrom(XElement member, Assembly assembly)
		{
			var fullTypeName = member.Attribute("name").Value.Substring(startIndex: 2);
			var type = assembly.GetType(fullTypeName);
			return new TypeDocumentation(type, fullTypeName, GetSummary(member), GetRemarks(member));
		}

		public void Add(PropertyDocumentation property)
		{
			_properties.Add(property);
		}
	}
}