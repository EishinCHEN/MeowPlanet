using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Models
{
    public partial class CollectionListJoinCat
    {
        //public int CollectionListId { get; set; }
        public int UserId { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? IsDeleted { get; set; }

        //public virtual Cat Cat { get; set; }
        //public virtual UserData User { get; set; }
    }
}