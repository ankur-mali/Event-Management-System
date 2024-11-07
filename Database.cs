using MySql.Data.MySqlClient;

namespace EventManagementSystem
{
    public class Database
    {
        private readonly string _connectionString;

        public Database()
        {
            
            string server = "localhost";
            string database = "EventManagementDB";
            string userID = "root";
            string password = "Am@986091";

            _connectionString = $"Server={server};Database={database};User ID={userID};Password={password};";
            
        }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}
