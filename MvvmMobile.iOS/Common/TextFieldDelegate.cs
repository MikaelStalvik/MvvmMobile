using System;
using Foundation;
using UIKit;

namespace MvvmMobile.iOS.Common
{
    public class TextFieldDelegate : UITextFieldDelegate
    {
        private readonly Action _editingEnded;
        private readonly Action<(NSRange range, string replacementString)> _shouldChangeCharacters;

        public TextFieldDelegate(Action editingEnded, Action<(NSRange range, string replacementString)> shouldChangeCharacters)
        {
            _editingEnded = editingEnded;
            _shouldChangeCharacters = shouldChangeCharacters;
        }

        public override void EditingEnded(UITextField textField)
        {
            _editingEnded?.Invoke();
        }

        public override bool ShouldChangeCharacters(UITextField textField, NSRange range, string replacementString)
        {
            _shouldChangeCharacters?.Invoke((range, replacementString));

            return false;
        }
    }
}