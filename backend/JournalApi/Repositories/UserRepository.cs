using System.Data;
using Dapper;
using JournalApi.Models;

namespace JournalApi.Repositories;

public interface IUserRepository
{
    Task<User> FindByEmailAsync(string email);
    Task<User> FindByIdAsync(Guid id);
    Task<User> AddAsync(User newUser);
    Task<User> UpdateAsync(User updatedUser);
    Task<User> DeleteAsync(Guid id);
}

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;

    public UserRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<User> FindByEmailAsync(string email)
    {
        var sql = "SELECT * FROM users WHERE email = @Email";
        return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
    }

    public async Task<User> FindByIdAsync(Guid id)
    {
        var sql = "SELECT * FROM users WHERE id = @Id";
        return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }

    public async Task<User> AddAsync(User newUser)
    {
        var sql =
            "INSERT INTO users (id, email, password, is_email_Confirmed) "
            + "VALUES (@Id, @Email, @Password, @IsEmailConfirmed)";
        await _db.ExecuteAsync(sql, newUser);
        return newUser;
    }

    public async Task<User> UpdateAsync(User updatedUser)
    {
        var sql =
            "UPDATE users SET email = @Email, password = @Password, "
            + "is_email_confirmed = @IsEmailConfirmed "
            + "WHERE id = @Id";
        await _db.ExecuteAsync(sql, updatedUser);
        return updatedUser;
    }

    public async Task<User> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM users WHERE id = @Id";
        return await _db.QueryFirstOrDefaultAsync<User>(sql, new { Id = id });
    }
}
