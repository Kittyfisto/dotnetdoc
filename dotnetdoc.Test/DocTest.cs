using NUnit.Framework;

namespace dotnetdoc.Test
{
	[TestFixture]
	public sealed class DocTest
	{
		[Test]
		public void Test()
		{
			using (var doc = new Doc(typeof(Doc).Assembly))
			{
				
			}
		}
	}
}
