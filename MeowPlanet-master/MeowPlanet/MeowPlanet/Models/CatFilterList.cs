using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Models
{
    public class CatFilterList
    {
    
        public string UserId { get; set; }
        public int CatId { get; set; }
        public string CatColor { get; set; }
        public string Ligation { get; set; }
        public string Age { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int? CatGender { get; set; }
        public string Vaccine { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? IsDeleted { get; set; }

        public DateTime? PublishedDay { get; set; }
    }
}
