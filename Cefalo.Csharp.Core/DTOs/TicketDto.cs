using Cefalo.Csharp.Core.Entities;

namespace Cefalo.Csharp.Core.DTOs;

public class TicketDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public Cefalo.Csharp.Core.Entities.TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public int UserId { get; set; }
}