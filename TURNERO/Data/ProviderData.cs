using System.Data.SqlClient;
using System.Data;
using TURNERO.Helpers;
using TURNERO.Models;
using System.Net;
using System.Xml.Linq;

namespace TURNERO.Data
{
    public class ProviderData
    {
        public List<ProviderModel> List()
        {
            var ol = new List<ProviderModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spProviderList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new ProviderModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            code = Convert.ToInt32(dr["code"] ?? 0),
                            cuit = Convert.ToString(dr["cuit"]),
                            business_name = Convert.ToString(dr["business_name"]),
                            name = Convert.ToString(dr["name"]),
                            commercial_mail = Convert.ToString(dr["commercial_mail"]),
                            it_mail = Convert.ToString(dr["it_mail"]),
                            region_id = Convert.ToInt32(dr["region_id"] ?? 0),
                            origin = Convert.ToString(dr["origin"]),
                            address = Convert.ToString(dr["address"]),
                            observations = Convert.ToString(dr["observations"]),
                            fc_required = Convert.ToInt32(dr["fc_required"] ?? 0),
                        });
                    }
                }
            }
            return ol;
        }
        public ProviderModel Read(int id)
        {
            var oc = new ProviderModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spProviderRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.code = Convert.ToInt32(dr["code"] ?? 0);
                        oc.cuit = Convert.ToString(dr["cuit"]);
                        oc.business_name = Convert.ToString(dr["business_name"]);
                        oc.name = Convert.ToString(dr["name"]);
                        oc.commercial_mail = Convert.ToString(dr["commercial_mail"]);
                        oc.it_mail = Convert.ToString(dr["it_mail"]);
                        oc.region_id = Convert.ToInt32(dr["region_id"] ?? 0);
                        oc.origin = Convert.ToString(dr["origin"]);
                        oc.address = Convert.ToString(dr["address"]);
                        oc.observations = Convert.ToString(dr["observations"]);
                        oc.fc_required = Convert.ToInt32(dr["fc_required"] ?? 0);
                    }
                }
            }
            return oc;
        }
        public bool Create(ProviderModel oc, int created_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spProviderCreate", connection);
                    cmd.Parameters.AddWithValue("@cuit", oc.cuit);
                    cmd.Parameters.AddWithValue("@commercial_mail", oc.@commercial_mail);
                    cmd.Parameters.AddWithValue("@pass", HelperText.Sha256(oc.cuit));
                    cmd.Parameters.AddWithValue("@region_id", oc.region_id);
                    cmd.Parameters.AddWithValue("@code", oc.code);
                    cmd.Parameters.AddWithValue("@business_name", oc.business_name);
                    cmd.Parameters.AddWithValue("@name", oc.name);
                    cmd.Parameters.AddWithValue("@it_mail", oc.it_mail);
                    cmd.Parameters.AddWithValue("@origin", oc.origin);
                    cmd.Parameters.AddWithValue("@address", oc.address);
                    cmd.Parameters.AddWithValue("@observations", oc.observations);
                    cmd.Parameters.AddWithValue("@fc_required", oc.fc_required);
                    cmd.Parameters.AddWithValue("@created_by", created_by);
                    SqlParameter status = new SqlParameter("@status", SqlDbType.Bit);
                    status.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(status);
                    SqlParameter message = new SqlParameter("@message", SqlDbType.VarChar, 100);
                    message.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(message);
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
        public bool Update(ProviderModel oc, int updated_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spProviderUpdate", connection);
                    cmd.Parameters.AddWithValue("@id", oc.id);
                    cmd.Parameters.AddWithValue("@cuit", oc.cuit);
                    cmd.Parameters.AddWithValue("@commercial_mail", oc.commercial_mail);
                    cmd.Parameters.AddWithValue("@region_id", oc.region_id);
                    cmd.Parameters.AddWithValue("@code", oc.code);
                    cmd.Parameters.AddWithValue("@business_name", oc.business_name);
                    cmd.Parameters.AddWithValue("@name", oc.name);
                    cmd.Parameters.AddWithValue("@it_mail", oc.it_mail);
                    cmd.Parameters.AddWithValue("@origin", oc.origin);
                    cmd.Parameters.AddWithValue("@address", oc.address);
                    cmd.Parameters.AddWithValue("@observations", oc.observations);
                    cmd.Parameters.AddWithValue("@fc_required", oc.fc_required);
                    cmd.Parameters.AddWithValue("@updated_by", updated_by);
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
                    SqlCommand cmd = new SqlCommand("spProviderDelete", connection);
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
