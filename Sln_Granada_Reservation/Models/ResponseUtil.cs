using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sln_Granada_Reservation.Models
{
    public class ResponseUtil
    {
        public dynamic body { get; set; }
        public bool status { get; set; }
        public string message { get; set; }

        public string GetCredential()
        {
            return "<p><span style=\"color:#ffffff\">Acceso a la plataforma de reserva de asientos para la cena anual de Caja Rural de Granada 2023</span></p>\r\n\r\n<p><span style=\"font-size:12pt\"><span style=\"font-family:&quot;Times New Roman&quot;,serif\"><span style=\"font-size:13.5pt\"><span style=\"font-family:&quot;Arial&quot;,sans-serif\"><img src=\"https://mivoto.es/Resource/Votation/Repository/350Ccrg.png\" style=\"float:left; height:350px; width:350px\" /></span></span></span></span></p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">&nbsp; &nbsp; &nbsp; &nbsp; Buenos dias,&nbsp;</span></span></p>\r\n\r\n<p style=\"margin-left:40px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Aqui dispone de las credenciales para el acceso a la plataforma de reservas de asientos para la cena anual de Caja Rural de Granada 2023.</span></span></p>\r\n\r\n<p style=\"margin-left:40px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Usuario: {user}</span></span></p>\r\n\r\n<p style=\"margin-left:40px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Contrase&ntilde;a: {pass}</span></span></p>\r\n\r\n<p style=\"margin-left:40px\"><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Acceso a la plataforma: CLICK <u><strong><a href=\"https://mivoto.es:86/Home/UserView\" target=\"_blank\"><span style=\"color:#0033cc\">AQUI</span></a></strong></u></span></span></p>\r\n";
        }
        public string GetReservation()
        {
            return "<p style=\"margin-left:40px\"><strong><em><span style=\"font-size:12.0pt\"><span style=\"font-family:&quot;Aptos&quot;,sans-serif\"><span style=\"color:black\">No responda a este correo, este es un correo gen&eacute;rico.</span></span></span></em></strong></p>\r\n\r\n<p style=\"text-align:center\"><span style=\"font-size:12pt\"><span style=\"font-family:&quot;Times New Roman&quot;,serif\"><span style=\"font-size:13.5pt\"><span style=\"font-family:&quot;Arial&quot;,sans-serif\"><img src=\"https://mivoto.es/Resource/Votation/Repository/350Ccrg.png\" style=\"float:left; height:350px; width:350px\" /></span></span></span></span></p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p>&nbsp;</p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Nos complace informarle que su reserva ha quedado confirmada seg&uacute;n lo siguiente:&nbsp;</span></span></p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Reserva: {mesa}&nbsp;</span></span></p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">{silla 1}</span></span></p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Observaciones:&nbsp; &nbsp;{observaciones 1}</span></span></p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">{silla 2}</span></span></p>\r\n\r\n<p><span style=\"font-family:Arial,Helvetica,sans-serif\"><span style=\"font-size:18px\">Observaciones:&nbsp; &nbsp;{observaciones 2}</span></span></p>\r\n";
        }
    }
    public class ResponseReservation
    {
        public int Id { get; set; }
        public string Mesa { get; set; }
        public bool IsComplete { get; set; }
        public int ChairDisponible { get; set; } 
        public int ChairOcupate { get; set; }
    }

    public class ResponseTableDetail
    {
        public string MesaName { get; set; }
        public int MesaId { get; set; }
        public int ChairId { get; set; }
        public string ChairName { get; set; }
        public int UserId { get; set; }
        public string NameUser { get; set; }
        public bool IsOcupate { get; set; }
        public string Observation { get; set; }
    }
    public class ResponseUserTable
    {
        public int TableId { get; set; }
        public string Text { get; set; }
    }

    public class Template
    { 
        public string templateCredential { get; set; }
        public string templateConfirmation { get; set; }
    }

    public class UserMail
    {
        public string destination { get; set; }
        public string lastEvent { get; set; }
        public string lastInteraction { get; set; }
    }

    public class UserResponse
    {
        public string email { get; set; }
        public string body { get; set; }
        public string adjunt { get; set; }
    }

}