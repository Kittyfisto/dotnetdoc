using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;

namespace dotnetdoc
{
	/// <summary>
	///     Provides access to the documentation for a particular member (be it type, field, property, method, etc...), typically
	///     extracted from the assembly's XML documentation file.
	/// </summary>
	public class MemberDocumentation
	{
		public readonly string Summary;
		public readonly IReadOnlyList<string> Remarks;

		public MemberDocumentation(string summary, IReadOnlyList<string> remarks)
		{
			Summary = summary;
			Remarks = remarks;
		}

		protected static string GetSummary(XElement member)
		{
			var summary = member.Descendants("summary").FirstOrDefault();
			return summary?.Value.Trim();
		}

		[Pure]
		protected static IReadOnlyList<string> GetRemarks(XElement member)
		{
			var remarks = member.Descendants("remarks");
			var allRemarks = new List<string>();
			foreach (var remark in remarks)
			{
				allRemarks.Add(remark.Value.Trim());
			}
			return allRemarks;
		}
	}
}