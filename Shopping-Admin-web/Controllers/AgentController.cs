using Newtonsoft.Json;
using Shopping_Admin_web.Class;
using Shopping_Admin_web.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Shopping_Admin_web.Controllers
{
    public class AgentController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;

        /// 註冊管理帳號
        [HttpPost]
        [Route("api/{controller}/registerAgent")]
        public string RegisterAgent([FromBody] Agent payload)
        {
            Result result = new Result(200, "success");
            if (payload == null)
            {
                result.set(100, "params is required 帳號規則錯誤");
                return JsonConvert.SerializeObject(result);
            }
            // TODO: pwd要hash過才進庫
            // TODO: 以下的商業邏輯要拆分去哪裡?
            AccountValidator accountValidator = new AccountValidator(payload.account);
            PwdValidator pwdValidator = new PwdValidator(payload.pwd);

            if (accountValidator.IsAccountValid() && pwdValidator.IsPwdValid())
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    // 開啟連線
                    conn.Open();

                    DateTime dt = DateTime.Now;

                    // TODO: Date拉到庫給預設值
                    SqlCommand cmd = new SqlCommand($"INSERT INTO t_agents(f_account, f_enabled, f_pwd, f_createdDate, f_updatedDate, f_role) VALUES('{payload.account}', 0, '{payload.pwd}', '{dt.GetDateTimeFormats('s')[0].ToString()}', '{dt.GetDateTimeFormats('s')[0].ToString()}', '');", conn);

                    cmd.ExecuteNonQuery();

                    // 關閉連線
                    conn.Close();
                }
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }

        // 登入後台帳號
        [HttpPost]
        [Route("api/{controller}/loginAgent")]
        public string LoginAgent([FromBody] Agent payload)
        {
            Result result = new Result(100, "fail");
            if (payload == null)
            {
                result.set(100, "params is required");
                return JsonConvert.SerializeObject(result);
            }
            using (SqlConnection conn = new SqlConnection(connectString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand($"SELECT * FROM t_agents WHERE f_account = '{payload.account}'", conn);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        if (reader["f_pwd"].ToString() == payload.pwd)
                        {
                            result.set(200, "success");
                        }
                        else
                        {
                            result.set(100, "wrong password");
                        }
                    }
                    else
                    {
                        result.set(100, "member not found");
                    }
                    conn.Close();
                }
            }

            string output = JsonConvert.SerializeObject(result);
            return output;
        }
    }
}