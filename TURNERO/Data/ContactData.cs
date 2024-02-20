using System.Data.SqlClient;
using System.Data;
using TURNERO.Data;
using TURNERO.Models;

namespace TURNERO.Data
{
    public class ContactData
    {
        public List<ContactModel> List()
        {
            var ol = new List<ContactModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spContactList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new ContactModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            name = Convert.ToString(dr["name"]),
                            phone = Convert.ToString(dr["phone"]),
                            mail = Convert.ToString(dr["mail"])
                        });
                    }
                }
            }
            return ol;
        }
        public ContactModel Read(int id)
        {
            var oc = new ContactModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spContactRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.name = Convert.ToString(dr["name"]);
                        oc.phone = Convert.ToString(dr["phone"]);
                        oc.mail = Convert.ToString(dr["mail"]);
                    }
                }
            }
            return oc;
        }
        public bool Create(ContactModel oc)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spContactCreate", connection);
                    cmd.Parameters.AddWithValue("name", oc.name);
                    cmd.Parameters.AddWithValue("phone", oc.phone);
                    cmd.Parameters.AddWithValue("mail", oc.mail);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                res = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
                res = false;
            }
            return res;
        }
        public bool Update(ContactModel oc)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spContactUpdate", connection);
                    cmd.Parameters.AddWithValue("id", oc.id);
                    cmd.Parameters.AddWithValue("name", oc.name);
                    cmd.Parameters.AddWithValue("phone", oc.phone);
                    cmd.Parameters.AddWithValue("mail", oc.mail);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                res = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
                res = false;
            }
            return res;
        }
        public bool Delete(int id)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spContactDelete", connection);
                    cmd.Parameters.AddWithValue("id", id);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.ExecuteNonQuery();
                }
                res = true;
            }
            catch (Exception e)
            {
                string error = e.Message;
                res = false;
            }
            return res;
        }
    }
}
