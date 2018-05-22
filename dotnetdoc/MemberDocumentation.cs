using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace dotnetdoc
{
	/// <summary>
	///     Provides access to the documentation for a particular member (be it type, field, property, method, etc...),
	///     typically
	///     extracted from the assembly's XML documentation file.
	/// </summary>
	public class MemberDocumentation
	{
		/// <summary>
		///     The list of remarks to this member.
		/// </summary>
		/// <remarks>
		///     May be empty.
		/// </remarks>
		public readonly IReadOnlyList<string> Remarks;

		/// <summary>
		///     The summary
		/// </summary>
		public readonly string Summary;

		/// <summary>
		///     Initizes this class.
		/// </summary>
		/// <param name="summary"></param>
		/// <param name="remarks"></param>
		public MemberDocumentation(string summary, IReadOnlyList<string> remarks)
		{
			Summary = summary;
			Remarks = remarks;
		}

		/// <summary>
		///     Retrieves the summary of the given element.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		protected static string GetSummary(XElement member)
		{
			var summary = member.Descendants("summary").FirstOrDefault();
			return GetRawValue(summary);
		}

		/// <summary>
		///     Retrieves the list of remarks from the given element.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		[Pure]
		protected static IReadOnlyList<string> GetRemarks(XElement member)
		{
			var remarks = member.Descendants("remarks");
			var allRemarks = new List<string>();
			foreach (var remark in remarks) allRemarks.Add(GetRawValue(remark));
			return allRemarks;
		}

		[Pure]
		private static string GetRawValue(XElement element)
		{
			if (element == null)
				return null;

			var rawText = string.Concat(element.Nodes());
			var cleaned = rawText.Trim();
			return cleaned;
		}
	}
}