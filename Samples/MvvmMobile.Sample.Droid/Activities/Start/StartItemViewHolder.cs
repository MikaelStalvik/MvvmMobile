﻿using System;
using Android.Views;
using Android.Widget;
using MvvmMobile.Sample.Core.Model;
using MvvmMobile.Sample.Droid.Common;

namespace MvvmMobile.Sample.Droid.Activities.Start
{
    public class StartItemViewHolder : Java.Lang.Object
    {
        // Private Members
        private readonly LinearLayout _mainLayout;
        private readonly RelativeLayout _deleteButton;
        private readonly TextView _titleTextView;

        private readonly Action<int> _selectListener;

        private int _deleteButtonWidth;
        private int _position;
        private bool _editMode;
        private float _lastDeltaX;
        private bool _shouldCancelPan;


        // -----------------------------------------------------------------------------

        // Constructors
        public StartItemViewHolder(View itemView, Action<int> selectListener, Action<int> deleteListener)
        {
            // Init
            _selectListener = selectListener;

            // Controls
            _mainLayout = itemView.FindViewById<LinearLayout>(Resource.Id.mainLayout);
            _deleteButton = itemView.FindViewById<RelativeLayout>(Resource.Id.deleteButton);
            _titleTextView = itemView.FindViewById<TextView>(Resource.Id.titleTextView);

            // Measure
            _deleteButton.ViewTreeObserver.AddOnPreDrawListener(new PreDrawListener(() => 
            {
                _deleteButtonWidth = _deleteButton.Width;
                return _deleteButton;
            }));

            // Click Events
            _deleteButton.Click += (sender, e) => deleteListener?.Invoke(_position);

            // Gestures
            _mainLayout.SetOnTouchListener(new SampleOnTouchListener(MainLayoutPanStarted, MainLayoutPanMoved, MainLayoutPanEnded, ItemSelected));
        }


        // -----------------------------------------------------------------------------

        // Public Methods
        public void Update(IMotorcycle motorcycle, int position)
        {
            _position = position;

            if (motorcycle == null)
            {
                return;
            }

            _titleTextView.Text = motorcycle.ToString();
        }


        // -----------------------------------------------------------------------------

        // Private Methods
        private void ItemSelected()
        {
            if (_editMode == false)
            {
                _selectListener?.Invoke(_position);
            }

            HideSwipeButtons(true);
        }

        private void MainLayoutPanStarted(float startX)
        {
            _shouldCancelPan = false;

            if (_editMode)
            {
                HideSwipeButtons(true);
                _shouldCancelPan = true;
                return;
            }
        }

        private void MainLayoutPanMoved(SamplePanMoveArgs args)
        {
            if (_shouldCancelPan)
            {
                return;
            }

            if (_editMode)
            {
                HideSwipeButtons(true);
                _shouldCancelPan = true;
                return;
            }

            if(args.MovedX > 0 || Math.Abs(args.MovedY) > Math.Abs(args.MovedX))
            {
                return;
            }

            _lastDeltaX = Convert.ToInt32(args.MovedX);

            if (_lastDeltaX > 0)
            {
                _lastDeltaX = 0;
            }

            ShowSwipeButtons();
        }

        private void MainLayoutPanEnded()
        {
            if (_editMode)
            {
                return;
            }

            if (_lastDeltaX < 0)
            {
                ShowSwipeButtons(true);
            }
            else if (_lastDeltaX > 0)
            {
                HideSwipeButtons(true);
            }

            _shouldCancelPan = false;
        }

        private void ShowSwipeButtons(bool animate = false)
        {
            if(animate)
            {
                _lastDeltaX = _deleteButtonWidth * -1;

                var anim = new LeftMarginAnimation(_mainLayout, Convert.ToInt32(_lastDeltaX))
                {
                    Duration = 200,
                    StartOffset = 0
                };

                anim.SetAnimationListener(new AnimationListener(null, null, () => 
                {
                    _editMode = true;
                }));

                _mainLayout.StartAnimation(anim);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine(_lastDeltaX);
                var layoutParams = (RelativeLayout.LayoutParams)_mainLayout.LayoutParameters;

                layoutParams.LeftMargin = Convert.ToInt32(_lastDeltaX);
                layoutParams.Width = _mainLayout.MeasuredWidth;

                _mainLayout.LayoutParameters = layoutParams;
            }
        }

        private void HideSwipeButtons(bool animate = false)
        {
            if(animate)
            {
                _lastDeltaX = 0;

                var anim = new LeftMarginAnimation(_mainLayout, Convert.ToInt32(_lastDeltaX))
                {
                    Duration = 200,
                    StartOffset = 0
                };

                anim.SetAnimationListener(new AnimationListener(null, null, () => 
                {
                    _editMode = false;
                }));

                _mainLayout.StartAnimation(anim);
            }
            else
            {
                var layoutParams = (RelativeLayout.LayoutParams)_mainLayout.LayoutParameters;

                layoutParams.LeftMargin = 0;
                layoutParams.Width = ViewGroup.LayoutParams.MatchParent;

                _mainLayout.LayoutParameters = layoutParams;
            }
        }
    }
}