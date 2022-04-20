using System.Text.RegularExpressions;

namespace Shopping_Admin_web.Validators {
    public class SpecialCharacterValidator {
        public bool IsStrContainSpecialCharacter(string str) {
            // 過濾特殊字元(包含空格)
            return Regex.IsMatch(str, "[\\s`~!@#$%^&*\"()_+\\-=[\\]{};':\\|,.<>\\/?]+");
        }
    }
}