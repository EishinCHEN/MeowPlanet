using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace MeowPlanet.Models
{
    public partial class ChatList
    {
        [NotMapped]
        public dynamic ImageToUpload { get; set; }
    }
}
