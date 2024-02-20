using System.Data.SqlClient;

namespace TURNERO.Data
{
    public class Connection
    {
        private string connectionString = string.Empty;

        public Connection()
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            connectionString = builder.GetSection("ConnectionStrings:StringSQL").Value;
        }

        public string getStringSQL()
        {
            return connectionString;
        }
    }
}
