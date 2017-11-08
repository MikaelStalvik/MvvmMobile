using System;
using Foundation;
using UIKit;

namespace MvvmMobile.iOS.Common
{
    public class TextViewDelegate : UITextViewDelegate
    {
        private readonly Action _editingEnded;
        private readonly Action<(NSRange range, string replacementString)> _shouldChangeText;

        public TextViewDelegate(Action editingEnded, Action<(NSRange range, string replacementString)> shouldChangeText)
        {
            _editingEnded = editingEnded;
            _shouldChangeText = shouldChangeText;
        }

        public override void EditingEnded(UITextView textView)
        {
            _editingEnded?.Invoke();
        }

        public override bool ShouldChangeText(UITextView textView, NSRange range, string text)
        {
            _shouldChangeText?.Invoke((range, text));

            return false;
        }
    }
}