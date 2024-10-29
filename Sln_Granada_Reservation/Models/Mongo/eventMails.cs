using MongoDB.Bson;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models.Mongo
{
    public class eventMails
    {
        public string _id { get; set; }
        public DateTime timestamp { get; set; }
        public string destination { get; set; }
        public string subject { get; set; }
        public string lastEvent { get; set; }
        public List<History> history { get; set; }
        public int __v { get; set; }
        public DateTime lastInteraction { get; set; }
    }


    public class History
    {
        public string @event { get; set; }
        public DateTime? timestamp { get; set; }
    }
}

//Acceso a la plataforma de reserva de asientos para la cena