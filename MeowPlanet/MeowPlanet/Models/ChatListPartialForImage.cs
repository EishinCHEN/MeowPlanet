using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeowPlanet.Models
{
    public partial class ChatList
    {
        [NotMapped]
        public string ImageToUpload { get; set; }
    }
}
