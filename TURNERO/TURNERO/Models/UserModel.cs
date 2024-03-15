using Humanizer;
using NuGet.Common;
using NuGet.Protocol.Plugins;

namespace TURNERO.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public int? type { get; set; }
        public string user { get; set; }
        public string mail { get; set; }
        public string? name { get; set; }
        public string? pass { get; set; }
        public string? confirm { get; set; }
        public string? token { get; set; }
        public bool? change_pass { get; set; }
        public int region_id { get; set; }
        public int status { get; set; }
        public List<BranchModel>? branchs { get; set; }
        
        public class Branchs
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
    public class AdminModel : UserModel
    { 
        public string? surname { get; set; }
        public bool user_config { get; set; }
        public bool provider_config { get; set; }
        public bool branch_config { get; set; }
    }

    public class ProviderModel : UserModel
    {
        public int code { get; set; }
        public string cuit { get; set; }
        public string? business_name {  get; set; }
        public string? commercial_mail { get; set; }
        public string? it_mail { get; set; }
        public int region_id { get; set; }
        public string? origin { get; set; }
        public string? address { get; set; }
        public string? observations { get; set; }
        public int? fc_required { get; set; }
    }
}
