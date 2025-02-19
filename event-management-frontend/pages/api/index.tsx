import { useState, useEffect } from "react";
import EventList from "../components/EventList";
import axios from "axios";

const Home = () => {
  const [events, setEvents] = useState([]);
  
  useEffect(() => {
    axios
      .get("http://localhost:5000/api/events")
      .then((res) => setEvents(res.data))
      .catch((err) => console.error("Error fetching events:", err));
  }, []);

  const handleDeleteEvent = async (id: number) => {
    try {
      await axios.delete(`http://localhost:5000/api/events/${id}`);
      setEvents(events.filter((event: any) => event.id !== id));
    } catch (err) {
      console.error("Error deleting event:", err);
    }
  };

  return (
    <div className="container mx-auto p-8">
      <h1 className="text-3xl font-bold text-center mb-6">Event Management</h1>
      <EventList events={events} onDelete={handleDeleteEvent} />
    </div>
  );
};

export default Home;
