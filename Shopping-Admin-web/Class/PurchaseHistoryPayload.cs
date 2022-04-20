namespace Shopping_Admin_web.Class {
    public class PurchaseHistoryPayload {
        /// <summary>
        /// 帳號
        /// </summary>
        public string account { get; set; }

        /// <summary>
        /// 起始時間
        /// </summary>
        public string startDate { get; set; }

        /// <summary>
        /// 結束時間
        /// </summary>
        public string dueDate { get; set; }

        // 可以自己塞初始值
        // public int demo1 { get; set; } = -1;
        // public bool demo2 { get; set; } = true;
    }
}
