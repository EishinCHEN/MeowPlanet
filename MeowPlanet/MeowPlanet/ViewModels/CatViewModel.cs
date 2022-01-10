using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MeowPlanet.ViewModels
{
    public class CatViewModel
    {
        public int CatId { get; set; }
        public int UserId { get; set; }
        public List<string> Image { get; set; }
        
        public string PhotoPath { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string  CatColor { get; set; }
        public int CatGender { get; set; }
        public string Age { get; set; }
        public string Ligation { get; set; }
        public string Vaccine  { get; set; }
        public string Chip { get; set; }
        public string Remark { get; set; }
        public DateTime? Date { get; set; }
    }
}
