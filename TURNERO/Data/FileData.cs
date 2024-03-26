using Microsoft.CodeAnalysis.Operations;
using System.Data;
using System.Data.SqlClient;
using TURNERO.Models;

namespace TURNERO.Data
{
    public class FileData
    {
        public List<FileModel> List()
        {
            var ol = new List<FileModel>();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spFileList", connection);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        ol.Add(new FileModel()
                        {
                            id = Convert.ToInt32(dr["id"]),
                            turn_id = Convert.ToInt32(dr["turn_id"]),
                            code = Convert.ToString(dr["code"]),
                    });
                    }
                }
            }
            return ol;
        }
        public FileModel Read(int id)
        {
            var oc = new FileModel();
            var conn = new Connection();
            using (var connection = new SqlConnection(conn.getStringSQL()))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("spFileRead", connection);
                cmd.Parameters.AddWithValue("id", id);
                cmd.CommandType = CommandType.StoredProcedure;

                using (var dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        oc.id = Convert.ToInt32(dr["id"]);
                        oc.turn_id = Convert.ToInt32(dr["turn_id"]);
                        oc.code = Convert.ToString(dr["code"]);
                    }
                }
            }
            return oc;
        }
        public bool Create(FileModel oc)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spFileCreate", connection);
                    cmd.Parameters.AddWithValue("@turn_id", oc.turn_id);
                    cmd.Parameters.AddWithValue("@code", oc.code);
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
        public bool Update(FileModel oc)
        {
            bool res;
            try
            {
                var conn = new Connection();
                using (var connection = new SqlConnection(conn.getStringSQL()))
                {
                    connection.Open();
                    SqlCommand cmd = new SqlCommand("spFileUpdate", connection);
                    cmd.Parameters.AddWithValue("@id", oc.id);
                    cmd.Parameters.AddWithValue("@turn_id", oc.turn_id);
                    cmd.Parameters.AddWithValue("@code", oc.code);
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
                    SqlCommand cmd = new SqlCommand("spFileDelete", connection);
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
