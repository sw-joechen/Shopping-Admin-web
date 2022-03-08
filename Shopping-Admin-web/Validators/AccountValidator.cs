using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Shopping_Admin_web.Validators
{
    public class AccountValidator
    {
        private string account; 

        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        public AccountValidator(string account)
        {
            this.account = account;
        }

        public bool IsAccountValid() {
            bool result = false;

            // 帳號規則: 英文開頭, 英數皆可 限6~20字元 
            Regex regex = new Regex("^[A-Za-z][A-Za-z0-9]{5,19}");
            if (regex.IsMatch(account))            
                result = true;
            return result;
        }
    }
}