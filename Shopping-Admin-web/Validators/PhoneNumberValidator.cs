using System.Text.RegularExpressions;

namespace Shopping_Admin_web.Validators {
    public class PhoneNumberValidator {
        public bool IsPhoneNumberValid(string phoneNumber) {
            return Regex.IsMatch(phoneNumber, @"^09[0-9]{8}$");
        }
    }
}