﻿using System;
using MvvmMobile.Core.ViewModel;

namespace MvvmMobile.Droid.Model
{
    internal sealed class FragmentContainerPayload : IFragmentContainerPayload
    {
        public Type FragmentType { get; set; }
        public IPayload FragmentPayload { get; set; }
        public Action<Guid> FragmentCallback { get; set; }
    }
}