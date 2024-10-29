using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class TbChair
    {
        public int Id { get; set; }
        public int TbTableId { get; set; }
        public string DescriptionChair { get; set; }
        public string Position { get; set; }
        public bool Active { get; set; } 
        public bool IsOcupate { get; set; }
        public DateTime? DateReservation { get; set; }
    }
}