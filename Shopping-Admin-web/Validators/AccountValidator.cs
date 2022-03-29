using System.Text.RegularExpressions;

namespace Shopping_Admin_web.Validators
{
    public class AccountValidator
    {        
        public bool IsAccountValid(string account)
        {
            // 帳號規則: 英文開頭, 英數皆可 限6~20字元
            return Regex.IsMatch(account, "^[A-Za-z][A-Za-z0-9]{5,19}$");
        }
    }
}