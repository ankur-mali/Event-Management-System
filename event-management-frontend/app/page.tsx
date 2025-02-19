import { useEffect, useState } from "react";

// Define the Event interface
interface Event {
  id: number;
  name: string;
  description: string;
  date: string;
  location: string;
}

export default function Home() {
  // State to hold events
  const [events, setEvents] = useState<Event[]>([]);
  const [newEvent, setNewEvent] = useState({
    name: "",
    description: "",
    date: "",
    location: "",
  });
  const [editEvent, setEditEvent] = useState<Event | null>(null);

  // Fetch events from the API
  useEffect(() => {
    fetch("http://localhost:5000/api/events") // Adjust API URL as needed
      .then((res) => res.json())
      .then((data) => setEvents(data))
      .catch((err) => console.error("Error fetching events:", err));
  }, []);

  // Handle event input change
  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>,
    field: keyof typeof newEvent
  ) => {
    setNewEvent({ ...newEvent, [field]: e.target.value });
  };

  // Add new event
  const handleAddEvent = async () => {
    try {
      const response = await fetch("http://localhost:5000/api/events", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newEvent),
      });
      const addedEvent = await response.json();
      setEvents((prevEvents) => [...prevEvents, addedEvent]);
      setNewEvent({ name: "", description: "", date: "", location: "" }); // Clear form
    } catch (err) {
      console.error("Error adding event:", err);
    }
  };

  // Delete an event
  const handleDeleteEvent = async (id: number) => {
    try {
      await fetch(`http://localhost:5000/api/events/${id}`, {
        method: "DELETE",
      });
      setEvents(events.filter((event) => event.id !== id));
    } catch (err) {
      console.error("Error deleting event:", err);
    }
  };

  // Edit an event
  const handleEditEvent = (event: Event) => {
    setEditEvent(event);
    setNewEvent({
      name: event.name,
      description: event.description,
      date: event.date,
      location: event.location,
    });
  };

  // Save edited event
  const handleSaveEdit = async () => {
    if (!editEvent) return;
    try {
      const response = await fetch(
        `http://localhost:5000/api/events/${editEvent.id}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify(newEvent),
        }
      );
      const updatedEvent = await response.json();
      setEvents(
        events.map((event) =>
          event.id === updatedEvent.id ? updatedEvent : event
        )
      );
      setEditEvent(null);
      setNewEvent({ name: "", description: "", date: "", location: "" });
    } catch (err) {
      console.error("Error saving event:", err);
    }
  };

  return (
    <div className="min-h-screen bg-gray-100 p-8">
      <h1 className="text-3xl font-bold text-center mb-6">Event Management</h1>

      {/* Event Form for Add and Edit */}
      <div className="mb-6">
        <h2 className="text-xl mb-4">{editEvent ? "Edit Event" : "Add Event"}</h2>
        <input
          type="text"
          placeholder="Event Name"
          className="p-2 border border-gray-300 rounded mb-2 w-full"
          value={newEvent.name}
          onChange={(e) => handleInputChange(e, "name")}
        />
        <textarea
          placeholder="Event Description"
          className="p-2 border border-gray-300 rounded mb-2 w-full"
          value={newEvent.description}
          onChange={(e) => handleInputChange(e, "description")}
        />
        <input
          type="datetime-local"
          className="p-2 border border-gray-300 rounded mb-2 w-full"
          value={newEvent.date}
          onChange={(e) => handleInputChange(e, "date")}
        />
        <input
          type="text"
          placeholder="Location"
          className="p-2 border border-gray-300 rounded mb-4 w-full"
          value={newEvent.location}
          onChange={(e) => handleInputChange(e, "location")}
        />
        <button
          onClick={editEvent ? handleSaveEdit : handleAddEvent}
          className="bg-blue-500 text-white px-4 py-2 rounded"
        >
          {editEvent ? "Save Changes" : "Add Event"}
        </button>
      </div>

      {/* Events List */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {events.map((event) => (
          <div key={event.id} className="bg-white p-4 rounded shadow">
            <h2 className="text-xl font-semibold">{event.name}</h2>
            <p className="text-gray-600">{event.description}</p>
            <p className="text-gray-500">{new Date(event.date).toDateString()}</p>
            <p className="text-gray-700 font-bold">{event.location}</p>
            <div className="flex mt-4 space-x-2">
              <button
                onClick={() => handleEditEvent(event)}
                className="bg-yellow-500 text-white px-3 py-1 rounded"
              >
                Edit
              </button>
              <button
                onClick={() => handleDeleteEvent(event.id)}
                className="bg-red-500 text-white px-3 py-1 rounded"
              >
                Delete
              </button>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
