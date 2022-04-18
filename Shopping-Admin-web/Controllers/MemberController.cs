using Newtonsoft.Json;
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

            if (httpRequest.Params["account"] == null || httpRequest.Params["enabled"] == null)
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                int enabled = Convert.ToInt32(Convert.ToBoolean(httpRequest.Params["enabled"]));

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"enabled=> {enabled}");

                AccountValidator accValidator = new AccountValidator();

                // 檢查帳號
                if (!accValidator.IsAccountValid(account))
                {
                    result.Set(103, "帳號密碼不符合規則");
                    return result.Stringify();
                }

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_editMember", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
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
        /// 會員儲值
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/deposit")]
        public string Deposit()
        {
            var dict = new Dictionary<string, object>();
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Params["account"] == null || httpRequest.Params["cash"] == null)
            {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try
            {
                string account = httpRequest.Params["account"];
                string cash = httpRequest.Params["cash"];

                Debug.WriteLine($"account=> {account}");
                Debug.WriteLine($"cash=> {cash}");

                AccountValidator accValidator = new AccountValidator();
                NumberValidator numberValidator = new NumberValidator();

                // 檢查帳號
                if (!accValidator.IsAccountValid(account))
                {
                    result.Set(103, "帳號密碼不符合規則");
                    return result.Stringify();
                }

                // 檢查數字
                if (!numberValidator.IsPureNumber(cash) || Convert.ToInt32(cash) <= 0)
                {
                    result.Set(109, "無效的參數");
                    return result.Stringify();
                }

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_editMemberCash", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@cash", cash);
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
                                    id = Convert.ToInt16(r["f_id"]),
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

        /// <summary>
        /// 取得會員購買記錄清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getMemberPurchaseHistory")]
        public string GetMemberPurchaseHistory(PurchaseHistoryPayload payload)
        {
            Debug.WriteLine($"payload=> {JsonConvert.SerializeObject(payload)}");
            Result result = new Result(100, "缺少參數");
            List<PurchaseHistory> purchaseHistories = new List<PurchaseHistory> { };

            if (payload == null || payload.startDate == null || payload.dueDate == null)
            {
                return result.Stringify();
            }

            // 檢查帳號
            string account = payload.account;
            if (payload.account != null)
            {
                AccountValidator accountValidator = new AccountValidator();
                if (!accountValidator.IsAccountValid(account))
                {
                    result.Set(103, "account not valid");
                    return result.Stringify();
                }
            }

            var startDate = DateTime.Parse(payload.startDate).ToUniversalTime();
            var dueDate = DateTime.Parse(payload.dueDate).ToUniversalTime();
            TimeSpan span = dueDate.Subtract(startDate);

            if (span.CompareTo(TimeSpan.Zero) < 0)
            {
                result.Set(121, "時間範圍錯誤");
                return result.Stringify();
            }
            Debug.WriteLine($"account=> {account}");
            Debug.WriteLine($"startDate=> {startDate}");
            Debug.WriteLine($"dueDate=> {dueDate}");

            try
            {
                DataSet ds = new DataSet();
                using (SqlConnection conn = new SqlConnection(connectString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getMemberPurchaseHistory", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (account == null)
                            cmd.Parameters.AddWithValue("@account", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@account", account);
                        cmd.Parameters.AddWithValue("@startDate", startDate);
                        cmd.Parameters.AddWithValue("@dueDate", dueDate);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        adapter.Fill(ds);
                    }
                }
                DataTable tb_purchaseHistory = ds.Tables[0];
                DataTable tb_subPurchaseHistory = ds.Tables[1];

                foreach (DataRow row in tb_purchaseHistory.Rows)
                {
                    DataRow[] rows = tb_subPurchaseHistory.Select($"orderNumber = {row["orderNumber"]}");
                    List<HistoryPurchasedItem> tempShoppingList = new List<HistoryPurchasedItem> { };

                    foreach (DataRow r in rows)
                    {
                        tempShoppingList.Add(new HistoryPurchasedItem
                        {
                            id = Convert.ToInt32(r["productID"]),
                            name = r["productName"].ToString(),
                            price = Convert.ToInt32(r["productPrice"]),
                            count = Convert.ToInt32(r["count"])
                        });
                    }

                    purchaseHistories.Add(new PurchaseHistory
                    {
                        orderNumber = row["orderNumber"].ToString(),
                        account = row["account"].ToString(),
                        phone = row["phone"].ToString(),
                        address = row["address"].ToString(),
                        createdDate = ((DateTime)row["createdDate"]).ToString("yyyy-MM-ddTHH:mm:sssZ"),
                        shoppingList = new List<HistoryPurchasedItem>(tempShoppingList)
                    });
                }
                Debug.WriteLine($"tb_purchaseHistory=> {JsonConvert.SerializeObject(tb_purchaseHistory)}");
                Debug.WriteLine($"tb_subPurchaseHistory=> {JsonConvert.SerializeObject(tb_subPurchaseHistory)}");
                Debug.WriteLine($"purchaseHistories=> {JsonConvert.SerializeObject(purchaseHistories)}");
                Debug.WriteLine($"purchaseHistories count=> {purchaseHistories.Count}");

                result.Set(200, "success", purchaseHistories);
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