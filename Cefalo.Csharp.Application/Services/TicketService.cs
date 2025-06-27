using Cefalo.Csharp.Core.Entities;
using Cefalo.Csharp.Core.Services;
using Cefalo.Csharp.Core.Repositories;

namespace Cefalo.Csharp.Application.Services;

public class TicketService : ITicketService
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IUserRepository _userRepository;

    public TicketService(ITicketRepository ticketRepository, IUserRepository userRepository)
    {
        _ticketRepository = ticketRepository;
        _userRepository = userRepository;
    }

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
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        return await _ticketRepository.GetByUserIdAsync(userId);
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        // Validate user exists
        var user = await _userRepository.GetByIdAsync(ticket.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {ticket.UserId} not found");
        }

        // Set default values
        ticket.Status = Cefalo.Csharp.Core.Entities.TicketStatus.Todo;
        ticket.CreatedAt = DateTime.UtcNow;

        return await _ticketRepository.AddAsync(ticket);
    }

    public async Task<Ticket> UpdateTicketAsync(Ticket ticket)
    {
        var existingTicket = await _ticketRepository.GetByIdAsync(ticket.Id);
        if (existingTicket == null)
        {
            throw new ArgumentException($"Ticket with ID {ticket.Id} not found");
        }

        // Validate user exists if changed
        if (ticket.UserId != existingTicket.UserId)
        {
            var user = await _userRepository.GetByIdAsync(ticket.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {ticket.UserId} not found");
            }
        }

        return await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task DeleteTicketAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            throw new ArgumentException($"Ticket with ID {id} not found");
        }

        await _ticketRepository.DeleteAsync(id);
    }
}