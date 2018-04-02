using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;

namespace dotnetdoc
{
	/// <summary>
	///     The documentation for a particular property of a .NET <see cref="Type" />.
	/// </summary>
	public sealed class PropertyDocumentation
		: MemberDocumentation
	{
		/// <summary>
		/// </summary>
		public readonly Type DeclaringType;

		/// <summary>
		/// </summary>
		public readonly string FullTypeName;

		/// <summary>
		/// </summary>
		public readonly PropertyInfo Property;

		/// <summary>
		/// </summary>
		public readonly string PropertyName;

		/// <summary>
		///     Initializes this class.
		/// </summary>
		/// <param name="declaringType"></param>
		/// <param name="property"></param>
		/// <param name="propertyName"></param>
		/// <param name="fullTypeName"></param>
		/// <param name="summary"></param>
		/// <param name="remarks"></param>
		public PropertyDocumentation(Type declaringType,
		                             PropertyInfo property,
		                             string propertyName,
		                             string fullTypeName,
		                             string summary,
		                             IReadOnlyList<string> remarks)
			: base(summary, remarks)
		{
			DeclaringType = declaringType;
			Property = property;
			PropertyName = propertyName;
			FullTypeName = fullTypeName;
		}

		/// <summary>
		///     The property being documentated.
		/// </summary>
		public Type PropertyType => Property?.PropertyType;

		/// <summary>
		///     Creates a <see cref="PropertyDocumentation" /> from the given element.
		/// </summary>
		/// <param name="member"></param>
		/// <param name="assembly"></param>
		/// <returns></returns>
		public static PropertyDocumentation CreateFrom(XElement member, Assembly assembly)
		{
			var name = member.Attribute("name").Value;
			var index = name.LastIndexOf(".");
			var propertyName = name.Substring(index + 1);
			var fullTypeName = name.Substring(startIndex: 2, length: index - 2);
			var declaringType = assembly.GetType(fullTypeName);
			var property = declaringType?.GetProperty(propertyName);

			return new PropertyDocumentation(declaringType, property, propertyName, fullTypeName, GetSummary(member),
			                                 GetRemarks(member));
		}
	}
}