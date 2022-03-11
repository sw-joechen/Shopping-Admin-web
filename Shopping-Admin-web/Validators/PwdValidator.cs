using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Shopping_Admin_web.Validators
{
    public class PwdValidator
    {
        private readonly string pwd;
        public PwdValidator(string pwd)
        {
            this.pwd = pwd;
        }

        public bool IsPwdValid()
        {
            bool result = false;

            // 密碼規則: 6 位數以上，並且至少包含大寫字母、小寫字母、數字各一
            Regex regex = new Regex("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{6,}$");
            if (regex.IsMatch(pwd))
                result = true;

            return result;

        }
    }
}