using MvvmMobile.Core.ViewModel;
using UIKit;

namespace MvvmMobile.iOS.Interface
{
    public interface IViewControllerFactory : IPayloadHandler
    {
        UIViewController CreateViewController();
    }
}