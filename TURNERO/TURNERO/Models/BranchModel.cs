namespace TURNERO.Models
{
    public class BranchModel
    {
        public int id { get; set; }  
        public string name { get; set; }
        public string code { get; set; }
        public string? business_name { get; set; }
        public string? commercial_mail { get; set; }
        public int region_id { get; set; }
        public string? address { get; set; }
        public string? longitude { get; set; }
        public string? latitude { get; set; }
        public int company_id { get; set; }
    }
}
