using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping_Admin_web.Validators
{
    public class AccountValidator
    {
        private string account;
        public AccountValidator(string account)
        {
            this.account = account;
        }

        public bool IsAccountValid() {
            bool result = true;

            // TODO: 驗證帳號合法性, 唯一性
            return result;
        }
    }
}