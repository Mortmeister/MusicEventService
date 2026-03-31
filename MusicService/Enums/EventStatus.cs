using System.ComponentModel;

namespace MusicService.Enums;

public enum EventStatus
{
    [Description("Scheduled and open for bookings")]
    Upcoming,
    [Description("Event date has passed")]
    Completed,
    [Description("Cancelled by the organiser")]
    Cancelled,
}