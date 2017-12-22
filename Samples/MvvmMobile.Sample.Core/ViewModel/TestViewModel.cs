using System;
using MvvmMobile.Core;
using MvvmMobile.Core.Common;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.Sample.Core.Model;

namespace MvvmMobile.Sample.Core.ViewModel
{
    public class TestViewModel : BaseViewModel, ITestViewModel
    {
        // Constructors
        public TestViewModel()
        {
            CancelCommand = new RelayCommand(() => 
            {
                NavigateBack();
            });

            SaveMotorcycleCommand = new RelayCommand(() =>
            {
                var mcPayload = Mvvm.Api.Resolver.Resolve<IMotorcyclePayload>();

                mcPayload.Motorcycle = new Motorcycle { Id = Guid.NewGuid(), Brand = "A", Model = "B", Year = 1000};

                NavigateBack(mcPayload);
            });
        }


        // -----------------------------------------------------------------------------

        // Properties
        private IMotorcycle _motorcycle;
        public IMotorcycle Motorcycle
        {
            get { return _motorcycle; }
            set
            {
                _motorcycle = value;
                NotifyPropertyChanged(nameof(Motorcycle));
            }
        }

        // -----------------------------------------------------------------------------

        // Commands
        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveMotorcycleCommand { get; }

        // -----------------------------------------------------------------------------

        // Public Methods
        public override void InitWithPayload(Guid payloadId)
        {
            base.InitWithPayload(payloadId);

            var payload = LoadPayload<IMotorcyclePayload>(payloadId);
            if (payload == null)
            {
                Motorcycle = new Motorcycle { Id = Guid.NewGuid() };
                return;
            }

            System.Diagnostics.Debug.WriteLine($"TestViewModel.InitWithPayload: {payload.Motorcycle.Brand}");

            Motorcycle = payload.Motorcycle;
        }
    }
}