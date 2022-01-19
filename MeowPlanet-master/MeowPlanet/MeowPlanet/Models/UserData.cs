using System;
using System.Collections.Generic;

#nullable disable

namespace MeowPlanet.Models
{
    public partial class UserData
    {
        public UserData()
        {
            Cats = new HashSet<Cat>();
            ChatLists = new HashSet<ChatList>();
            CollectionLists = new HashSet<CollectionList>();
            RoleManagements = new HashSet<RoleManagement>();
            Schedules = new HashSet<Schedule>();
        }

        public int UserId { get; set; }
        public string Account { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string EmailConfirm { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public string Job { get; set; }
        public string Salary { get; set; }
        public string AcceptableAmount { get; set; }
        public string Merrage { get; set; }
        public string OtherPets { get; set; }
        public string KeepPets { get; set; }
        public string Agents { get; set; }
        public string RelationShip { get; set; }
        public string PersonalPhoto { get; set; }

        public virtual ICollection<Cat> Cats { get; set; }
        public virtual ICollection<ChatList> ChatLists { get; set; }
        public virtual ICollection<CollectionList> CollectionLists { get; set; }
        public virtual ICollection<RoleManagement> RoleManagements { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
