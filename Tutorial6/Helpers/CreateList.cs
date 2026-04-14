using NuGet.Packaging.Signing;

namespace Tutorial6.Helpers;

using Tutorial6.Models;

public static class CreateList
{
    public static List<Room> GetRooms()
    {
        return new List<Room>
        {
            new Room
            {
                Id = 1,
                Name = "Lab 101",
                BuildingCode = "A",
                Floor = 1,
                Capacity = 20,
                HasProjector = true,
                IsActive = true
            },
            new Room
            {
                Id = 2,
                Name = "Conference 202",
                BuildingCode = "A",
                Floor = 2,
                Capacity = 40,
                HasProjector = true,
                IsActive = true
            },
            new Room
            {
                Id = 3,
                Name = "Workshop 1",
                BuildingCode = "B",
                Floor = 1,
                Capacity = 15,
                HasProjector = false,
                IsActive = true
            },
            new Room
            {
                Id = 4,
                Name = "Lab 204",
                BuildingCode = "B",
                Floor = 2,
                Capacity = 24,
                HasProjector = true,
                IsActive = false
            },
            new Room
            {
                Id = 5,
                Name = "Seminar Room",
                BuildingCode = "C",
                Floor = 3,
                Capacity = 30,
                HasProjector = false,
                IsActive = true
            }
        };
    }

    public static List<Reservation> GetReservations()
    {
        return new List<Reservation>
        {
            new Reservation
            {
                Id = 1,
                RoomId = 1,
                OrganizerName = "Anna Kowalska",
                Topic = "HTTP Basics",
                Date = DateTime.Parse("2026-05-10"),
                StartTime = TimeSpan.Parse("09:00"),
                EndTime = TimeSpan.Parse("11:00"),
                Status = "confirmed"
            },
            new Reservation
            {
                Id = 2,
                RoomId = 2,
                OrganizerName = "Jan Nowak",
                Topic = "ASP.NET Core Intro",
                Date = DateTime.Parse("2026-05-10"),
                StartTime = TimeSpan.Parse("12:00"),
                EndTime = TimeSpan.Parse("14:00"),
                Status = "planned"
            },
            new Reservation
            {
                Id = 3,
                RoomId = 3,
                OrganizerName = "Maria Wiśniewska",
                Topic = "Docker Workshop",
                Date = DateTime.Parse("2026-05-11"),
                StartTime = TimeSpan.Parse("10:00"),
                EndTime = TimeSpan.Parse("13:00"),
                Status = "confirmed"
            },
            new Reservation
            {
                Id = 4,
                RoomId = 1,
                OrganizerName = "Piotr Zieliński",
                Topic = "REST API Design",
                Date = DateTime.Parse("2026-05-12"),
                StartTime = TimeSpan.Parse("08:30"),
                EndTime = TimeSpan.Parse("10:30"),
                Status = "cancelled"
            },
            new Reservation
            {
                Id = 5,
                RoomId = 5,
                OrganizerName = "Katarzyna Lewandowska",
                Topic = "Testing with Postman",
                Date = DateTime.Parse("2026-05-12"),
                StartTime = TimeSpan.Parse("11:00"),
                EndTime = TimeSpan.Parse("13:00"),
                Status = "confirmed"
            }
        };
    }
}