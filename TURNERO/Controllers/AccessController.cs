using Microsoft.AspNetCore.Mvc;
using TURNERO.Helpers;
using TURNERO.Models;
using TURNERO.Data;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using NuGet.Common;

namespace TURNERO.Controllers
{
    public class AccessController : Controller
    {
        //LOGIN
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string user, string pass, int type)
        {
            string sp = type == 1 ? "spAdminValidate" : "spProviderValidate";

            UserModel ou = new UserModel();
            ou.user = user;
            ou.pass = HelperText.Sha256(pass);
            ou.type = type; 

            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand(sp, connection);
                cmd.Parameters.AddWithValue("user", ou.user);
                cmd.Parameters.AddWithValue("pass", ou.pass);
                cmd.CommandType = CommandType.StoredProcedure;
                ou.id = Convert.ToInt32(cmd.ExecuteScalar().ToString());
            }
            if(ou.id != 0)
            {
                HttpContext.Session.SetString("user", JsonConvert.SerializeObject(ou));
                /*string userJson = HttpContext.Session.GetString("user");
                                if (userJson != null)
                {
                    Deserializar la cadena JSON a un objeto UserModel
                    UserModel user = JsonConvert.DeserializeObject<UserModel>(userJson);
                }*/
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ViewData["error"] = "A00001 - Autentificación erronea";
                return View();
            }

        }
        //CHANGE PASS
        [HttpGet]
        public IActionResult ChangePass(string token, int type)
        {
            if (string.IsNullOrEmpty(token) || type == 0)
            {
                return RedirectToAction("Login", "Access");
            }

            try
            {
                string spSelect = type == 1 ? "SELECT id FROM admins WHERE token = @token" : "SELECT id FROM providers WHERE token = @token";
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand selectCmd = new SqlCommand(spSelect, connection);
                    selectCmd.Parameters.AddWithValue("@token", token);

                    object id = selectCmd.ExecuteScalar();
                    if (id != null && id != DBNull.Value)
                    {
                        ViewBag.Id = (int)id;
                    }
                    else
                    {
                        return RedirectToAction("Login", "Access");
                    }
                }

                ViewBag.Token = token;
                ViewBag.Type = type;
                return View();
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Access");
            }
        }
        [HttpPost]
        public IActionResult ChangePass(int type, int id, string token, string pass, string repass)
        {
            ViewBag.Id = id;
            ViewBag.Token = token;
            ViewBag.Type = type;

            if (pass == repass)
            {
                pass = HelperText.Sha256(pass);
            }
            else
            {
                ViewData["error"] = "A00002 - Claves no coincidentes";
                return View();
            }

            try
            {
                string sp = type == 1 ? "spAdminPassUpdate" : "spProviderPassUpdate";
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand(sp, connection);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.Parameters.AddWithValue("pass", pass);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }

                ViewData["success"] = "El cambio de clave fue efectuado correctamente.";
                return View("Login", "Access");
            }
            catch (Exception e)
            {
                string error = e.Message;
                ViewData["error"] = "A00006 - Cambio de clave no efectuado";
                return View();
            }
        }
        //LOGOFF
        [HttpGet]
        public IActionResult Logoff()
        {
            HttpContext.Session.Remove("user");
            return RedirectToAction("Login", "Access");
        }
    }
}
