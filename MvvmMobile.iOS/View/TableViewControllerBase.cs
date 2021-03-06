﻿using System;
using System.ComponentModel;
using MvvmMobile.Core.Navigation;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.iOS.Navigation;
using UIKit;

namespace MvvmMobile.iOS.View
{
    public class TableViewControllerBase<T> : UITableViewController, IViewControllerBase where T : class, IBaseViewModel
    {
        // Private Members
        private bool _isFramesReady;


        // -----------------------------------------------------------------------------

        // Constructors
        public TableViewControllerBase()
        {
        }

        public TableViewControllerBase (IntPtr handle) : base (handle)
        {
        }


        // -----------------------------------------------------------------------------

        // Lifecycle
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ViewModel = Core.Mvvm.Api.Resolver.Resolve<T>();

            _viewModel?.InitWithPayload(PayloadId);
        }

        public override void ViewWillLayoutSubviews()
        {
            if (_isFramesReady == false)
            {
                ViewFramesReady();
            }

            _isFramesReady = true;

            base.ViewWillLayoutSubviews();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (NavigationController != null)
            {
                ((AppNavigation)Core.Mvvm.Api.Resolver.Resolve<INavigation>()).NavigationController = NavigationController;
            }

            if (NavigationItem != null)
            {
                NavigationItem.Title = Title;
            }

            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                _viewModel.PropertyChanged += ViewModel_PropertyChanged;
            }

            _viewModel?.OnActivated();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            _viewModel?.OnPaused();

            if (_viewModel != null)
            {
                _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
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

        // Public Methods
        public UIViewController AsViewController()
        {
            return this;
        }


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
            var payloads = Core.Mvvm.Api.Resolver.Resolve<IPayloads>();
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