using Shopping_Admin_web.Class;
using Shopping_Admin_web.Validators;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Web;
using System.Web.Http;

namespace Shopping_Admin_web.Controller
{
    public class MemberController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// <summary>
        /// 更新會員帳號
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/updateMember")]
        public string UpdateMember()
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Params["account"] == null || httpRequest.Params["address"] == null || httpRequest.Params["phone"] == null ||
                httpRequest.Params["gender"] == null || httpRequest.Params["email"] == null || httpRequest.Params["enabled"] == null)
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                string address = httpRequest.Params["address"];
                string phone = httpRequest.Params["phone"];
                int gender = Convert.ToInt32(httpRequest.Params["gender"]);
                string email = httpRequest.Params["email"];
                int enabled = Convert.ToInt32(Convert.ToBoolean(httpRequest.Params["enabled"]));

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"address=> {address}");
                Debug.WriteLine($"phone=> {phone}");
                Debug.WriteLine($"gender=> {gender}");
                Debug.WriteLine($"email=> {email}");
                Debug.WriteLine($"enabled=> {enabled}");

                AccountValidator accValidator = new AccountValidator();
                PwdValidator pwdValidator = new PwdValidator();
                SpecialCharacterValidator specialCharacterValidator = new SpecialCharacterValidator();
                EmailValidator emailValidator = new EmailValidator();
                PhoneNumberValidator phoneNumberValidator = new PhoneNumberValidator();

                // 檢查帳號
                if (!accValidator.IsAccountValid(account))
                {
                    result.Set(103, "帳號密碼不符合規則");
                    return result.Stringify();
                }

                // 檢查email
                if (!emailValidator.IsEmailValid(email))
                {
                    result.Set(116, "email格式不合法");
                    return result.Stringify();
                }

                // 檢查phone
                if (!phoneNumberValidator.IsPhoneNumberValid(phone))
                {
                    result.Set(117, "電話號碼格式不合法");
                    return result.Stringify();
                }

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_sw_editMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@address", address);
                        cmd.Parameters.AddWithValue("@phone", phone);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@email", email);
                        cmd.Parameters.AddWithValue("@enabled", enabled);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read())
                        {
                            dict["id"] = r["f_id"];
                            dict["account"] = r["f_account"];
                            dict["address"] = r["f_address"];
                            dict["phone"] = r["f_phone"];
                            dict["gender"] = r["f_gender"];
                            dict["email"] = r["f_email"];
                            dict["enabled"] = r["f_enabled"];
                            dict["balance"] = r["f_balance"];
                            dict["createdDate"] = r["f_createdDate"];
                            dict["updatedDate"] = r["f_updatedDate"];
                            result.Set(200, "success", dict);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 取得會員清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getMembersList")]
        public string GetMembersList()
        {
            var httpRequest = HttpContext.Current.Request;
            Result result = new Result(100, "缺少參數");
            List<Member> memberList = new List<Member> { };

            try
            {
                string account = httpRequest.Params["account"] ?? null;
                string paramEnabled = httpRequest.Params["enabled"];

                int enabled = paramEnabled != null ? Convert.ToInt32(Convert.ToBoolean(paramEnabled)) : -1;

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"enabled=> {enabled}");

                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getMemberList", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (account == null)
                            cmd.Parameters.AddWithValue("@account", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@account", account);

                        if (enabled == -1)
                            cmd.Parameters.AddWithValue("@enabled", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@enabled", enabled);

                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows)
                        {
                            while (r.Read())
                            {
                                memberList.Add(new Member
                                {
                                    id = r["f_id"].ToString(),
                                    account = r["f_account"].ToString(),
                                    address = r["f_address"].ToString(),
                                    phone = r["f_phone"].ToString(),
                                    gender = Convert.ToInt16(r["f_gender"]),
                                    email = r["f_email"].ToString(),
                                    enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"])),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                    balance = Convert.ToDouble(r["f_balance"])
                                });
                            }
                            
                        }
                        result.Set(200, "success", memberList);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"ex: {ex}");
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }
    }
}