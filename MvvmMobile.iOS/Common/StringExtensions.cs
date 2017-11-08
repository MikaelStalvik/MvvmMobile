using Foundation;

namespace MvvmMobile.iOS.Common
{
    public static class StringExtensions
    {
        public static string Replace(this string s, NSRange range, string replacementString)
        {
            var text = new NSString(s);

            return text.Replace(range, new NSString(replacementString));
        }
    }
}