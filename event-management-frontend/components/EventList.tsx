import React from "react";
import { useRouter } from "next/router";

interface Event {
  id: number;
  name: string;
  description: string;
  date: string;
  location: string;
}

interface EventListProps {
  events: Event[];
  onDelete: (id: number) => void;
}

const EventList: React.FC<EventListProps> = ({ events, onDelete }) => {
  const router = useRouter();

  const handleEdit = (id: number) => {
    router.push(`/event/${id}`);
  };

  const handleDelete = async (id: number) => {
    const confirmed = window.confirm("Are you sure you want to delete?");
    if (confirmed) {
      onDelete(id);
    }
  };

  return (
    <div className="space-y-4">
      {events.map((event) => (
        <div key={event.id} className="bg-white p-4 rounded shadow">
          <h2 className="text-xl font-semibold">{event.name}</h2>
          <p className="text-gray-600">{event.description}</p>
          <p className="text-gray-500">{new Date(event.date).toLocaleString()}</p>
          <p className="text-gray-700 font-bold">{event.location}</p>
          <div className="flex mt-4 space-x-2">
            <button
              onClick={() => handleEdit(event.id)}
              className="bg-yellow-500 text-white px-3 py-1 rounded"
            >
              Edit
            </button>
            <button
              onClick={() => handleDelete(event.id)}
              className="bg-red-500 text-white px-3 py-1 rounded"
            >
              Delete
            </button>
          </div>
        </div>
      ))}
    </div>
  );
};

export default EventList;
