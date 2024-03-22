using System.Data.SqlClient;
using System.Data;
using TURNERO.Helpers;
using TURNERO.Models;
using System.Net;
using System.Xml.Linq;

namespace TURNERO.Data
{
    public class BranchData
    {
        public List<BranchModel> List()
        {
            var ol = new List<BranchModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spBranchList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new BranchModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            name = Convert.ToString(dr["name"]),
                            code = Convert.ToString(dr["code"]),
                            commercial_mail = Convert.ToString(dr["commercial_mail"]),
                            it_mail = Convert.ToString(dr["commercial_mail"]),
                            region_id = Convert.ToInt32(dr["region_id"]),
                            address = Convert.ToString(dr["address"]),
                            longitude = Convert.ToString(dr["longitude"]),
                            latitude = Convert.ToString(dr["latitude"]),
                            company_id = Convert.ToInt32(dr["company_id"]),
                        });
                    }
                }
            }
            return ol;
        }
        public BranchModel Read(int id)
        {
            var oc = new BranchModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spBranchRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.name = Convert.ToString(dr["name"]);
                        oc.code = Convert.ToString(dr["code"]);
                        oc.commercial_mail = Convert.ToString(dr["commercial_mail"]);
                        oc.it_mail = Convert.ToString(dr["commercial_mail"]);
                        oc.region_id = Convert.ToInt32(dr["region_id"]);
                        oc.address = Convert.ToString(dr["address"]);
                        oc.longitude = Convert.ToString(dr["longitude"]);
                        oc.latitude = Convert.ToString(dr["latitude"]);
                        oc.company_id = Convert.ToInt32(dr["company_id"]);
                    }
                }
            }
            return oc;
        }
        public bool Create(BranchModel oc, int created_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spBranchCreate", connection);
                    cmd.Parameters.AddWithValue("@name", oc.name);
                    cmd.Parameters.AddWithValue("@code", oc.code);
                    cmd.Parameters.AddWithValue("@commercial_mail", oc.commercial_mail);
                    cmd.Parameters.AddWithValue("@it_mail", oc.it_mail);
                    cmd.Parameters.AddWithValue("@region_id", oc.region_id);
                    cmd.Parameters.AddWithValue("@address", oc.address);
                    cmd.Parameters.AddWithValue("@longitude", oc.longitude);
                    cmd.Parameters.AddWithValue("@latitude", oc.latitude);
                    cmd.Parameters.AddWithValue("@company_id", oc.company_id);
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
        public bool Update(BranchModel oc, int updated_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spBranchUpdate", connection);
                    cmd.Parameters.AddWithValue("@id", oc.id);
                    cmd.Parameters.AddWithValue("@name", oc.name);
                    cmd.Parameters.AddWithValue("@code", oc.code);
                    cmd.Parameters.AddWithValue("@commercial_mail", oc.commercial_mail);
                    cmd.Parameters.AddWithValue("@it_mail", oc.it_mail);
                    cmd.Parameters.AddWithValue("@region_id", oc.region_id);
                    cmd.Parameters.AddWithValue("@address", oc.address);
                    cmd.Parameters.AddWithValue("@longitude", oc.longitude);
                    cmd.Parameters.AddWithValue("@latitude", oc.latitude);
                    cmd.Parameters.AddWithValue("@company_id", oc.company_id);
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
                    SqlCommand cmd = new SqlCommand("spBranchDelete", connection);
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
