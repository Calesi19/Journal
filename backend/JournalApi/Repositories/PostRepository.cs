using System.Data;
using Dapper;
using JournalApi.Models;

namespace JournalApi.Repositories;

public interface IPostRepository
{
    Task<List<Post>> FindByUserIdAsync(Guid userId);
    Task<Post> FindByIdAsync(Guid id);
    Task<Post> AddAsync(Post newPost);
    Task<Post> UpdateAsync(Post updatedPost);
    Task<Post> DeleteAsync(Guid id);
}

public class PostRepository : IPostRepository
{
    private readonly IDbConnection _db;

    public PostRepository(IDbConnection db)
    {
        _db = db;
    }

    public async Task<List<Post>> FindByUserIdAsync(Guid userId)
    {
        var sql =
            @"SELECT 
            id AS Id,
            content AS Content,
            date_created AS DateCreated,
            date_updated AS DateUpdated,
            user_id AS UserId
            FROM posts WHERE user_id = @UserId";
        return (await _db.QueryAsync<Post>(sql, new { UserId = userId })).ToList();
    }

    public async Task<Post> FindByIdAsync(Guid id)
    {
        var sql =
            @"SELECT
            id AS Id,
            content AS Content,
            date_created AS DateCreated,
            date_updated AS DateUpdated,
            user_id AS UserId
            FROM posts WHERE id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
    }

    public async Task<Post> AddAsync(Post newPost)
    {
        var sql =
            "INSERT INTO posts (id, content, date_created, date_updated, "
            + "user_id) "
            + "VALUES (@Id, @Content, @DateCreated, @DateUpdated, @UserId)";
        await _db.ExecuteAsync(sql, newPost);
        return newPost;
    }

    public async Task<Post> UpdateAsync(Post updatedPost)
    {
        var sql =
            "UPDATE posts SET content = @Content, date_updated = " + "@DateUpdated WHERE id = @Id";
        await _db.ExecuteAsync(sql, updatedPost);
        return updatedPost;
    }

    public async Task<Post> DeleteAsync(Guid id)
    {
        var sql = "DELETE FROM posts WHERE id = @Id";
        return await _db.QueryFirstOrDefaultAsync<Post>(sql, new { Id = id });
    }
}
