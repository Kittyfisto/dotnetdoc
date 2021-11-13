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

		/// <summary>
		///     The full type name of the type being documented.
		/// </summary>
		public readonly string FullTypeName;

		/// <summary>
		///     The properties of the type.
		/// </summary>
		public readonly IReadOnlyList<PropertyDocumentation> Properties;

		/// <summary>
		///     The type being documented.
		/// </summary>
		public readonly Type Type;

		/// <summary>
		///     Initializes this class.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="fullTypeName"></param>
		/// <param name="summary"></param>
		/// <param name="remarks"></param>
		public TypeDocumentation(Type type, string fullTypeName, string summary, IReadOnlyList<string> remarks) :
			base(summary, remarks)
		{
			Type = type;
			FullTypeName = fullTypeName;
			Properties = _properties = new List<PropertyDocumentation>();
		}

		/// <summary>
		///     Creates a new <see cref="TypeDocumentation" /> from the given element.
		/// </summary>
		/// <param name="member"></param>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static TypeDocumentation CreateFrom(XElement member, Assembly assembly)
		{
			var fullTypeName = member.Attribute("name").Value.Substring(startIndex: 2);
			var type = assembly.GetType(fullTypeName);
			return new TypeDocumentation(type, fullTypeName, GetSummary(member), GetRemarks(member));
		}

		/// <summary>
		///     Adds the given property to this type's documentation.
		/// </summary>
		/// <param name="property"></param>
		public void Add(PropertyDocumentation property)
		{
			_properties.Add(property);
		}
	}
}