using MvvmMobile.Core.Navigation;
using MvvmMobile.iOS.Navigation;
using MvvmMobile.Sample.Core.Page;
using MvvmMobile.Sample.iOS.ViewController;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(PagePase), typeof(FormsPageRenderer))]
namespace MvvmMobile.Sample.iOS.ViewController
{
    public class FormsPageRenderer : PageRenderer, IModalAware
    {
        public bool AsModal { get; protected set; }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController != null)
            {
                ((AppNavigation)MvvmMobile.Core.Mvvm.Api.Resolver.Resolve<MvvmMobile.Core.Navigation.INavigation>()).NavigationController = NavigationController;
            }

            //if (NavigationItem != null)
            //{
            //    NavigationItem.Title = Title;
            //}
        }
    }
}