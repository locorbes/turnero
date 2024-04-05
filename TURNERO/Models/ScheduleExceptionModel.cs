using System;

namespace TURNERO.Models
{
    public class ScheduleExceptionModel
    {
        public int id { get; set; }
        public int branch_id { get; set; }
        public int day { get; set; }
        public TimeSpan since { get; set; }
        public TimeSpan until { get; set; }
        public int turn_minutes {  get; set; }
        public int turn_maximum {  get; set; }        
    }
}
