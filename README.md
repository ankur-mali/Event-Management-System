# Event-Management-System  

## Overview  
The **Event Management System** is a console-based application designed to manage events efficiently. Users can create, update, delete, and view events stored in a MySQL database. The application also supports exporting and importing event data in CSV format.  

## Features  
- **Create Events**: Add new events with a name, description, date, and location.  
- **List Events**: View all events stored in the database.  
- **Search & Filter**:  
  - Search events by keyword (name or description).  
  - Filter events by location or date.  
- **Update & Delete**: Modify or remove existing events.  
- **Export to CSV**: Backup events to a CSV file.  
- **Import from CSV**: Load events from a CSV file.  

## Prerequisites  
Ensure you have the following installed:  
- .NET 6 SDK or later  
- MySQL Server  
- Visual Studio (or any compatible IDE)  

## Setup Instructions  

### 1. Clone the Repository  
bash  
git clone https://github.com/ankur-mali/Event-Management-System.git  
cd Event-Management-System  

2. Configure the Database
Create a MySQL database using the following command:

## sql
Copy code
CREATE DATABASE EventManagementDB;
Run the events.sql script located in the sql folder to create the Events table.

## 3. Update Connection String
Modify the Database class to use your MySQL credentials:

csharp
Copy code
private const string ConnectionString = "Server=localhost;Database=EventManagementDB;User ID=your-username;Password=your-password;";
## 4. Run the Application
Build and run the application:

### bash
Copy code
dotnet run

## How to Use

- **Create Event**: `create`  
- **List Events**: `list`  
- **Search Event**: `search <keyword>`  
- **Filter by Location**: `filter location <location>`  
- **Filter by Date**: `filter date <yyyy-MM-dd>`  
- **Update Event**: `update <event_id>`  
- **Delete Event**: `delete <event_id>`  
- **Export to CSV**: `export <file_path>`  
- **Import from CSV**: `import <file_path>`  
## Preview of example 
![Screenshot 2024-11-05 213643](https://github.com/user-attachments/assets/44e3f44e-7417-4277-a217-acd974319c03)

### CSV Export/Import
Exported files will be saved in the specified path.
Ensure imported CSV files follow this format: Id,Name,Description,Date,Location.
## Contributing
Feel free to fork this repository and make your changes. Contributions are welcome!
