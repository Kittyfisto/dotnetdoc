namespace dotnetdoc.TestTypes
{
	/// <summary>
	///     I have a property <see cref="SomeProperty" />.
	/// </summary>
	public class TypeWithPropertyLinks
	{
		/// <summary>
		/// I'm related to <see cref="AnotherProperty"/>.
		/// </summary>
		public string SomeProperty { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public int AnotherProperty { get; set; }
	}
}