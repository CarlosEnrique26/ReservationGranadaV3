using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class TbConfig
    {
        public int Id { get; set; }
        public string Enterprise { get; set; }
        public string BackgroundTable { get; set; }
        public string URL { get; set; }
        public string txtCredential { get; set; }
        public string txtConfirm { get; set; }
    }
}