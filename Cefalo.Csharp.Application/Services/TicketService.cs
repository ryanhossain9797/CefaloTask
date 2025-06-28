using Cefalo.Csharp.Core.Entities;
using Cefalo.Csharp.Core.Services;
using Cefalo.Csharp.Core.Repositories;

namespace Cefalo.Csharp.Application.Services;

public class TicketService(ITicketRepository ticketRepository, IUserRepository userRepository) : ITicketService
{
    private readonly ITicketRepository _ticketRepository = ticketRepository;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Ticket?> GetTicketByIdAsync(int id)
    {
        return await _ticketRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Ticket>> GetAllTicketsAsync()
    {
        return await _ticketRepository.GetAllAsync();
    }

    public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId)
    {
        _ = await _userRepository.GetByIdAsync(userId) ?? throw new ArgumentException($"User with ID {userId} not found");

        return await _ticketRepository.GetByUserIdAsync(userId);
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        _ = await _userRepository.GetByIdAsync(ticket.UserId) ?? throw new ArgumentException($"User with ID {ticket.UserId} not found");

        ticket.Status = TicketStatus.Todo;
        ticket.CreatedAt = DateTime.UtcNow;

        return await _ticketRepository.AddAsync(ticket);
    }

    public async Task<Ticket> UpdateTicketAsync(Ticket ticket)
    {
        var existingTicket = await _ticketRepository.GetByIdAsync(ticket.Id) ?? throw new ArgumentException($"Ticket with ID {ticket.Id} not found");

        if (ticket.UserId != existingTicket.UserId)
        {
            _ = await _userRepository.GetByIdAsync(ticket.UserId) ?? throw new ArgumentException($"User with ID {ticket.UserId} not found");
        }

        return await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task DeleteTicketAsync(int id)
    {
        _ = await _ticketRepository.GetByIdAsync(id) ?? throw new ArgumentException($"Ticket with ID {id} not found");

        await _ticketRepository.DeleteAsync(id);
    }
}