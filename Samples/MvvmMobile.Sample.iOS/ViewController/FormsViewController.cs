using System;
using MvvmMobile.Core.Navigation;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.iOS.Interface;
using UIKit;
using Xamarin.Forms;

namespace MvvmMobile.Sample.iOS.ViewController
{
    public class FormsViewController<T> : IViewControllerFactory, IModalAware where T : Page, new()
    {
        private Page _page;

        public bool AsModal 
        {
            get
            {
                if (_page == null)
                {
                    return false;
                }

                var modalAwarePage = _page as IModalAware;
                if (modalAwarePage == null)
                {
                    return false;
                }

                return modalAwarePage.AsModal;
            }
        }

        public UIViewController CreateViewController()
        {
            _page = new T();
            return _page.CreateViewController();
        }

        public void SetCallback(Action<Guid> callbackAction)
        {
            if (_page is IPayloadHandler payloadHandler)
            {
                payloadHandler.SetCallback(callbackAction);
            }
        }

        public void SetPayload(IPayload payload)
        {
            if (_page is IPayloadHandler payloadHandler)
            {
                payloadHandler.SetPayload(payload);
            }
        }
    }
}