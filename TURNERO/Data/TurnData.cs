using Microsoft.CodeAnalysis.Operations;
using System.Data;
using System.Data.SqlClient;
using TURNERO.Models;

namespace TURNERO.Data
{
    public class TurnData
    {
        public List<TurnModel> List()
        {
            var ol = new List<TurnModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spTurnList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new TurnModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            provider_id = Convert.ToInt32(dr["provider_id"]),
                            branch_id = Convert.ToInt32(dr["branch_id"]),
                            time = Convert.ToDateTime(dr["time"]),
                            entry_time = Convert.ToDateTime(dr["entry_time"]),
                            absent = Convert.ToBoolean(dr["absent"]),
                            status = Convert.ToInt32(dr["status"]),
                            observations = Convert.ToString(dr["observations"]),
                        });
                    }
                }
            }
            return ol;
        }
        public TurnModel Read(int id)
        {
            var oc = new TurnModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spTurnRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.provider_id = Convert.ToInt32(dr["provider_id"]);
                        oc.branch_id = Convert.ToInt32(dr["branch_id"]);
                        oc.time = Convert.ToDateTime(dr["time"]);
                        oc.entry_time = Convert.ToDateTime(dr["entry_time"]);
                        oc.absent = Convert.ToBoolean(dr["absent"]);
                        oc.status = Convert.ToInt32(dr["status"]);
                        oc.observations = Convert.ToString(dr["observations"]);
                    }
                }
            }
            return oc;
        }
        public bool Create(TurnModel oc, int created_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spTurnCreate", connection);
                    cmd.Parameters.AddWithValue("@provider_id", oc.provider_id);
                    cmd.Parameters.AddWithValue("@branch_id", oc.branch_id);
                    cmd.Parameters.AddWithValue("@time", oc.time);
                    cmd.Parameters.AddWithValue("@entry_time", oc.entry_time);
                    cmd.Parameters.AddWithValue("@absent", oc.absent);
                    cmd.Parameters.AddWithValue("@observations", oc.observations);
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
        public bool Update(TurnModel oc, int updated_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spTurnUpdate", connection);
                    cmd.Parameters.AddWithValue("@id", oc.id);
                    cmd.Parameters.AddWithValue("@provider_id", oc.provider_id);
                    cmd.Parameters.AddWithValue("@branch_id", oc.branch_id);
                    cmd.Parameters.AddWithValue("@time", oc.time);
                    cmd.Parameters.AddWithValue("@entry_time", oc.entry_time);
                    cmd.Parameters.AddWithValue("@absent", oc.absent);
                    cmd.Parameters.AddWithValue("@observations", oc.observations);
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
                    SqlCommand cmd = new SqlCommand("spTurnDelete", connection);
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
