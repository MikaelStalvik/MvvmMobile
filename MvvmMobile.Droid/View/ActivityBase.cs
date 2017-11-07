﻿using System;
using System.ComponentModel;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmMobile.Core.Navigation;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.Droid.Model;
using MvvmMobile.Droid.Navigation;
using XLabs.Ioc;

namespace MvvmMobile.Droid.View
{
    public class ActivityBase<T> : Activity where T : class, IBaseViewModel
    {
        // Private Members
        private const string CallBackPayloadId = "MvvmMobileActivityBase-CallBackPayloadId";


        // -----------------------------------------------------------------------------

        // Properties
        private T _viewModel;
        protected T ViewModel
        {
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
                _viewModel.CallbackAction = HandleCallback;
            }
        }

        protected bool BackButtonEnabled { get; private set; }

        protected Guid PayloadId { get; private set; }
        protected Guid CallbackId { get; private set; }


        // -----------------------------------------------------------------------------

        // Lifecycle
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel = Resolver.Resolve<T>();

            ((AppNavigation)Resolver.Resolve<INavigation>()).Context = this;

            var extras = Intent.Extras;

            if (extras != null && string.IsNullOrWhiteSpace(extras.GetString(AppNavigation.PayloadAppParameter)) == false)
            {
                PayloadId = new Guid(extras.GetString(AppNavigation.PayloadAppParameter));
            }

            CallbackId = Guid.Empty;
            if (extras != null && string.IsNullOrWhiteSpace(extras.GetString(AppNavigation.CallbackAppParameter)) == false)
            {
                CallbackId = new Guid(extras.GetString(AppNavigation.CallbackAppParameter));
            }
        }

        protected override void OnStart()
        {
            base.OnStart();

            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        protected override void OnResume()
        {
            base.OnResume();

            ((AppNavigation)Resolver.Resolve<INavigation>()).Context = this;

            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            _viewModel?.OnActivated();

            if (_viewModel is IPayloadViewModel vm)
            {
                vm.Load(PayloadId);
            }
        }

        protected override void OnPause()
        {
            base.OnPause();

            _viewModel?.OnPaused();

            if (_viewModel != null)
            { 
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
        }

        public override void OnBackPressed()
        {
            if (BackButtonEnabled)
            {
                base.OnBackPressed();
            }
        }

        public override bool OnOptionsItemSelected(Android.Views.IMenuItem item)
        {
            OnBackPressed();

            return base.OnOptionsItemSelected(item);
        }

        protected virtual void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }


        // -----------------------------------------------------------------------------

        // Public Methods
        public void EnableBackButton(bool enable)
        {
            BackButtonEnabled = enable;

            ActionBar?.SetDisplayHomeAsUpEnabled(enable);
        }


        // -----------------------------------------------------------------------------

        // Payload and Callback Handling
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            ((AppNavigation)Resolver.Resolve<INavigation>()).Context = this;

            if (requestCode != AppNavigation.CallbackActivityRequestCode)
            {
                return;
            }

            if (resultCode != Result.Ok)
            {
                return;
            }

            var extras = data.Extras;
            if (extras == null || string.IsNullOrWhiteSpace(extras.GetString(AppNavigation.CallbackAppParameter)))
            {
                return;
            }

            // Get the callback id
            var callbackId = new Guid(extras.GetString(AppNavigation.CallbackAppParameter));

            // Get the callback payload
            var payloads = Resolver.Resolve<IPayloads>();
            var callbackPayload = payloads.GetAndRemove<ICallbackPayload>(callbackId);
            if (callbackPayload == null)
            {
                return;
            }

            // Get the payload
            var payloadId = new Guid(data.GetStringExtra(CallBackPayloadId));

            // Execute the callback
            callbackPayload.CallbackAction?.Invoke(payloadId);
        }

        protected void ClearPayload()
        {
            PayloadId = Guid.Empty;
        }

        private void HandleCallback(Guid payloadId)
        {
            // Create the callback intent
            var resultIntent = new Intent();

            resultIntent.PutExtra(AppNavigation.CallbackAppParameter, CallbackId.ToString());
            resultIntent.PutExtra(CallBackPayloadId, payloadId.ToString());

            SetResult(Result.Ok, resultIntent);

            Finish();
        }
    }
}