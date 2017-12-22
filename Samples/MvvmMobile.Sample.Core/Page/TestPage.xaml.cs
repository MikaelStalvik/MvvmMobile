using System;
using System.ComponentModel;
using MvvmMobile.Core.Navigation;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.Sample.Core.ViewModel;
using Xamarin.Forms;

namespace MvvmMobile.Sample.Core.Page
{
    public partial class TestPage : PageBase<ITestViewModel>
    {
        public TestPage()
        {
            InitializeComponent();
            AsModal = true;
        }

        protected override void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.ViewModel_PropertyChanged(sender, e);

            System.Diagnostics.Debug.WriteLine($"{e.PropertyName}, {ViewModel.Motorcycle}");
        }
    }

    public class PageBase<T> : ContentPage, IPayloadHandler, IModalAware where T : class, IBaseViewModel
    {
        // Lifecycle
        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel = MvvmMobile.Core.Mvvm.Api.Resolver.Resolve<T>();

            BindingContext = ViewModel;

            _viewModel?.InitWithPayload(PayloadId);

            //TODO: Somehow set the current NavigationController on iOS...

            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            _viewModel?.OnActivated();
        }

        protected override void OnDisappearing()
        {
            _viewModel?.OnPaused();

            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }

            base.OnDisappearing();
        }


        // -----------------------------------------------------------------------------

        // Properties
        protected Guid PayloadId { get; set; }
        protected Action<Guid> CallbackAction { get; set; }
        public bool AsModal { get; protected set; }

        private T _viewModel;
        protected T ViewModel
        {
            // ReSharper disable once UnusedMember.Global
            get { return _viewModel; }
            set
            {
                _viewModel = value;

                if (_viewModel == null)
                {
                    return;
                }

                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;

                _viewModel.OnLoaded();
                _viewModel.CallbackAction = CallbackAction;
            }
        }


        // -----------------------------------------------------------------------------

        // Virtual Methods
        protected virtual void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e) { }
        protected virtual void ViewFramesReady() { }


        // -----------------------------------------------------------------------------

        // Payload and Callback Handling
        public void SetPayload(IPayload payload)
        {
            if (payload == null)
            {
                return;
            }

            // Set payload id
            PayloadId = Guid.NewGuid();

            // Add payload
            var payloads =  MvvmMobile.Core.Mvvm.Api.Resolver.Resolve<IPayloads>();
            payloads.Add(PayloadId, payload);
        }

        public void SetCallback(Action<Guid> callbackAction)
        {
            if (callbackAction == null)
            {
                return;
            }

            CallbackAction = callbackAction;
        }
    }
}