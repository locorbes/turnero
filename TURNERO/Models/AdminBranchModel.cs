namespace TURNERO.Models
{
    public class AdminBranchModel
    {
        public int id { get; set; }
        public int branch_id { get; set; }
        public int admin_id { get; set; }
        public int profile_id { get; set; }
        public bool confirm {  get; set; }
    }
}
