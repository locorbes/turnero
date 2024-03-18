using System.Data.SqlClient;
using System.Data;
using TURNERO.Helpers;
using TURNERO.Models;

namespace TURNERO.Data
{
    public class AdminData
    {
        public List<AdminModel> List()
        {
            var ol = new List<AdminModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spAdminList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new AdminModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            name = Convert.ToString(dr["name"]),
                            surname = Convert.ToString(dr["surname"]),
                            mail = Convert.ToString(dr["mail"]),
                            region_id = Convert.ToInt32(dr["region_id"]),
                            status = Convert.ToInt32(dr["status"]),
                            user_config = Convert.ToBoolean(Convert.ToInt32(dr["user_config"])),
                            provider_config = Convert.ToBoolean(Convert.ToInt32(dr["provider_config"])),
                            branch_config = Convert.ToBoolean(Convert.ToInt32(dr["branch_config"])),
                        });
                    }
                }
            }
            return ol;
        }
        public AdminModel Read(int id)
        {
            var oc = new AdminModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spAdminRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.name = Convert.ToString(dr["name"]);
                        oc.surname = Convert.ToString(dr["surname"]);
                        oc.mail = Convert.ToString(dr["mail"]);
                        oc.region_id = Convert.ToInt32(dr["region_id"]);
                        oc.status = Convert.ToInt32(dr["status"]);
                        oc.user_config = Convert.ToBoolean(Convert.ToInt32(dr["user_config"]));
                        oc.provider_config = Convert.ToBoolean(Convert.ToInt32(dr["provider_config"]));
                        oc.branch_config = Convert.ToBoolean(Convert.ToInt32(dr["branch_config"]));
                    }
                }
            }
            return oc;
        }
        public bool Create(AdminModel oc)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spAdminCreate", connection);
                    cmd.Parameters.AddWithValue("user", oc.user);
                    cmd.Parameters.AddWithValue("mail", oc.mail);
                    cmd.Parameters.AddWithValue("pass", HelperText.Sha256(oc.user));
                    cmd.Parameters.AddWithValue("name", oc.name);
                    cmd.Parameters.AddWithValue("surname", oc.surname);
                    cmd.Parameters.AddWithValue("user_config", oc.user_config);
                    cmd.Parameters.AddWithValue("provider_config", oc.provider_config);
                    cmd.Parameters.AddWithValue("branch_config", oc.branch_config);
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
        public bool Update(AdminModel oc)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spAdminUpdate", connection);
                    cmd.Parameters.AddWithValue("id", oc.id);
                    cmd.Parameters.AddWithValue("user", oc.user);
                    cmd.Parameters.AddWithValue("mail", oc.mail);
                    cmd.Parameters.AddWithValue("name", oc.name);
                    cmd.Parameters.AddWithValue("surname", oc.surname);
                    cmd.Parameters.AddWithValue("user_config", oc.user_config);
                    cmd.Parameters.AddWithValue("provider_config", oc.provider_config);
                    cmd.Parameters.AddWithValue("branch_config", oc.branch_config);
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
                    SqlCommand cmd = new SqlCommand("spAdminDelete", connection);
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
