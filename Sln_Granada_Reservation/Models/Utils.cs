using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class Utils
    {
        public static string  GenerateAleatPass()
        {
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 4));

            var random = new Random(seed);
            var value = random.Next(0, 5);
            return (value + seed).ToString(); 
        }

 
    }
}