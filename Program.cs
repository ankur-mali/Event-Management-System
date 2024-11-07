using System;
using System.Linq;
using System.Collections.Generic;

namespace EventManagementSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            EventService eventService = new EventService();
            Console.WriteLine("Welcome to the Event Management System!");

            while (true)
            {
                Console.Write("Please enter a command: ");
                var input = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(input)) continue;

                var command = input.Split(" ");
                switch (command[0].ToLower())
                {
                    case "create":
                        CreateEvent(eventService);
                        break;
                    case "list":
                        ListEvents(eventService);
                        break;
                    case "get":
                        if (command.Length > 1 && int.TryParse(command[1], out int getId))
                            GetEvent(eventService, getId);
                        else
                            Console.WriteLine("Please provide a valid event ID.");
                        break;
                    case "update":
                        if (command.Length > 1 && int.TryParse(command[1], out int updateId))
                            UpdateEvent(eventService, updateId);
                        else
                            Console.WriteLine("Please provide a valid event ID.");
                        break;
                    case "delete":
                        if (command.Length > 1 && int.TryParse(command[1], out int deleteId))
                            DeleteEvent(eventService, deleteId);
                        else
                            Console.WriteLine("Please provide a valid event ID.");
                        break;
                    case "filter":
                        FilterEvents(eventService, command);
                        break;
                    case "search":
                        if (command.Length > 1)
                            SearchEvents(eventService, string.Join(" ", command.Skip(1)));
                        else
                            Console.WriteLine("Please enter a keyword to search.");
                        break;
                    case "export":
                        ExportEvents(eventService, command.Length > 1 ? command[1] : "events.csv");
                        break;
                    case "import":
                        ImportEvents(eventService, command.Length > 1 ? command[1] : "events.csv");
                        break;
                    case "exit":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Unknown command.");
                        break;
                }
            }
        }

        static void CreateEvent(EventService eventService)
        {
            Console.Write("Enter Event Name: ");
            string name = Console.ReadLine();

            Console.Write("Enter Description (optional): ");
            string description = Console.ReadLine();

            Console.Write("Enter Date (yyyy-MM-dd): ");
            DateTime date;
            while (!DateTime.TryParse(Console.ReadLine(), out date))
            {
                Console.WriteLine("Invalid date format. Please enter the date as yyyy-MM-dd.");
            }

            Console.Write("Enter Location (optional): ");
            string location = Console.ReadLine();

            var newEvent = eventService.CreateEvent(name, description, date, location);
            Console.WriteLine($"Event created successfully with ID: {newEvent.Id}");
        }

        static void ListEvents(EventService eventService)
        {
            var events = eventService.ListEvents();
            if (events.Count == 0)
            {
                Console.WriteLine("No events found.");
            }
            else
            {
                foreach (var evt in events)
                {
                    Console.WriteLine($"ID: {evt.Id}, Name: {evt.Name}, Date: {evt.Date:yyyy-MM-dd}, Location: {evt.Location}");
                }
            }
        }

        static void GetEvent(EventService eventService, int id)
        {
            var evt = eventService.GetEventById(id);
            if (evt == null)
            {
                Console.WriteLine("Event not found.");
            }
            else
            {
                Console.WriteLine($"Event Details:\nName: {evt.Name}\nDescription: {evt.Description}\nDate: {evt.Date:yyyy-MM-dd}\nLocation: {evt.Location}");
            }
        }

        static void UpdateEvent(EventService eventService, int id)
        {
            Console.Write("Enter new Event Name (leave empty to keep current): ");
            string name = Console.ReadLine();

            Console.Write("Enter new Description (leave empty to keep current): ");
            string description = Console.ReadLine();

            Console.Write("Enter new Date (leave empty to keep current): ");
            DateTime date;
            DateTime? newDate = DateTime.TryParse(Console.ReadLine(), out date) ? date : (DateTime?)null;

            Console.Write("Enter new Location (leave empty to keep current): ");
            string location = Console.ReadLine();

            bool isUpdated = eventService.UpdateEvent(id, name, description, newDate, location);
            Console.WriteLine(isUpdated ? "Event updated successfully!" : "Event not found.");
        }

        static void DeleteEvent(EventService eventService, int id)
        {
            bool isDeleted = eventService.DeleteEvent(id);
            Console.WriteLine(isDeleted ? "Event deleted successfully!" : "Event not found.");
        }

        static void FilterEvents(EventService eventService, string[] command)
        {
            if (command.Length < 3)
            {
                Console.WriteLine("Invalid filter command. Use 'filter location [location]' or 'filter date [yyyy-MM-dd]'.");
                return;
            }

            switch (command[1].ToLower())
            {
                case "location":
                    var location = string.Join(" ", command.Skip(2));
                    var locationFilteredEvents = eventService.FilterEventsByLocation(location);
                    if (locationFilteredEvents.Count == 0)
                    {
                        Console.WriteLine("No events found for the specified location.");
                    }
                    else
                    {
                        foreach (var evt in locationFilteredEvents)
                        {
                            Console.WriteLine($"ID: {evt.Id}, Name: {evt.Name}, Date: {evt.Date:yyyy-MM-dd}, Location: {evt.Location}");
                        }
                    }
                    break;
                case "date":
                    if (DateTime.TryParse(command[2], out DateTime date))
                    {
                        var dateFilteredEvents = eventService.FilterEventsByDate(date);
                        if (dateFilteredEvents.Count == 0)
                        {
                            Console.WriteLine("No events found for the specified date.");
                        }
                        else
                        {
                            foreach (var evt in dateFilteredEvents)
                            {
                                Console.WriteLine($"ID: {evt.Id}, Name: {evt.Name}, Date: {evt.Date:yyyy-MM-dd}, Location: {evt.Location}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
                    }
                    break;
                default:
                    Console.WriteLine("Unknown filter type.");
                    break;
            }
        }

        static void SearchEvents(EventService eventService, string keyword)
        {
            var searchResults = eventService.SearchEventsByKeyword(keyword);
            if (searchResults.Count == 0)
            {
                Console.WriteLine("No events found for the specified keyword.");
            }
            else
            {
                foreach (var evt in searchResults)
                {
                    Console.WriteLine($"ID: {evt.Id}, Name: {evt.Name}, Date: {evt.Date:yyyy-MM-dd}, Location: {evt.Location}");
                }
            }
        }
        static void ExportEvents(EventService eventService, string filePath)
        {
            eventService.ExportEventsToCsv(filePath);
        }

        static void ImportEvents(EventService eventService, string filePath)
        {
            eventService.ImportEventsFromCsv(filePath);
        }
    }
}
