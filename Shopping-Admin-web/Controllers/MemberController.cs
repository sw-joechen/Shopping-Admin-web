using Newtonsoft.Json;
using Shopping_Admin_web.Class;
using Shopping_Admin_web.Validators;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Shopping_Admin_web.Controller
{
    public class MemberController : ApiController
    {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
     
        /// 註冊會員帳號
        [HttpPost]
        [Route("api/{controller}/registerMember")]
        public string RegisterMember([FromBody] Member payload)
        {
            Result result = new Result(200, "success");
            if (payload == null)
            {
                result.set(100, "params is required");
                return JsonConvert.SerializeObject(result);
            }
            // TODO: pwd要hash過才進庫
            // TODO: 

            // 以下的邏輯要拆分去哪裡?
            AccountValidator validator = new AccountValidator(payload.account);

            if (validator.IsAccountValid())
            {
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    // 開啟連線
                    conn.Open();

                    DateTime dt = DateTime.Now;

                    SqlCommand cmd = new SqlCommand($"INSERT INTO t_members(f_account, f_address, f_phone, f_gender, f_email, f_enabled, f_pwd, f_createdDate, f_updatedDate) " +
                        $"VALUES ('m1', 'address', '0912345678', '1', 'abc@d.com', 0, 'pwd', 2022-04-01 , 2022-04-01);", conn);


                    cmd.ExecuteNonQuery();

                    // 關閉連線
                    conn.Close();
                }
            }
            

            string output = JsonConvert.SerializeObject(result);
            return output;
        }


        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}