namespace Shopping_Admin_web.Class
{
    public class Member
    {
        public int id { get; set; }
        public string account { get; set; }
        public string address { get; set; }
        public string phone { get; set; }
        public int gender { get; set; }
        public string email { get; set; }
        public bool enabled { get; set; }
        public string createdDate { get; set; }
        public string updatedDate { get; set; }
        public double balance { get; set; }
    }
}