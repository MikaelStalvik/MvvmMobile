using System;
using MvvmMobile.Core.Common;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.Sample.Core.Model;

namespace MvvmMobile.Sample.Core.ViewModel
{
    public interface IEditMotorcycleViewModel : IPayloadViewModel
    {
        Guid Id { get; }
        string Brand { get; set; }
        string Model { get; set; }
        int Year { get; set; }

        RelayCommand CancelCommand { get; }
        RelayCommand SaveMotorcycleCommand { get; }
    }
}