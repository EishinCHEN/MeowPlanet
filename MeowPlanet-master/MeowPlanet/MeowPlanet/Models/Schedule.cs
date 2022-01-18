using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class Schedule
    {
        public int ScheduleId { get; set; }
        public int UserId { get; set; }
        public int CatId { get; set; }
        public DateTime Date { get; set; }

        public virtual Cat Cat { get; set; }
        public virtual UserData User { get; set; }
    }
}
