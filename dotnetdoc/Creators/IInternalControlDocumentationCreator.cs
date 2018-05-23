using System.Windows;
using System.Windows.Media.Imaging;

namespace dotnetdoc.Creators
{
	/// <summary>
	/// 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IInternalControlDocumentationCreator<T>
		: IControlDocumentationCreator<T>
		where T : FrameworkElement
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="screenshot"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		string AddImage(BitmapSource screenshot, string name);
	}
}