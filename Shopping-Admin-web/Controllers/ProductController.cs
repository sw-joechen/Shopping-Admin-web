using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Shopping_Admin_web.Class;
using Shopping_Admin_web.Validators;

namespace Shopping_Admin_web.Controllers {
    public class ProductController : ApiController {
        string connectString = System.Web.Configuration.WebConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 新增商品
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/addProduct")]
        public string AddProduct() {
            Result result = new Result(100, "缺少參數");

            if (!Request.Content.IsMimeMultipartContent()) {
                result.Set(110, "上傳格式錯誤");
                return result.Stringify();
            }

            var httpRequest = HttpContext.Current.Request;

            // 沒上傳圖檔
            if (httpRequest.Files.Count < 1) {
                result.Set(111, "未上傳檔案");
                return result.Stringify();
            }

            Logger.Info($"demo, name: {httpRequest.Params["name"]}, price: {httpRequest.Params["price"]}");
            if (httpRequest.Params["name"] == null || httpRequest.Params["price"] == null ||
                httpRequest.Params["amount"] == null || httpRequest.Params["description"] == null ||
                httpRequest.Params["type"] == null
            ) {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try {
                string name = httpRequest.Params["name"];
                string price = httpRequest.Params["price"];
                string amount = httpRequest.Params["amount"];
                string description = httpRequest.Params["description"];
                string type = httpRequest.Params["type"];
                string picture;

                Logger.Info($"API: addProduct, name: {name}, price: {price}, amount: {amount}, description: {description}, type: {type}");

                // 檢查價格不可為負數
                if (Convert.ToInt32(price) < 0) {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 檢查數量不可為負數
                if (Convert.ToInt32(amount) < 0) {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 檢查名稱
                SpecialCharacterValidator specialCharacterValidator = new SpecialCharacterValidator();
                if (specialCharacterValidator.IsStrContainSpecialCharacter(name)) {
                    result.Set(113, "商品名稱不合法");
                    return result.Stringify();
                }

                // 檢查描述
                if (specialCharacterValidator.IsStrContainSpecialCharacter(description)) {
                    result.Set(115, "商品描述不合法");
                    return result.Stringify();
                }

                var postedFile = httpRequest.Files[0];

                // 串上時戳_原始檔名
                var filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}");
                postedFile.SaveAs(filePath);

                picture = $"/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}";

                // 寫進庫
                using (SqlConnection conn = new SqlConnection(connectString)) {
                    var sqlResponse = 0;
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_addProduct", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@enabled", 1);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@picture", picture);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@type", type);
                        SqlDataReader r = cmd.ExecuteReader();
                        if (r.Read()) {
                            sqlResponse = (int)r["result"];
                        }
                    }

                    if (sqlResponse == 200) {
                        result.Set(200, "success");
                    }
                    else {
                        result.Set(101, "網路錯誤");
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex);
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 取得商品清單
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/getProductsList")]
        public string GetProductsList() {
            var httpRequest = HttpContext.Current.Request;
            Result result = new Result(100, "缺少參數");
            List<Product> productList = new List<Product> { };

            try {
                string paramID = httpRequest.Params["id"];
                string name = httpRequest.Params["name"] ?? null;
                string type = httpRequest.Params["type"] ?? null;
                string paramEnabled = httpRequest.Params["enabled"];

                int enabled = paramEnabled != null ? Convert.ToInt32(Convert.ToBoolean(paramEnabled)) : -1;
                int id = paramID != null ? Convert.ToInt32(paramID) : -1;

                Logger.Info($"API: getProductsList, id: {id}, name: {name}, type: {type}, enabled: {enabled}");

                using (SqlConnection conn = new SqlConnection(connectString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_getProductList", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (id == -1)
                            cmd.Parameters.AddWithValue("@id", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@id", id);

                        if (name == null)
                            cmd.Parameters.AddWithValue("@name", DBNull.Value);
                        else
                            cmd.Parameters.AddWithValue("@name", name);

                        if (type == null)
                            cmd.Parameters.AddWithValue("@type", DBNull.Value);

                        else
                            cmd.Parameters.AddWithValue("@type", type);

                        if (enabled == -1)
                            cmd.Parameters.AddWithValue("@enabled", DBNull.Value);

                        else
                            cmd.Parameters.AddWithValue("@enabled", enabled);

                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.HasRows) {
                            while (r.Read()) {
                                productList.Add(new Product {
                                    id = Convert.ToInt32(r["f_id"]),
                                    name = r["f_name"].ToString(),
                                    description = r["f_description"].ToString(),
                                    price = Convert.ToInt32(r["f_price"]),
                                    picture = $"/Uploads{r["f_picture"]}",
                                    amount = Convert.ToInt32(r["f_amount"]),
                                    type = r["f_type"].ToString(),
                                    enabled = Convert.ToBoolean(Convert.ToInt16(r["f_enabled"])),
                                    createdDate = r["f_createdDate"].ToString(),
                                    updatedDate = r["f_updatedDate"].ToString(),
                                });
                            }
                            result.Set(200, "success", productList);
                        }
                        else {
                            result.Set(112, "找不到該商品");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex);
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 更新商品
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/updateProduct")]
        public string UpdateProduct() {
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (!Request.Content.IsMimeMultipartContent()) {
                result.Set(110, "上傳格式錯誤");
                return result.Stringify();
            }

            if (httpRequest.Params["id"] == null || httpRequest.Params["type"] == null || httpRequest.Params["price"] == null || httpRequest.Params["amount"] == null || httpRequest.Params["name"] == null || httpRequest.Params["description"] == null) {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try {
                int enabled = Convert.ToInt32(Convert.ToBoolean(httpRequest.Params["enabled"]));
                int id = Convert.ToInt32(httpRequest.Params["id"]);
                string name = httpRequest.Params["name"];
                string price = httpRequest.Params["price"];
                string amount = httpRequest.Params["amount"];
                string description = httpRequest.Params["description"];
                string type = httpRequest.Params["type"];
                string picture = $"/{Path.GetFileName(httpRequest.Params["picture"])}";

                Logger.Info($"API: updateProduct, id: {id}, enabled: {enabled}, name: {name}, price: {price}, amount: {amount},description: {description}, type: {type}, picture: {picture}");

                // 檢查價格不可為負數
                if (Convert.ToInt32(price) < 0) {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 檢查數量不可為負數
                if (Convert.ToInt32(amount) < 0) {
                    result.Set(114, "價格與數量不可為負");
                    return result.Stringify();
                }

                // 檢查名稱
                SpecialCharacterValidator specialCharacterValidator = new SpecialCharacterValidator();
                if (name != null && specialCharacterValidator.IsStrContainSpecialCharacter(name)) {
                    result.Set(113, "商品名稱不合法");
                    return result.Stringify();
                }

                // 檢查描述
                if (description != null && specialCharacterValidator.IsStrContainSpecialCharacter(description)) {
                    result.Set(115, "商品描述不合法");
                    return result.Stringify();
                }

                if (httpRequest.Files.Count >= 1) {
                    var postedFile = httpRequest.Files[0];
                    Logger.Info($"postedFile: {postedFile.FileName}");

                    // 串上時戳_原始檔名
                    var filePath = HttpContext.Current.Server.MapPath($"~/Uploads/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}");
                    postedFile.SaveAs(filePath);

                    picture = $"/{DateTimeOffset.Now.ToUnixTimeSeconds()}_{postedFile.FileName}";
                }

                // 檢查完參數再寫進庫
                using (SqlConnection conn = new SqlConnection(connectString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_editProduct", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.Parameters.AddWithValue("@enabled", enabled);
                        cmd.Parameters.AddWithValue("@name", name);
                        cmd.Parameters.AddWithValue("@price", price);
                        cmd.Parameters.AddWithValue("@picture", picture);
                        cmd.Parameters.AddWithValue("@amount", amount);
                        cmd.Parameters.AddWithValue("@description", description);
                        cmd.Parameters.AddWithValue("@type", type);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.Read()) {
                            result.Set(200, "success");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex);
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }

        /// <summary>
        /// 刪除商品
        /// </summary>
        [HttpPost]
        [Route("api/{controller}/delProduct")]
        public string DelProduct() {
            Result result = new Result(100, "缺少參數");
            var httpRequest = HttpContext.Current.Request;

            if (!Request.Content.IsMimeMultipartContent()) {
                result.Set(110, "上傳格式錯誤");
                return result.Stringify();
            }

            if (httpRequest.Params["id"] == null) {
                result.Set(100, "缺少參數");
                return result.Stringify();
            }

            try {
                int id = Convert.ToInt32(httpRequest.Params["id"]);

                Logger.Info($"API: delProduct, id: {id}");

                // 檢查完參數再寫進庫
                using (SqlConnection conn = new SqlConnection(connectString)) {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("pro_saw_delProduct", conn)) {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", id);
                        SqlDataReader r = cmd.ExecuteReader();

                        if (r.Read()) {
                            result.Set(Convert.ToInt32(r["result"]), "success");
                        }
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(ex);
                result.Set(101, "網路錯誤");
            }
            return result.Stringify();
        }
    }
}
