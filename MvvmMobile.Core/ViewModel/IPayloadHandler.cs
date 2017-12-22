using System;

namespace MvvmMobile.Core.ViewModel
{
    public interface IPayloadHandler
    {
        void SetPayload(IPayload payload);
        void SetCallback(Action<Guid> callbackAction);
    }
}