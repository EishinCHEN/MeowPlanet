using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.Models
{
    public partial class CollectionListJoinCat
    {
        public int UserId { get; set; }
        public int CatId { get; set; }
        public string Name { get; set; }
        public string Age { get; set; }
        public string Image { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public int? IsDeleted { get; set; }
        public string Adopt { get; set; }
    }
}
