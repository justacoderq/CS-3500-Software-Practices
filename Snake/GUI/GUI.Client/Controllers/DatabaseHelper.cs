using MySql.Data.MySqlClient;

namespace GUI.Client.Controllers
{
    public static class DatabaseHelper
    {

        /// <summary>
        /// The connection string.
        /// Your uID login name serves as both your database name and your uid
        /// </summary>
        public const string connectionString = "server=atr.eng.utah.edu;database=u1479273;uid=u1479273;password=mysqlpassword";

        public static MySqlConnection GetConnection()
        {
            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();
                return connection;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
