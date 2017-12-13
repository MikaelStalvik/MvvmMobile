using MvvmMobile.Core.Binding;

namespace MvvmMobile.Sample.Core.ValueConverter
{
    public class IntToStringValueConverter : IValueConverter
    {
        public object Convert(object value)
        {
            return value.ToString();
        }

        public object ConvertBack(object value)
        {
            int.TryParse(value.ToString(), out int intValue);

            return intValue;
        }
    }
}