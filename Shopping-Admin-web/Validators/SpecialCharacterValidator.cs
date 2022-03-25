using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Shopping_Admin_web.Validators
{
    public class SpecialCharacterValidator
    {
        public bool IsStrContainSpecialCharacter(string str)
        {
            // 過濾特殊字元(包含空格)
            return Regex.IsMatch(str, "[\\s`~!@#$%^&*\"()_+\\-=[\\]{};':\\|,.<>\\/?]+");
        }
    }
}