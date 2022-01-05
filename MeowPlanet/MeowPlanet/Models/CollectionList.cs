using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class CollectionList
    {
        public int CollectionListId { get; set; }
        public int UserId { get; set; }
        public int CatId { get; set; }

        public virtual Cat Cat { get; set; }
        public virtual UserData User { get; set; }
    }
}
