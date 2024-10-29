
using Amazon;
using Amazon.Runtime;
using Amazon.SimpleEmail.Model;
using Amazon.SimpleEmail;
using OfficeOpenXml;
using Sln_Granada_Reservation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Text;
using static System.Net.WebRequestMethods;
using System.Threading.Tasks;
using System.Data.Entity;
using Sln_Granada_Reservation.Filters; // Asegúrate de que este espacio de nombres esté incluido

namespace Sln_Granada_Reservation.Controllers
{
    public class HomeController : Controller
    {
        public DBContext dBContext;

        public HomeController()
        {
            dBContext = new DBContext();
        }
        public ActionResult Index()
        {
            if (Session["admin"] == null) 
                return RedirectToAction("Login"); 
            else
                return View();
        }
        public ActionResult Login()
        {

            return View();
        }
       
        public ActionResult LogOut()
        {
            // Limpiar la sesión
            Session.Clear(); // Elimina todos los elementos de la sesión
            Session.Abandon(); // Abandona la sesión actual
            return RedirectToAction("Login"); // Redirige a la página de inicio de sesión
        }

        public ActionResult Config()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }

        public ActionResult About()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }

        public ActionResult Contact()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }

        ///

        public ActionResult Perfil()
        {
            // Verificar si hay un usuario en la sesión (admin o usuario regular)
            TbUser user = null;

            if (Session["admin"] != null)
            {
                user = Session["admin"] as TbUser;  // Cargar usuario admin de la sesión
            }
            else if (Session["user"] != null)
            {
                user = Session["user"] as TbUser;  // Cargar usuario regular de la sesión
            }

            // Si no hay usuario en la sesión, redirigir a la página de inicio de sesión
            if (user == null)
            {
                return RedirectToAction("Login");
            }

            // Enviar los datos del usuario a la vista
            return View(user);
        }


        ///

        public ActionResult Table()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }
        public ActionResult Users()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }
        public ActionResult Reservation()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }

        public ActionResult ReservationReport()
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
                return View();
        }
    
        public ActionResult ReservationDetail(int tableid)
        {
            if (Session["admin"] == null)
                return RedirectToAction("Login");
            else
            { 
                ViewBag.TableId = tableid;
                return View();
            }
             
        }

        public ActionResult UserView()
        {
            return View();
        }
        
        public ActionResult ViewResumeTableUser()
        {
            if (Session["user"] != null)
            {

                var user = (TbUser)Session["user"];
                ViewBag.UserId = user.Id;
                ViewBag.NameUser = user.NameUser;
                return View();
            }
            else
            {
                return RedirectToAction("UserView");
            }
        }

        public ActionResult ViewTableUser()
        {
            if(Session["user"] != null){

                var user = (TbUser)Session["user"];
                ViewBag.UserId = user.Id;
                ViewBag.NameUser = user.NameUser;

                return View();
            }
            else
            {
                return RedirectToAction("UserView");
            }
        }


        [HttpGet]
        public JsonResult FindChairOcupate(int chairid)
        {
            var chair = dBContext.TbChair.Where(x => x.Id == chairid).FirstOrDefault();
            if (chair.IsOcupate)
            {
                TimeSpan? v = DateTime.Now - chair.DateReservation;
                if(v.Value.Minutes > 2)
                { 
                    chair.IsOcupate = false;
                    chair.DateReservation = null;
                    dBContext.TbChair.Add(chair);
                    dBContext.Entry(chair).State = System.Data.Entity.EntityState.Modified;
                    dBContext.SaveChanges();

                    return Json(new ResponseUtil()
                    {
                        status = false,
                        message = ""
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResponseUtil()
                    {
                        status = true,
                        message = "La silla esta siendo reservada por otro usuario, desea cambiar de silla?"
                    }, JsonRequestBehavior.AllowGet);

                } 
            }
            else
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = ""
                }, JsonRequestBehavior.AllowGet);

           
        }

        [HttpGet]
        public JsonResult SendInformationAlarm()
        {
            var listUser = new List<UserResponse>();
            try
            {
                var tables = dBContext.TbTable.ToList();
                var config = dBContext.TbConfig.Find(1);

                foreach (var table in tables)
                {
                    var reservations = dBContext.TbReservation.Where(x => x.TbTableId == table.Id).ToList();
                    var chairs = dBContext.TbChair.Where(x => x.TbTableId == table.Id).ToList();
                    var users = (from x in reservations select new { x.TbUserId }).Distinct().ToList();

                    foreach (var user in users)
                    {
                        
                            var userReservation = reservations.Where(x => x.TbUserId == user.TbUserId).ToList();
                            var emailhtml = config.txtConfirm;
                            emailhtml = emailhtml.Replace("{mesa}", table.DescriptionTable);

                            var userInfo = dBContext.TbUser.Where(x => x.Id == user.TbUserId).FirstOrDefault();
                            var email = "Reserva : " + table.DescriptionTable + "  ";
                            var num = 1;
                            if ((userInfo.Email.Contains("SIN MAIL")) || String.IsNullOrEmpty(userInfo.Email) || userInfo.Email == "M9855")
                            {

                            }
                            else
                            {
                                foreach (var row in userReservation)
                                {
                                    var chair = chairs.Where(x => x.Id == row.TbChair).FirstOrDefault();
                                    if (num == 1)
                                        emailhtml = emailhtml.Replace("{silla " + num + "}", chair.DescriptionChair + " : D/Dª : " + userInfo.NameUser);
                                    else
                                        emailhtml = emailhtml.Replace("{silla " + num + "}", chair.DescriptionChair + ": Acompañante D/Dª : " + userInfo.NameUser);

                                    emailhtml = emailhtml.Replace("{observaciones " + num + "}", row.Observation);
                                    num++;
                                }

                            var userResponse = new UserResponse() { 
                                adjunt = "Información Convención Anual Empleados 2023",
                                email = userInfo.Email,
                                body = emailhtml
                            };
                            listUser.Add(userResponse);
                         } 
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }


            Parallel.ForEach(listUser, row => {
                var usermodel = row;
                sendMailAmazon(usermodel.email, usermodel.body, usermodel.adjunt); 
            });

            return Json("",JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SeparateChairOcupate(int chairid, bool status)
        {
            try
            {
                var chair = dBContext.TbChair.Where(x => x.Id == chairid).FirstOrDefault();
                chair.IsOcupate = status;
                chair.DateReservation = DateTime.Now;
                dBContext.TbChair.Add(chair);
                dBContext.Entry(chair).State = System.Data.Entity.EntityState.Modified;
                dBContext.SaveChanges();

                return Json(new ResponseUtil()
                {
                    status = true,
                    message = " "
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = " "
                }, JsonRequestBehavior.AllowGet); 
            }
 
        }

        [HttpPost]
        public JsonResult SendEmail(List<TbUser> users)
        { 
            ResponseUtil rsuti  = new ResponseUtil();
            try
            {
                var url = dBContext.TbConfig.Find(1);
                foreach (var item in users)
                {
                    var htmltext = url.txtCredential;
                    htmltext = htmltext.Replace("{user}", item.UserName);
                    htmltext = htmltext.Replace("{pass}", item.PasswordUser);
                    htmltext = htmltext.Replace("{url}", url.URL);

                    sendMailAmazon(item.Email, htmltext, "Acceso a la plataforma de reserva de asientos para la cena anual de Caja Rural Granada 2023");
                }
                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Credenciales enviadas correctamente."
                });
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "Ocurrio un error al enviar las credenciales."
                });
            }
  
        }

        [HttpGet]
        public JsonResult GetUserListMongo()
        {
            MongoDBConnection mongoDBConnection = new MongoDBConnection();
            var list = mongoDBConnection.GetDocumentParticipant();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUserLikeTable(string search)
        {
            var listTable = new List<ResponseUserTable>();

            // Obtener solo las mesas que no están bloqueadas

            if (string.IsNullOrEmpty(search))
            {
            var tables = dBContext.TbTable.Where(x => !x.IsBlocked).ToList();

                foreach (var item in tables)
                {
                    var reservation = dBContext.TbReservation.Where(x => x.TbTableId == item.Id).ToList();
                    var tableModel = new ResponseUserTable()
                    {
                        TableId = item.Id,
                        Text = item.DescriptionTable + " Disponibles(" + (item.TotalPerson - reservation.Count) + ")"
                    };

                    listTable.Add(tableModel);
                }
            }

            return Json(listTable.Distinct().ToList(), JsonRequestBehavior.AllowGet);
        }

       
        bool validatePreviusTable(int tableid)
        {
            var chairsReservation = dBContext.TbReservation.Where(x => x.TbTableId == tableid).ToList();
            return chairsReservation.Count > 0;
        }
        
        [HttpPost]
        public JsonResult SaveTable(TbTable model)
        {
            try
            {
                int tableid = 0;
                int totalPersonOld = 0;
                if (model.Id > 0)
                {
                    var table = dBContext.TbTable.Find(model.Id);
                    totalPersonOld = table.TotalPerson;
                    table.DescriptionTable = model.DescriptionTable;
                    table.TotalPerson = model.TotalPerson;
                    table.Active = model.Active;
                    table.IsBlocked = model.IsBlocked;  // Actualizar el estado de bloqueo
                    dBContext.TbTable.Add(table);
                    dBContext.Entry(table).State = System.Data.Entity.EntityState.Modified; 
                    tableid = table.Id;
                }
                else
                {

                    dBContext.TbTable.Add(model);
                    dBContext.SaveChanges();
                    tableid = model.Id;
                }

           
              if (totalPersonOld != model.TotalPerson)
              {
                    //verificamos que no exista reserva
                    if (!validatePreviusTable(tableid))
                    {
                        var chairs = dBContext.TbChair.Where(x => x.TbTableId == tableid).ToList();
                        dBContext.TbChair.RemoveRange(chairs);
                        dBContext.SaveChanges();

                        for (int i = 0; i < model.TotalPerson; i++)
                        {
                            var chair = new TbChair()
                            {
                                DescriptionChair = "Silla " + (i + 1),
                                Position = "",
                                Active = true,
                                TbTableId = tableid
                            };
                            dBContext.TbChair.Add(chair);
                            dBContext.SaveChanges();
                        }
                         
                    }
                    else
                    {
                        return Json(new ResponseUtil()
                        {
                            status = false,
                            message = "Si desea cambiar la cantidad de sillas es necesario eliminar la reserva"
                        });
                    }

                }
                else  dBContext.SaveChanges(); 

                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Mesa guardada correctamente"
                });
 

            }
            catch (Exception ex)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "Ocurrio un error al crear la mesa, intentelo nuevamente"
                });
            }
          
        }
        
        [HttpGet]
        public JsonResult GetTableById(int tableid)
        {
            var tables = dBContext.TbTable.Where(x => x.Id == tableid).FirstOrDefault();
            return Json(tables, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetTables()
        {
            var tables = dBContext.TbTable.ToList();
            return Json(tables, JsonRequestBehavior.AllowGet);
        }
        
        

        [HttpPost]
        public JsonResult SaveChair(TbChair model)
        {
            dBContext.TbChair.Add(model);
            dBContext.SaveChanges();
            return Json("");
        }

        [HttpPost]
        public JsonResult SaveUser(TbUser model)
        { 

            try
            {
                if (model.Id > 0)
                {
                    var user = dBContext.TbUser.Find(model.Id);
                    user.NameUser = model.NameUser;
                    user.DNI = model.DNI;
                    user.UserName = model.UserName;
                    user.NameFriend = model.NameFriend;
                    user.Email = model.Email;
                    user.Phone = model.Phone;
                    user.PasswordUser = String.IsNullOrEmpty(model.PasswordUser) ? Utils.GenerateAleatPass() : model.PasswordUser;
                    user.Active = model.Active;

                    dBContext.TbUser.Add(user);
                    dBContext.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    dBContext.SaveChanges();
                }
                else
                {
                    dBContext.TbUser.Add(model);
                    dBContext.SaveChanges();
                }
                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Usuario guardado correctamente"
                });
            }
            catch (Exception ex)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "Ocurrio un error al crear el usuario, intentelo nuevamente"
                });
            }
        }
        [HttpGet]
        public JsonResult GetUsers()
        {
            var users = dBContext.TbUser.Where(x => x.TypeUser == 2).ToList();

            return Json(users, JsonRequestBehavior.AllowGet);
        }
  
        [HttpGet]
        public JsonResult DeleteReservationByUser(int userid)
        {
            try
            {
                var reservation = dBContext.TbReservation.Where(x => x.TbUserId == userid).ToList();

                foreach (var item in reservation)
                {
                    var chair = dBContext.TbChair.Where(x => x.Id == item.TbChair).FirstOrDefault();
                    chair.IsOcupate = false;
                    chair.DateReservation = null;
                    dBContext.TbChair.Add(chair);
                    dBContext.Entry(chair).State = System.Data.Entity.EntityState.Modified;
                    dBContext.SaveChanges();
                }

                dBContext.TbReservation.RemoveRange(reservation);
                dBContext.SaveChanges();
                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Reserva eliminada correctamente"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se pudo eliminar la reserva"
                }, JsonRequestBehavior.AllowGet);
            }
   
        }

        [HttpGet]
        public JsonResult DeleteReservationByChairId(int chairid)
        {
            try
            {
                var reservation = dBContext.TbReservation.Where(x => x.TbChair == chairid).ToList();
                var chair = dBContext.TbChair.Where(x => x.Id == chairid).FirstOrDefault();
                chair.IsOcupate = false;
                chair.DateReservation = null;

                dBContext.TbReservation.RemoveRange(reservation);
                dBContext.SaveChanges();
                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Reserva eliminada correctamente"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se pudo eliminar la reserva"
                }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult UploadConfig(TbConfig model)
        {
            try {
                var config = dBContext.TbConfig.Where(x => x.Id == 1).FirstOrDefault();
                config.BackgroundTable = model.BackgroundTable;
                  
                dBContext.TbConfig.Add(config);
                dBContext.Entry(config).State = System.Data.Entity.EntityState.Modified;
                dBContext.SaveChanges();

                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Imagen cargada correctamente"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (System.Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se puede cargar la imagen"
                }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public JsonResult TextConfig(TbConfig model)
        {
            try
            {
                var config = dBContext.TbConfig.Where(x => x.Id == 1).FirstOrDefault();
                config.txtCredential = model.txtCredential;
                config.txtConfirm = model.txtConfirm;

                dBContext.TbConfig.Add(config);
                dBContext.Entry(config).State = System.Data.Entity.EntityState.Modified;
                dBContext.SaveChanges();

                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Textos guardados correctamente"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se puede guardar los textos"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetConfig()
        {
            var reservation = dBContext.TbConfig.Where(x => x.Id == 1).FirstOrDefault();
            if (reservation != null)
            {
                return Json(reservation, JsonRequestBehavior.AllowGet);
            }
             
                return Json("", JsonRequestBehavior.AllowGet); 
        }

         
        public ActionResult UpladoTables()
        {
            string fullpath = "";
            string filend = "";

            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    var req = Request.Browser.Browser.ToUpper();
                    string fname;

                    if (req == "IE" || req == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        string filen = Request.Files.AllKeys[0].ToString();
                        string path = Path.Combine(HttpRuntime.AppDomainAppPath, "Content\\");
                        fullpath = Path.Combine(path, filen);
                        file.SaveAs(fullpath);

                        using (var reader = new StreamReader(fullpath, Encoding.UTF8, true))
                        {
                            while (!reader.EndOfStream)
                            {
                                var line = reader.ReadLine();
                                var values = line.Split(';');

                                if (!values[0].Contains("Numero de mesa"))
                                {
                                    var table = new TbTable()
                                    {
                                        DescriptionTable = "Mesa " + values[0].ToString(),
                                        Active = true,
                                        Position = "",
                                        TotalPerson = int.Parse(values[1].ToString()),
                                        IsBlocked = false
                                         
                                    };
                                    dBContext.TbTable.Add(table);
                                    dBContext.SaveChanges();

                                    var id = table.Id;
                                    for (int i = 0; i < int.Parse(values[1]); i++)
                                    {
                                        var silla = new TbChair()
                                        {
                                            DescriptionChair = "Silla " + (1 + i).ToString(),
                                            Active = true,
                                            Position = "", 
                                            IsOcupate = false,
                                            TbTableId = id
                                        };
                                        dBContext.TbChair.Add(silla);
                                        dBContext.SaveChanges();

                                    }
                                }
                            }
                        }
                    }

                    return Json(new ResponseUtil()
                    {
                        status = true,
                        message = "Mesas cargadas correctamente"
                    });
                }
                catch(Exception e)
                {
                    return Json(new ResponseUtil()
                    {
                        status = false,
                        message = "No se pudo cargar"
                    });
                }
            

            }
            return Json(new ResponseUtil()
            {
                status = true,
                message = "Mesas cargadas correctamente"
            });
        }




        [HttpGet]
        public JsonResult DeleteAllTable()
        {
            try
            { 

                return Json("", JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult DeleteTable(int tableid)
        {
            var reservation = dBContext.TbReservation.Where(x => x.TbTableId == tableid).ToList();
            if (reservation.Count > 0)
            { 
                return Json(new ResponseUtil() {
                    status = false,
                    message = "No se puede eliminar, la mesa tiene asignada usuarios"
                },JsonRequestBehavior.AllowGet);
            }

            var table = dBContext.TbTable.Where(x => x.Id == tableid).FirstOrDefault();
            dBContext.TbTable.Remove(table);
            dBContext.SaveChanges();

            return Json(new ResponseUtil()
            {
                status = true,
                message = "Mesa eliminada correctamente"
            },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteAllUser()
        {
            try
            { 
                var users = dBContext.TbUser.Where(x => x.TypeUser == 2).ToList();

                foreach (var item in users)
                {
                    var reservations = dBContext.TbReservation.Where(x => x.TbUserId == item.Id).ToList();
                    if (reservations.Count > 0)
                    {
                        foreach (var reservation in reservations)
                        { 
                            var chair = dBContext.TbChair.Where(x => x.Id == reservation.TbChair).FirstOrDefault();
                            chair.IsOcupate = false;
                            chair.DateReservation = null;
                            dBContext.TbChair.Add(chair);
                            dBContext.Entry(chair).State = System.Data.Entity.EntityState.Modified;
                            dBContext.SaveChanges();
                        } 
                    }

                    dBContext.TbReservation.RemoveRange(reservations);
                    dBContext.SaveChanges(); 
                }

                dBContext.TbUser.RemoveRange(users);
                dBContext.SaveChanges();


                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Usuarios eliminados correctamente"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se eliminaron los usuarios"
                }, JsonRequestBehavior.AllowGet);
            }
             
        }

        [HttpGet]
        public JsonResult DeleteUser(int userid)
        {
            var reservation = dBContext.TbReservation.Where(x => x.TbUserId == userid).ToList();
            if (reservation.Count > 0)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se puede eliminar, el usuario tiene una mesa asignada"
                }, JsonRequestBehavior.AllowGet);
            }

            var user = dBContext.TbUser.Where(x => x.Id == userid).FirstOrDefault();
            dBContext.TbUser.Remove(user);
            dBContext.SaveChanges();

            return Json(new ResponseUtil()
            {
                status = true,
                message = "Usuario eliminado correctamente"
            }, JsonRequestBehavior.AllowGet);
        }




        [HttpPost]
        public JsonResult SaveReservation(List<TbReservation> model)
        {
            try
            {
                var userid = model[0].TbUserId;
                var table = model[0].TbTableId;
                var localizaterese = dBContext.TbReservation.Where(x => x.TbUserId == userid).ToList();
                var userdata = dBContext.TbUser.Where(x => x.Id == userid).FirstOrDefault();
                var type = "";

                if (userdata.TypeUser == 2)
                {
                    if (localizaterese.Count > 0)
                    {
                        var list = localizaterese.ToList();
                        foreach (var item in list)
                        {
                            var chair = dBContext.TbChair.Where(x => x.Id == item.TbChair).FirstOrDefault();
                            chair.IsOcupate = false;
                            chair.DateReservation = null;
                            dBContext.TbChair.Add(chair);
                            dBContext.Entry(chair).State = System.Data.Entity.EntityState.Modified;
                            dBContext.SaveChanges();

                        }
                        dBContext.TbReservation.RemoveRange(localizaterese);
                          
                        dBContext.SaveChanges();
                        type = " editada ";

                    }
                }
                 
                dBContext.TbReservation.AddRange(model);
                dBContext.SaveChanges();

                var tableDetail = dBContext.TbTable.Where(x => x.Id == table).FirstOrDefault();
                ResponseUtil util = new ResponseUtil();

                var config = dBContext.TbConfig.Find(1);

                var emailhtml = config.txtConfirm;
                emailhtml = emailhtml.Replace("{mesa}", type + tableDetail.DescriptionTable);

                var email = "Reserva : " + type + tableDetail.DescriptionTable + "  ";
                var num = 1;
                foreach (var item in model)
                {
                    var chair = dBContext.TbChair.Where(x => x.Id == item.TbChair).FirstOrDefault();
                    var user = dBContext.TbUser.Where(x => x.Id == item.TbUserId).FirstOrDefault();
                    //email += "</br> " + chair.DescriptionChair + "";
                    if (num == 1)
                    {
                        emailhtml = emailhtml.Replace("{silla " + num + "}",chair.DescriptionChair + " : D/Dª : " + user.NameUser);
                    
                    }
                    else
                    {
                        emailhtml = emailhtml.Replace("{silla " + num + "}", chair.DescriptionChair + ": Acompañante D/Dª : " + user.NameUser);
                    }
                    emailhtml= emailhtml.Replace("{observaciones " + num +"}",  item.Observation);
                    num++;
                } 

                sendMailAmazon(userdata.Email, emailhtml, "Confirmación de reserva");

                return Json(new ResponseUtil()
                    {
                        status = true,
                        message = "Reserva guardada correctamente"
                    }, JsonRequestBehavior.AllowGet); 
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se puede realizar la reserva"
                }, JsonRequestBehavior.AllowGet);
            } 
        }

 

        [HttpGet]
        public JsonResult GetChair(int tableid)
        {
            var chairs = dBContext.TbChair.Where(x => x.TbTableId == tableid).ToList();
            return Json(chairs, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPersonByDNI(string dni)
        {
            var person = dBContext.TbUser.Where(x => x.DNI == dni).ToList();
            return Json(person, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetTablesReservation()
        {
            var tables = dBContext.TbTable.ToList();
            var reservations = dBContext.TbReservation.ToList();
            var tablesList = new List<TbTable>();

            foreach (var item in tables)
            {
                var res = reservations.Where(x => x.TbTableId == item.Id).ToList();
                var table = tables.Where(x => x.Id == item.Id).FirstOrDefault();
                if (res.Count > 0)
                {
                    if (table.TotalPerson != res.Count)  
                        tablesList.Add(item); 
                }
                else 
                    tablesList.Add(item); 

            }

            return Json(tablesList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChairsReservation(int tableid)
        {
            var chairs = dBContext.TbChair.Where(x => x.TbTableId == tableid).ToList();
            var reservations = dBContext.TbReservation.ToList();
            var chairList = new List<TbChair>();

            foreach (var item in chairs)
            {
                var res = reservations.Where(x => x.TbChair == item.Id).ToList(); 
                if (res.Count == 0)
                    chairList.Add(item);
                else
                {
                    item.IsOcupate = true;
                    chairList.Add(item); 
                }

            }

            return Json(chairList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUsersReservation()
        {
            var users = dBContext.TbUser.Where(x => x.TypeUser == 2).ToList();
            var reservations = dBContext.TbReservation.ToList();
            var userList = new List<TbUser>();

            foreach (var item in users)
            {
                var res = reservations.Where(x => x.TbUserId == item.Id).ToList();
                if (res.Count == 0)
                    userList.Add(item);

            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUsersReservationWhereName(string name)
        {
            var users = dBContext.TbUser.Where(x => x.TypeUser == 2 && x.NameUser.Contains(name)).ToList();
            var reservations = dBContext.TbReservation.ToList();
            var userList = new List<TbUser>();

            foreach (var item in users)
            {
                var res = reservations.Where(x => x.TbUserId == item.Id).ToList();
                if (res.Count == 0)
                    userList.Add(item);

            }

            return Json(userList, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetChairTablesAndUser()
         {
            var tables = dBContext.TbTable.ToList(); 
            var reservations = new List<ResponseReservation>();
            foreach (var item in tables)
            {
                var reservationList = dBContext.TbReservation.Where(x => x.TbTableId == item.Id).ToList();
           
                var model = new ResponseReservation()
                {
                    Id = item.Id,
                    ChairDisponible = item.TotalPerson - reservationList.Count,
                    ChairOcupate = reservationList.Count,
                    Mesa = item.DescriptionTable,
                    IsComplete = (item.TotalPerson - reservationList.Count == 0) ? true : false
                };
                reservations.Add(model);
            }

            return Json(reservations, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DeleteAssignChair(int chairid)
        {  
            try
            {
                var reservation = dBContext.TbReservation.Where(x => x.TbChair == chairid).FirstOrDefault();
                if (reservation != null) {
                    dBContext.TbReservation.Remove(reservation);
                    dBContext.SaveChanges();
                }
                var chair = dBContext.TbChair.Where(x => x.Id == chairid).FirstOrDefault();
                 
                chair.IsOcupate = false;
                chair.DateReservation = null;
                dBContext.TbChair.Add(chair);
                dBContext.Entry(chair).State = System.Data.Entity.EntityState.Modified;
                dBContext.SaveChanges();

                return Json(new ResponseUtil()
                {
                    status = true,
                    message = "Silla habilitada correctamente"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "No se puede realizar la acción"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetDetailReport()
        {
            var tables = dBContext.TbTable.ToList();

            var reservations = new List<ResponseTableDetail>();
            foreach (var item in tables)
            {
                var chairs = dBContext.TbChair.Where(x => x.TbTableId == item.Id).ToList();

                foreach (var chair in chairs)
                {
                    var reservation = dBContext.TbReservation.Where(x => x.TbChair == chair.Id).FirstOrDefault();

                    if (reservation != null)
                    {
                        var user = dBContext.TbUser.Where(x => x.Id == reservation.TbUserId).FirstOrDefault();

                        var model = new ResponseTableDetail()
                        {
                            MesaName = item.DescriptionTable,
                            ChairId = reservation.TbChair,
                            ChairName = chair.DescriptionChair,
                            UserId = reservation.TbUserId,
                            NameUser = user.NameUser, 
                            Observation = reservation.Observation
                        };
                        reservations.Add(model);
                    } 

                }
            }
             
            return Json(reservations, JsonRequestBehavior.AllowGet);
        }

        public ResponseUtil sendMailAmazon(string email , string bodyMail ,string subject)
        {
            string configSet = "mailSet";
            string senderAddress = "registro@customevent.es";

            List<string> receiverAddress = new List<string>();
            receiverAddress.Add(email);
             
            var htmlBody = bodyMail;
            using (var client = new AmazonSimpleEmailServiceClient(
   new BasicAWSCredentials("AKIAXUJYTLZWWZMKQIOD", "NIeobeYzkeFV94trisdIHR+oIzksluyMl14GlS6i"), RegionEndpoint.USEast2))
            {
                var sendRequest = new SendEmailRequest
                {
                    Source = senderAddress,
                    Destination = new Destination
                    {
                        ToAddresses = receiverAddress,
                    },
                    Message = new Message
                    {
                        Subject = new Amazon.SimpleEmail.Model.Content(subject),
                        Body = new Body
                        {
                            Html = new Amazon.SimpleEmail.Model.Content { Charset = "UTF-8", Data = htmlBody },
                            Text = new Amazon.SimpleEmail.Model.Content { Charset = "UTF-8", Data = bodyMail }
                        }
                    },
                    ConfigurationSetName = configSet
                };
                try
                {

                    var response = client.SendEmail(sendRequest);
                    response.MessageId.ToString();
                    return new ResponseUtil()
                     {
                         status = true,
                         message = "Mail enviado correctamente"
                     };
                }
                catch (Exception ex)
                {
                    Console.WriteLine("The email was not sent.");
                    Console.WriteLine("Error message: " + ex.Message);

                    return new ResponseUtil()
                    {
                        status = false,
                        message = "Ocurrio un error, inténtelo nuevamente"
                    };

                }
            };
        }

        [HttpGet]
        public JsonResult GetDetailTablesAndUser(int tableid)
        {
            var chairs = dBContext.TbChair.Where(x => x.TbTableId == tableid).ToList();
            var reservations = new List<ResponseTableDetail>();
            foreach (var item in chairs)
            {
                var reservation = dBContext.TbReservation.Where(x => x.TbChair == item.Id).FirstOrDefault();
              
                if (reservation != null)
                {
                    var user = dBContext.TbUser.Where(x => x.Id == reservation.TbUserId).FirstOrDefault();
                    var table = dBContext.TbTable.Where(x => x.Id == reservation.TbTableId).FirstOrDefault();

                    var model = new ResponseTableDetail()
                    {
                        MesaId = tableid,
                        MesaName = table.DescriptionTable,
                        ChairId = reservation.TbChair,
                        ChairName = item.DescriptionChair,
                        UserId = reservation.TbUserId,
                        NameUser = user.NameUser,
                        IsOcupate = true,
                        Observation = reservation.Observation
                    };
                    reservations.Add(model);
                }
                else
                {
                    var table = dBContext.TbTable.Where(x => x.Id == tableid).FirstOrDefault();

                    var model = new ResponseTableDetail()
                    {
                        MesaId = tableid,
                        ChairId = item.Id,
                        MesaName = table.DescriptionTable,
                        ChairName = item.DescriptionChair,
                        UserId = 0,
                        NameUser = "",
                        IsOcupate = false,
                        Observation = (reservation == null) ? "" :  reservation.Observation

                    };
                    reservations.Add(model);
                }
               
            }

            return Json(reservations, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetInfoUserReservation(int userid)
        {
            var reservations = dBContext.TbReservation.Where(x => x.TbUserId == userid).ToList();

            var listReservation = new List<ResponseTableDetail>();
            foreach (var item in reservations)
            {
                var table = dBContext.TbTable.Where(x => x.Id == item.TbTableId).FirstOrDefault();

                var model = new ResponseTableDetail()
                {
                    MesaId = table.Id,
                    MesaName = table.DescriptionTable,
                    ChairId = item.TbChair,
                    ChairName = dBContext.TbChair.Where(x => x.Id == item.TbChair).FirstOrDefault().DescriptionChair,
                    NameUser = dBContext.TbUser.Where(x => x.Id == item.TbUserId).FirstOrDefault().NameUser,
                    Observation = String.IsNullOrEmpty(item.Observation) ? "-" : item.Observation
                    
                };
                listReservation.Add(model);
            }

            return Json(listReservation, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetLoginUserPassAdmin(string login, string password)
        {
            try
            {
                var user = dBContext.TbUser.Where(x => x.UserName == login && x.PasswordUser == password && x.TypeUser == 1).FirstOrDefault();

                if (user != null)
                {
                    Session["admin"] = user;
                    return Json(new ResponseUtil()
                    {
                        status = true,
                        body = user,
                        message = "Bienvenido"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResponseUtil()
                    {
                        status = false,
                        message = "El usuario no se encuentra registrado en el sistema"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "Ocurrio un error, inténtelo nuevamente"
                }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetLoginUserPass(string login, string password)
        {
            try
            {
                var user = dBContext.TbUser.Where(x => x.UserName == login && x.PasswordUser == password && x.TypeUser == 2).FirstOrDefault();

                if (user != null) {
                    Session["user"] = user;
                    return Json(new ResponseUtil()
                    {
                        status = true,
                        body = user,
                        message = "Bienvenido"
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new ResponseUtil()
                    {
                        status = false,
                        message = "El usuario no se encuentra registrado en el sistema"
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new ResponseUtil()
                {
                    status = false,
                    message = "Ocurrio un error, inténtelo nuevamente"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UploadCenso()
        {
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        HttpPostedFileBase file = files[i];
                        string fname;

                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                            fname = testfiles[testfiles.Length - 1];
                        }
                        else
                            fname = file.FileName;

                        var newName = fname.Split('.');
                        fname = newName[0] + "_" + DateTime.Now.Ticks.ToString() + "." + newName[1];
                        var uploadRootFolderInput = AppDomain.CurrentDomain.BaseDirectory + "\\ExcelUploads";
                        Directory.CreateDirectory(uploadRootFolderInput);
                        var directoryFullPathInput = uploadRootFolderInput;
                        fname = Path.Combine(directoryFullPathInput, fname);
                        file.SaveAs(fname);
                        string xlsFile = fname;
                         
                        var response = ReadExcelUserVoteFOnline(xlsFile);

                        dBContext.TbUser.AddRange(response);
                        dBContext.SaveChanges();
                    }
                }
                catch (Exception)
                {
                    return Json("Error de carga");

                }
            }
            return Json("Correcto");
        }

        public int GenerateAletario()
        {
            var guid = Guid.NewGuid();
            var justNumbers = new String(guid.ToString().Where(Char.IsDigit).ToArray());
            var seed = int.Parse(justNumbers.Substring(0, 4));

            var random = new Random(seed);
            var value = random.Next(0, 5);
            return value + seed;
        }

        public List<TbUser> ReadExcelUserVoteFOnline(string filepath)
        {
            var listUser = new List<TbUser>();
            FileInfo existingFile = new FileInfo(filepath);
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                int rowCount = worksheet.Dimension.End.Row;

                string[] excelHeader = new string[7] { "NOMBRE", "DNI", "LOGIN", "ACOMPAÑANTE", "EMAIL", "TELEFONO", "CONTRASEÑA"};

                for (int row = 1; row <= excelHeader.Length; row++)
                {
                    string header = worksheet.Cells[1, row].Value.ToString().ToUpper().Trim();
                    var headcsv = header.Replace("\r\n", "");

                    if (headcsv == excelHeader[row - 1]) { }
                    else
                    {
                        row = excelHeader.Length;
                        return listUser;
                    }
                }
                for (int i = 2; i <= rowCount; i++)
                {
                    if (worksheet.Cells[i, 1].Value != null)
                    {
                        var user = new TbUser()
                        {
                            NameUser = worksheet.Cells[i, 1].Value.ToString(),
                            DNI = worksheet.Cells[i, 2].Value.ToString(),
                            UserName = worksheet.Cells[i, 3].Value.ToString(),
                            NameFriend = worksheet.Cells[i, 4].Value == null ? "" : worksheet.Cells[i, 4].Value.ToString(),
                            Email = worksheet.Cells[i, 5].Value.ToString(),
                            Phone = worksheet.Cells[i, 6].Value.ToString(),
                            PasswordUser = worksheet.Cells[i, 7].Value == null ? "M"+ GenerateAletario() : worksheet.Cells[i, 7].Value.ToString(),
                            TypeUser = 2,
                            Active = true
                        }; 
                        listUser.Add(user);
                    }
                }
            }
            return listUser;
        }






        ///

    }
}