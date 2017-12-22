using MvvmMobile.iOS.Interface;
using UIKit;
using Xamarin.Forms;

namespace MvvmMobile.Sample.iOS.ViewController
{
    public class FormsViewController<T> : IViewControllerFactory where T : Page, new()
    {
        public UIViewController CreateViewController()
        {
            return new T().CreateViewController();
        }
    }
}