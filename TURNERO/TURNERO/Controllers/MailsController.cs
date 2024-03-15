using Microsoft.AspNetCore.Mvc;
using NuGet.Common;
using System.Data.SqlClient;
using System.Data;
using TURNERO.Helpers;
using TURNERO.Data;


namespace TURNERO.Controllers
{
    public class MailsController : Controller
    {
        private HelperMail helpermail;

        public MailsController(HelperMail helpermail)
        {
            this.helpermail = helpermail;
        }
        private string Token()
        {
            string pattern = "1234567890abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            char[] keyArray = new char[20];
            for (int i = 0; i < 20; i++)
            {
                int randomIndex = random.Next(0, pattern.Length);
                keyArray[i] = pattern[randomIndex];
            }
            string token = new string(keyArray);
            return token;
        }
        [HttpGet]
        public IActionResult Recover()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Recover(int type, string mail)
        {
            string token;

            try
            {
                string spSelect = type == 1 ? "SELECT COUNT(*) FROM users WHERE mail = @mail" : "SELECT COUNT(*) FROM providers WHERE commercial_mail = @mail";
                string spUpdate = type == 1 ? "spUserTokenUpdate" : "spProviderTokenUpdate";
                var conn = new Connection();

                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand selectCmd = new SqlCommand(spSelect, connection);
                    selectCmd.Parameters.AddWithValue("mail", mail);

                    int count = (int)selectCmd.ExecuteScalar();

                    if (count > 0)
                    {
                        token = Token();

                        SqlCommand updateCmd = new SqlCommand(spUpdate, connection);
                        updateCmd.Parameters.AddWithValue("mail", mail);
                        updateCmd.Parameters.AddWithValue("token", token);
                        updateCmd.CommandType = CommandType.StoredProcedure;
                        updateCmd.ExecuteNonQuery();
                    }
                    else
                    {
                        ViewData["error"] = "A00003 - Mail no registrado";
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                string error = e.Message;
                ViewData["error"] = "A00004 - Token no generado";
                return View();
            }

            string scheme = Request.Scheme;
            string host = Request.Host.Value; 
            string path = $"{scheme}://{host}{Request.PathBase}";

            string subject = "TURNERO - Nueva Clave";
            string message = "Se realizo una solicitud para recuperar su clave. Si fue usted, haga <a href='"+path+"/Access/ChangePass?token="+token+"&type="+type.ToString()+"'>click aquí</a> para proceder a la pantalla de cambio de clave.";
            try
            {
                helpermail.SendMail(mail, subject, message);
                ViewData["success"] = "Mail enviado";
                return View();

            }
            catch (Exception e)
            {
                string error = e.Message;
                ViewData["error"] = "A00005 - Mail no enviado";
                return View();

            }
        }
    }
}
