namespace Shopping_Admin_web.Class {
    public class SearchAgent {
        /// <summary>
        /// id
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 啟用
        /// </summary>
        public int enabled { get; set; }

        /// <summary>
        /// 建立時間
        /// </summary>
        public string createdDate { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public string updatedDate { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string role { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string pwd { get; set; }

        /// <summary>
        /// 錯誤次數
        /// </summary>
        public string count { get; set; }
    }
}