namespace Tutorial6.Helpers;
using Tutorial6.Models;
using Tutorial6.Helpers;

public static class DataStore
{ 
        public static List<Room> Rooms = CreateList.GetRooms(); 
        public static List<Reservation> Reservations = CreateList.GetReservations();
}