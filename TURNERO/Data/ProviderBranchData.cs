using System.Data.SqlClient;
using System.Data;
using TURNERO.Helpers;
using TURNERO.Models;
using System.Net;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Operations;

namespace TURNERO.Data
{
    public class ProviderBranchData
    {
        public List<ProviderBranchModel> List()
        {
            var ol = new List<ProviderBranchModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spProviderBranchList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new ProviderBranchModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            admin_id = Convert.ToInt32(dr["admin_id"]),
                            branch_id = Convert.ToInt32(dr["branch_id"]),
                            profile_id = Convert.ToInt32(dr["profile_id"]),
                        });
                    }
                }
            }
            return ol;
        }
        public ProviderBranchModel Read(int id)
        {
            var oc = new ProviderBranchModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spProviderBranchRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.admin_id = Convert.ToInt32(dr["admin_id"]);
                        oc.branch_id = Convert.ToInt32(dr["branch_id"]);
                        oc.profile_id = Convert.ToInt32(dr["profile_id"]);
                    }
                }
            }
            return oc;
        }
        public bool Create(ProviderBranchModel oc, int created_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spProviderBranchCreate", connection);
                    cmd.Parameters.AddWithValue("@admin_id", oc.admin_id);
                    cmd.Parameters.AddWithValue("@branch_id", oc.branch_id);
                    cmd.Parameters.AddWithValue("@profile_id", oc.profile_id);
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
        public bool Update(ProviderBranchModel oc, int updated_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spProviderBranchUpdate", connection);
                    cmd.Parameters.AddWithValue("@admin_id", oc.admin_id);
                    cmd.Parameters.AddWithValue("@branch_id", oc.branch_id);
                    cmd.Parameters.AddWithValue("@profile_id", oc.profile_id);
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
                    SqlCommand cmd = new SqlCommand("spProviderBranchDelete", connection);
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
