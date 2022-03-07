using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopping_Admin_web.Validators
{
    public class PwdValidator
    {
        private string pwd;
        public PwdValidator(string pwd)
        {
            this.pwd = pwd;
        }

        public bool IsPwdValid()
        {
            bool result = true;

            // TODO: 驗證密碼符合規則
            return result;
        }
    }
}