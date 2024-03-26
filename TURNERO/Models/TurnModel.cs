namespace TURNERO.Models
{
    public class TurnModel
    {
        public int id;
        public int provider_id;
        public int branch_id;
        public DateTime time;
        public DateTime entry_time;
        public bool absent;
        public int status;            
        public string? observations;
        public List<FileModel>? files { get; set; }

    }
}
