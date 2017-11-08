using Foundation;
using MvvmMobile.Core.Binding;
using MvvmMobile.Core.ViewModel;
using MvvmMobile.iOS.Common;
using UIKit;

namespace MvvmMobile.iOS.Binding
{
    public static class ViewModelExtensions
    {
        // UITextField
        public static void SetBinding(this IBaseViewModel vm, UITextField textField, string path, BindingMode mode = BindingMode.OneWay, IValueConverter converter = null)
        {
            if (vm == null)
            {
                return;
            }

            if (mode == BindingMode.TwoWay)
            {
                textField.Delegate = new TextFieldDelegate(null, ((NSRange range, string replacementString) result) => 
                {
                    var text = textField.Text.Replace(result.range, new NSString(result.replacementString));

                    if (converter != null)
                    {
                        BindingHelper.SetDeepPropertyValue(vm, path, converter.ConvertBack(text));
                    }
                    else
                    {
                        BindingHelper.SetDeepPropertyValue(vm, path, text);
                    }
                });
            }

            var propertyName = path.Split('.')[0];

            vm.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == propertyName)
                {
                    if (converter != null)
                    {
                        textField.Text = converter.Convert(BindingHelper.GetDeepPropertyValue(vm, path)).ToString();
                    }
                    else
                    {
                        textField.Text = BindingHelper.GetDeepPropertyValue(vm, path).ToString();
                    }
                }
            };
        }

        // UITextView
        public static void SetBinding(this IBaseViewModel vm, UITextView textView, string path, BindingMode mode = BindingMode.OneWay, IValueConverter converter = null)
        {
            if (vm == null)
            {
                return;
            }

            if (mode == BindingMode.TwoWay)
            {
                textView.Delegate = new TextViewDelegate(null, ((NSRange range, string replacementString) result) => 
                {
                    var text = textView.Text.Replace(result.range, new NSString(result.replacementString));

                    if (converter != null)
                    {
                        BindingHelper.SetDeepPropertyValue(vm, path, converter.ConvertBack(text));
                    }
                    else
                    {
                        BindingHelper.SetDeepPropertyValue(vm, path, text);
                    }
                });
            }

            var propertyName = path.Split('.')[0];

            vm.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == propertyName)
                {
                    if (converter != null)
                    {
                        textView.Text = converter.Convert(BindingHelper.GetDeepPropertyValue(vm, path)).ToString();
                    }
                    else
                    {
                        textView.Text = BindingHelper.GetDeepPropertyValue(vm, path).ToString();
                    }
                }
            };
        }

        // UILabel
        public static void SetBinding(this IBaseViewModel vm, UILabel label, string path, IValueConverter converter = null)
        {
            if (vm == null)
            {
                return;
            }

            var propertyName = path.Split('.')[0];

            vm.PropertyChanged += (sender, e) => 
            {
                if (e.PropertyName == propertyName)
                {
                    if (converter != null)
                    {
                        label.Text = converter.Convert(BindingHelper.GetDeepPropertyValue(vm, path)).ToString();
                    }
                    else
                    {
                        label.Text = BindingHelper.GetDeepPropertyValue(vm, path).ToString();
                    }
                }
            };
        }
    }
}