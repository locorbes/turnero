using System.Data.SqlClient;
using System.Data;
using TURNERO.Helpers;
using TURNERO.Models;
using System.Net;
using System.Xml.Linq;
using Microsoft.CodeAnalysis.Operations;

namespace TURNERO.Data
{
    public class ScheduleExceptionData
    {
        public List<ScheduleExceptionModel> List()
        {
            var ol = new List<ScheduleExceptionModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spScheduleExceptionList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new ScheduleExceptionModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            branch_id = Convert.ToInt32(dr["branch_id"]),
                            day = Convert.ToInt32(dr["day"]),
                            since = (TimeSpan)dr["since"],
                            until = (TimeSpan)dr["until"],
                            turn_minutes = Convert.ToInt32(dr["turn_minutes"]),
                            turn_maximum = Convert.ToInt32(dr["turn_maximum"]),
                        });
                    }
                }
            }
            return ol;
        }
        public ScheduleExceptionModel Read(int id)
        {
            var oc = new ScheduleExceptionModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spScheduleExceptionRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.branch_id = Convert.ToInt32(dr["branch_id"]);
                        oc.day = Convert.ToInt32(dr["day"]);
                        oc.since = (TimeSpan)dr["since"];
                        oc.until = (TimeSpan)dr["until"];
                        oc.turn_minutes = Convert.ToInt32(dr["turn_minutes"]);
                        oc.turn_maximum = Convert.ToInt32(dr["turn_maximum"]);
                    }
                }
            }
            return oc;
        }
        public bool Create(ScheduleExceptionModel oc, int created_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spScheduleExceptionCreate", connection);
                    cmd.Parameters.AddWithValue("@branch_id", oc.branch_id);
                    cmd.Parameters.AddWithValue("@day", oc.day);
                    cmd.Parameters.AddWithValue("@since", oc.since);
                    cmd.Parameters.AddWithValue("@until", oc.until);
                    cmd.Parameters.AddWithValue("@turn_minutes", oc.turn_minutes);
                    cmd.Parameters.AddWithValue("@turn_maximum", oc.turn_maximum);
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
        public bool Update(ScheduleExceptionModel oc, int updated_by)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spScheduleExceptionUpdate", connection);
                    cmd.Parameters.AddWithValue("@id", oc.id);
                    cmd.Parameters.AddWithValue("@branch_id", oc.branch_id);
                    cmd.Parameters.AddWithValue("@day", oc.day);
                    cmd.Parameters.AddWithValue("@since", oc.since);
                    cmd.Parameters.AddWithValue("@until", oc.until);
                    cmd.Parameters.AddWithValue("@turn_minutes", oc.turn_minutes);
                    cmd.Parameters.AddWithValue("@turn_maximum", oc.turn_maximum);
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
                    SqlCommand cmd = new SqlCommand("spScheduleExceptionDelete", connection);
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
