using System.Text.RegularExpressions;

namespace Shopping_Admin_web.Validators
{
    public class NumberValidator
    {
        public bool IsPureNumber(string payloadNumber) {
            return Regex.IsMatch(payloadNumber, @"^\d+$");
        }
    }
}