using MySql.Data.MySqlClient;

namespace EventManagementSystem
{
    public class Database
    {
        private readonly string _connectionString;

        public Database()
        {
            
            string server = "";
            string database = "";
            string userID = "";
            string password = "";

            _connectionString = $"Server={server};Database={database};User ID={userID};Password={password};";
            
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
