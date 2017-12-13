using System;
using MvvmMobile.Core.Binding;
using MvvmMobile.iOS.Binding;
using MvvmMobile.iOS.Common;
using MvvmMobile.iOS.View;
using MvvmMobile.Sample.Core.ValueConverter;
using MvvmMobile.Sample.Core.ViewModel;
using UIKit;

namespace MvvmMobile.Sample.iOS.View
{
    [Storyboard(storyboardName:"Main", storyboardId:"EditMotorcycleViewController")]
    public partial class EditMotorcycleViewController : ViewControllerBase<IEditMotorcycleViewModel>
    {
        // Constructors
        public EditMotorcycleViewController(IntPtr handle) : base(handle)
        {
            AsModal = true;
        }


        // -----------------------------------------------------------------------------

        // Lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel.SetBinding(BrandTextField, "Brand", BindingMode.TwoWay);
            ViewModel.SetBinding(ModelTextField, "Model", BindingMode.TwoWay);
            ViewModel.SetBinding(YearTextField, "Year", BindingMode.TwoWay, new IntToStringValueConverter());
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            NavigationItem?.SetLeftBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Cancel, (sender, e) => ViewModel?.CancelCommand.Execute()), false);
            NavigationItem?.SetRightBarButtonItem(new UIBarButtonItem(UIBarButtonSystemItem.Done, (sender, e) => ViewModel?.SaveMotorcycleCommand.Execute()), false);
        }
    }
}