using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace dotnetdoc
{
	/// <summary>
	/// 
	/// </summary>
	public static class TextWriterExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="name"></param>
		public static void WriteHeader(this TextWriter writer, string name)
		{
			writer.WriteLine("# {0}  ", name);
			writer.WriteLine();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="documentation"></param>
		public static void WriteSummary(this TextWriter writer, MemberDocumentation documentation)
		{
			if (documentation != null)
			{
				var summary = documentation.Summary ?? string.Empty;
				writer.Write(Format(summary));
				writer.WriteLine();
				writer.WriteLine();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="writer"></param>
		/// <param name="documentation"></param>
		public static void WriteRemarks(this TextWriter writer, MemberDocumentation documentation)
		{
			if (documentation != null)
			{
				foreach (var remark in documentation.Remarks)
				{
					writer.Write(Format(remark));
					writer.WriteLine();
					writer.WriteLine();
				}
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="rawValue"></param>
		/// <returns></returns>
		[Pure]
		public static string Format(string rawValue)
		{
			// TODO: Another (probably better) solution would be to parse this using XDocument
			//       and then write a simple formatter to print the resulting tree

			// Ideally we would replace <see ... /> with a markup link, but that's not required just yet
			var regex = new Regex("<see cref=\"([^\"]*)\" />");
			var result = regex.Replace(rawValue, ReplaceLinks);
			return result;
		}

		private static string ReplaceLinks(Match match)
		{
			var value = match.Groups[1].Value;
			if (value.StartsWith("T:"))
				return value.Substring(2);

			if (value.StartsWith("P:") || value.StartsWith("F:"))
			{
				var idx = value.LastIndexOf(".", StringComparison.InvariantCulture);
				if (idx != -1)
					return value.Substring(idx + 1);

				return value.Substring(2);
			}

			return value;
		}
	}
}