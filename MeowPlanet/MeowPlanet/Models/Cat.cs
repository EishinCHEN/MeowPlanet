using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class Cat
    {
        public Cat()
        {
            CollectionLists = new HashSet<CollectionList>();
            Schedules = new HashSet<Schedule>();
        }

        public int CatId { get; set; }
        public string Adopt { get; set; }
        public string Adopter { get; set; }
        public int UserId { get; set; }
        public string CatColor { get; set; }
        public string Ligation { get; set; }
        public string Age { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int? CatGender { get; set; }
        public string Vaccine { get; set; }
        public string Chip { get; set; }
        public string Remark { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int? IsDeleted { get; set; }
        public DateTime? PublishedDay { get; set; }

        public virtual UserData User { get; set; }
        public virtual ICollection<CollectionList> CollectionLists { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
