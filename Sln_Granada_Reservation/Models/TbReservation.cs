using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class TbReservation
    {
        public int Id { get; set; }
        public int TbTableId { get; set; }
        public int TbUserId { get; set; }
        public int TbChair { get; set; }
        public string Observation { get; set; } = "";
        
    }
}