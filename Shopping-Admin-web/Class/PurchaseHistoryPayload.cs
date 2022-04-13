using System;
namespace Shopping_Admin_web.Class
{
    public class PurchaseHistoryPayload
    {
        // 可以自己塞初始值
        // public int demo1 { get; set; } = -1;
        // public bool demo2 { get; set; } = true;
        public string account { get; set; }
        public string startDate { get; set; }
        public string dueDate { get; set; }
    }
}
