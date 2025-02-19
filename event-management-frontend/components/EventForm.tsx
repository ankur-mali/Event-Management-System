import React, { useState, useEffect } from "react";
import { useRouter } from "next/router";

interface Event {
  name: string;
  description: string;
  date: string;
  location: string;
}

interface EventFormProps {
  initialData?: Event;
  onSubmit: (event: Event) => void;
}

const EventForm: React.FC<EventFormProps> = ({ initialData, onSubmit }) => {
  const [formData, setFormData] = useState<Event>({
    name: initialData?.name || "",
    description: initialData?.description || "",
    date: initialData?.date || "",
    location: initialData?.location || "",
  });

  const handleInputChange = (
    e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    onSubmit(formData);
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-4">
      <input
        type="text"
        name="name"
        placeholder="Event Name"
        value={formData.name}
        onChange={handleInputChange}
        className="p-2 border rounded w-full"
        required
      />
      <textarea
        name="description"
        placeholder="Event Description"
        value={formData.description}
        onChange={handleInputChange}
        className="p-2 border rounded w-full"
        required
      />
      <input
        type="datetime-local"
        name="date"
        value={formData.date}
        onChange={handleInputChange}
        className="p-2 border rounded w-full"
        required
      />
      <input
        type="text"
        name="location"
        placeholder="Event Location"
        value={formData.location}
        onChange={handleInputChange}
        className="p-2 border rounded w-full"
        required
      />
      <button type="submit" className="bg-blue-500 text-white px-4 py-2 rounded">
        Submit
      </button>
    </form>
  );
};

export default EventForm;
