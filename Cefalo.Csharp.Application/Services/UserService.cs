using Cefalo.Csharp.Core.Entities;
using Cefalo.Csharp.Core.Services;
using Cefalo.Csharp.Core.Repositories;

namespace Cefalo.Csharp.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _userRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _userRepository.GetAllAsync();
    }

    public async Task<User> CreateUserAsync(User user)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new ArgumentException("User name is required");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("User email is required");
        }

        // Email format validation (basic)
        if (!user.Email.Contains("@"))
        {
            throw new ArgumentException("Invalid email format");
        }

        return await _userRepository.AddAsync(user);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        var existingUser = await _userRepository.GetByIdAsync(user.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"User with ID {user.Id} not found");
        }

        // Basic validation
        if (string.IsNullOrWhiteSpace(user.Name))
        {
            throw new ArgumentException("User name is required");
        }

        if (string.IsNullOrWhiteSpace(user.Email))
        {
            throw new ArgumentException("User email is required");
        }

        return await _userRepository.UpdateAsync(user);
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {id} not found");
        }

        await _userRepository.DeleteAsync(id);
    }
}