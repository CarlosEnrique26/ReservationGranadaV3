using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class TbUser
    {
        public int Id { get; set; }
        public string NameUser { get; set; }
        public string DNI { get; set; }
        public string UserName { get; set; }
        public int TypeUser { get; set; }
        public string NameFriend { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string PasswordUser { get; set; }
        public bool Active { get; set; }
    }
}