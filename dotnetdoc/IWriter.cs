using System.IO;

namespace dotnetdoc
{
	internal interface IWriter
	{
		void WriteTo(TextWriter textWriter);
	}
}