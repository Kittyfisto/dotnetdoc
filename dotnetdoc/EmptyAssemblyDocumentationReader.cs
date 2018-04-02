using System;

namespace dotnetdoc
{
	/// <summary>
	/// 
	/// </summary>
	internal sealed class EmptyAssemblyDocumentationReader
		: IAssemblyDocumentationReader
	{
		public TypeDocumentation GetDocumentationOf(Type type)
		{
			return null;
		}

		public TypeDocumentation GetDocumentationOf<T>()
		{
			return null;
		}
	}
}