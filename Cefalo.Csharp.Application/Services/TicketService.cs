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
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        if (user.Deleted)
        {
            throw new ArgumentException($"Cannot get tickets for deleted user with ID {userId}");
        }

        return await _ticketRepository.GetByUserIdAsync(userId);
    }

    public async Task<Ticket> CreateTicketAsync(Ticket ticket)
    {
        var user = await _userRepository.GetByIdAsync(ticket.UserId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {ticket.UserId} not found");
        }

        if (user.Deleted)
        {
            throw new ArgumentException($"Cannot create ticket for deleted user with ID {ticket.UserId}");
        }

        ticket.Status = TicketStatus.Todo;
        ticket.CreatedAt = DateTime.UtcNow;
        ticket.Deleted = false; // Ensure new tickets are not marked as deleted

        return await _ticketRepository.AddAsync(ticket);
    }

    public async Task<Ticket> UpdateTicketAsync(Ticket ticket)
    {
        var existingTicket = await _ticketRepository.GetByIdAsync(ticket.Id);
        if (existingTicket == null)
        {
            throw new ArgumentException($"Ticket with ID {ticket.Id} not found");
        }

        if (existingTicket.Deleted)
        {
            throw new ArgumentException($"Cannot update deleted ticket with ID {ticket.Id}");
        }

        if (ticket.UserId != existingTicket.UserId)
        {
            var user = await _userRepository.GetByIdAsync(ticket.UserId);
            if (user == null)
            {
                throw new ArgumentException($"User with ID {ticket.UserId} not found");
            }

            if (user.Deleted)
            {
                throw new ArgumentException($"Cannot assign ticket to deleted user with ID {ticket.UserId}");
            }
        }

        ticket.Deleted = false; // Ensure we don't accidentally mark as deleted during update
        return await _ticketRepository.UpdateAsync(ticket);
    }

    public async Task DeleteTicketAsync(int id)
    {
        var ticket = await _ticketRepository.GetByIdAsync(id);
        if (ticket == null)
        {
            throw new ArgumentException($"Ticket with ID {id} not found");
        }

        if (ticket.Deleted)
        {
            throw new ArgumentException($"Ticket with ID {id} is already deleted");
        }

        await _ticketRepository.DeleteAsync(id);
    }
}