namespace Tutorial6.DTOs;

public class CreateReservationDto
{
    public int RoomId { get; set; }
    public string OrganizerName { get; set; }
    public string Topic {get; set;}
    public DateTime Date { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string Status { get; set; }
}