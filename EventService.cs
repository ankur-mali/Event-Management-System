using MySql.Data.MySqlClient;
using System.Data;
using System.Text;


namespace EventManagementSystem
{
    public class EventService
    {
        private readonly Database _database;

        public EventService()
        {
            _database = new Database();
        }

        public Event CreateEvent(string name, string description, DateTime date, string location)
        {
            try
            {
                using var connection = _database.GetConnection();
                connection.Open();

                string query = "INSERT INTO Events (Name, Description, Date, Location) VALUES (@name, @description, @date, @location)";
                using var cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@description", description);
                cmd.Parameters.AddWithValue("@date", date);
                cmd.Parameters.AddWithValue("@location", location);
                cmd.ExecuteNonQuery();

                return new Event
                {
                    Id = (int)cmd.LastInsertedId,
                    Name = name,
                    Description = description,
                    Date = date,
                    Location = location
                };
            }
            catch (MySqlException ex)
            {
                Console.WriteLine($"Database error: {ex.Message}");
                return null;
            }
        }

        public List<Event> ListEvents()
        {
            var events = new List<Event>();
            using var connection = _database.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Events";
            using var cmd = new MySqlCommand(query, connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                events.Add(new Event
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                    Date = reader.GetDateTime("Date"),
                    Location = reader.IsDBNull("Location") ? "" : reader.GetString("Location")
                });
            }

            return events;
        }

        public Event GetEventById(int id)
        {
            using var connection = _database.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Events WHERE Id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Event
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                    Date = reader.GetDateTime("Date"),
                    Location = reader.IsDBNull("Location") ? "" : reader.GetString("Location")
                };
            }

            return null;
        }

        public bool UpdateEvent(int id, string name, string description, DateTime? date, string location)
        {
            using var connection = _database.GetConnection();
            connection.Open();

            var queryBuilder = new StringBuilder("UPDATE Events SET ");
            var parameters = new List<MySqlParameter>();

            if (!string.IsNullOrEmpty(name))
            {
                queryBuilder.Append("Name = @name, ");
                parameters.Add(new MySqlParameter("@name", name));
            }

            if (!string.IsNullOrEmpty(description))
            {
                queryBuilder.Append("Description = @description, ");
                parameters.Add(new MySqlParameter("@description", description));
            }

            if (date.HasValue)
            {
                queryBuilder.Append("Date = @date, ");
                parameters.Add(new MySqlParameter("@date", date.Value));
            }

            if (!string.IsNullOrEmpty(location))
            {
                queryBuilder.Append("Location = @location, ");
                parameters.Add(new MySqlParameter("@location", location));
            }

            // Remove the last comma and space
            if (parameters.Count > 0)
            {
                queryBuilder.Length -= 2; // Remove the trailing ", "
            }
            else
            {
                // No fields to update
                Console.WriteLine("No fields provided to update.");
                return false;
            }

            queryBuilder.Append(" WHERE Id = @id");
            parameters.Add(new MySqlParameter("@id", id));

            using var cmd = new MySqlCommand(queryBuilder.ToString(), connection);
            cmd.Parameters.AddRange(parameters.ToArray());

            return cmd.ExecuteNonQuery() > 0;
        }

        public bool DeleteEvent(int id)
        {
            using var connection = _database.GetConnection();
            connection.Open();

            string query = "DELETE FROM Events WHERE Id = @id";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@id", id);

            return cmd.ExecuteNonQuery() > 0;
        }
        public List<Event> FilterEventsByLocation(string location)
        {
            var events = new List<Event>();
            using var connection = _database.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Events WHERE Location LIKE @location";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@location", $"%{location}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                events.Add(new Event
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                    Date = reader.GetDateTime("Date"),
                    Location = reader.IsDBNull("Location") ? "" : reader.GetString("Location")
                });
            }

            return events;
        }

        public List<Event> FilterEventsByDate(DateTime date)
        {
            var events = new List<Event>();
            using var connection = _database.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Events WHERE Date = @date";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@date", date);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                events.Add(new Event
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                    Date = reader.GetDateTime("Date"),
                    Location = reader.IsDBNull("Location") ? "" : reader.GetString("Location")
                });
            }

            return events;
        }

        public List<Event> SearchEventsByKeyword(string keyword)
        {
            var events = new List<Event>();
            using var connection = _database.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Events WHERE Name LIKE @keyword OR Description LIKE @keyword";
            using var cmd = new MySqlCommand(query, connection);
            cmd.Parameters.AddWithValue("@keyword", $"%{keyword}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                events.Add(new Event
                {
                    Id = reader.GetInt32("Id"),
                    Name = reader.GetString("Name"),
                    Description = reader.IsDBNull("Description") ? "" : reader.GetString("Description"),
                    Date = reader.GetDateTime("Date"),
                    Location = reader.IsDBNull("Location") ? "" : reader.GetString("Location")
                });
            }

            return events;
        }

        public void ExportEventsToCsv(string filePath)
        {
            var events = ListEvents();

            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine("Id,Name,Description,Date,Location"); 

                foreach (var evt in events)
                {
                    writer.WriteLine($"{evt.Id},{evt.Name},{evt.Description},{evt.Date:yyyy-MM-dd},{evt.Location}");
                }
            }
            Console.WriteLine("Events exported successfully to " + filePath);
        }

        public void ImportEventsFromCsv(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                return;
            }

            using (var reader = new StreamReader(filePath))
            {
                string headerLine = reader.ReadLine(); // Skip headers
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    var fields = line.Split(',');

                    if (fields.Length >= 4 && DateTime.TryParse(fields[3], out DateTime date))
                    {
                        var name = fields[1];
                        var description = fields[2];
                        var location = fields.Length > 4 ? fields[4] : "";

                        CreateEvent(name, description, date, location); 
                    }
                }
            }
            Console.WriteLine("Events imported successfully from " + filePath);
        }
    }

    }


    

