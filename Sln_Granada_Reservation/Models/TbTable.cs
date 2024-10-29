using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Sln_Granada_Reservation.Models
{
    public class TbTable
    {
        public int Id { get; set; }
        public string DescriptionTable { get; set; }
        public int TotalPerson { get; set; }
        public string Position { get; set; }
        public bool Active { get; set; }
        public bool IsBlocked { get; set; }  // Nuevo campo para el bloqueo
    }
}