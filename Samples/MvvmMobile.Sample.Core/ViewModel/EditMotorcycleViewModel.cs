using System;
using MvvmMobile.Core.Common;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.Sample.Core.Model;
using XLabs.Ioc;

namespace MvvmMobile.Sample.Core.ViewModel
{
    public class EditMotorcycleViewModel : BaseViewModel, IEditMotorcycleViewModel
    {
        // Constructors
        public EditMotorcycleViewModel()
        {
            CancelCommand = new RelayCommand(o => 
            {
                NavigateBack();
            });

            SaveMotorcycleCommand = new RelayCommand(o =>
            {
                var mcPayload = Resolver.Resolve<IMotorcyclePayload>();

                mcPayload.Motorcycle = new Motorcycle { Id = Id, Brand = Brand, Model = Model, Year = Year };

                NavigateBack(mcPayload);
            });
        }


        // -----------------------------------------------------------------------------

        // Properties
        public Guid Id { get; private set; }

        private string _brand;
        public string Brand
        {
            get { return _brand; }
            set
            {
                _brand = value;
                NotifyPropertyChanged(nameof(Brand));
            }
        }

        private string _model;
        public string Model
        {
            get { return _model; }
            set
            {
                _model = value;
                NotifyPropertyChanged(nameof(Model));
            }
        }

        private int _year;
        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                NotifyPropertyChanged(nameof(Year));
            }
        }


        // -----------------------------------------------------------------------------

        // Commands
        public RelayCommand CancelCommand { get; }
        public RelayCommand SaveMotorcycleCommand { get; }


        // -----------------------------------------------------------------------------

        // Public Methods
        public void Load(Guid payloadId)
        {
            var payload = LoadPayload<IMotorcyclePayload>(payloadId);
            if (payload == null)
            {
                Id = Guid.NewGuid();
                return;
            }

            Id = payload.Motorcycle.Id;
            Brand = payload.Motorcycle.Brand;
            Model = payload.Motorcycle.Model;
            Year = payload.Motorcycle.Year;
        }
    }
}