using System.Data;
using Dapper;

namespace JournalApi;

public interface IUserRepository
{
    Task<UserModel> GetUserByEmail(string email);
    Task<UserModel> GetUserById(Guid id);
    Task<UserModel> CreateUser(UserModel user);
    Task<UserModel> UpdateUser(UserModel user);
    Task<UserModel> DeleteUser(Guid id);
}

public class UserRepository : IUserRepository
{
    private readonly IDbConnection _db;

    public UserRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<UserModel> GetUserByEmail(string email)
    {
        var sql = "SELECT * FROM Users WHERE Email = @Email";
        return await _db.QueryFirstOrDefaultAsync<UserModel>(sql, new { Email = email });
    }

    public async Task<UserModel> GetUserById(Guid id)
    {
        var sql = "SELECT * FROM Users WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<UserModel>(sql, new { Id = id });
    }

    public async Task<UserModel> CreateUser(UserModel user)
    {
        var sql =
            "INSERT INTO Users (Id, Email, Password, EmailConfirmed) "
            + "VALUES (@Id, @Email, @Password, @EmailConfirmed)";
        await _db.ExecuteAsync(sql, user);
        return user;
    }

    public async Task<UserModel> UpdateUser(UserModel user)
    {
        var sql =
            "UPDATE Users SET Email = @Email, Password = @Password, "
            + "EmailConfirmed = @EmailConfirmed WHERE Id = @Id";
        await _db.ExecuteAsync(sql, user);
        return user;
    }

    public async Task<UserModel> DeleteUser(Guid id)
    {
        var sql = "DELETE FROM Users WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<UserModel>(sql, new { Id = id });
    }

    public async Task<UserModel> ConfirmEmail(Guid id)
    {
        var sql = "UPDATE Users SET EmailConfirmed = true WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<UserModel>(sql, new { Id = id });
    }

    public async Task<UserModel> ChangePassword(Guid id, string password)
    {
        var sql = "UPDATE Users SET Password = @Password WHERE Id = @Id";
        return await _db.QueryFirstOrDefaultAsync<UserModel>(
            sql,
            new { Id = id, Password = password }
        );
    }
}
