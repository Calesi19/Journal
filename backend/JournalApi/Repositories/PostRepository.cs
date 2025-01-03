using System.Data;
using Dapper;
using JournalApi.DTOs;
using JournalApi.Models;

namespace JournalApi.Repositories;

public interface IPostRepository
{
    Task<List<Post>> FindByUserIdAsync(Guid userId, QueryParameters parameters);
    Task<int> GetTotalPostCountAsync(Guid userId, QueryParameters parameters);
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

    public async Task<List<Post>> FindByUserIdAsync(Guid userId, QueryParameters parameters)
    {
        var sql =
            @"
        SELECT 
            id AS Id,
            content AS Content,
            date_created AS DateCreated,
            date_updated AS DateUpdated,
            user_id AS UserId
        FROM posts 
        WHERE user_id = @UserId";

        if (!string.IsNullOrEmpty(parameters.SearchText))
        {
            sql += " AND content ILIKE @SearchText";
        }

        if (parameters.DateFrom.HasValue)
        {
            sql += " AND date_created >= @DateFrom";
        }

        if (parameters.DateTo.HasValue)
        {
            sql += " AND date_created <= @DateTo";
        }

        sql += " ORDER BY date_created DESC";

        if (parameters.PageNumber.HasValue && parameters.PageSize.HasValue)
        {
            sql += " OFFSET @Offset LIMIT @PageSize";
        }

        return (
            await _db.QueryAsync<Post>(
                sql,
                new
                {
                    UserId = userId,
                    SearchText = $"%{parameters.SearchText}%",
                    Offset = (parameters.PageNumber - 1) * parameters.PageSize,
                    PageSize = parameters.PageSize,
                }
            )
        ).ToList();
    }

    public async Task<int> GetTotalPostCountAsync(Guid userId, QueryParameters parameters)
    {
        var sql = "SELECT COUNT(*) FROM posts WHERE user_id = @UserId";

        if (!string.IsNullOrEmpty(parameters.SearchText))
        {
            sql += " AND content ILIKE @SearchText";
        }

        if (parameters.DateFrom.HasValue)
        {
            sql += " AND date_created >= @DateFrom";
        }

        if (parameters.DateTo.HasValue)
        {
            sql += " AND date_created <= @DateTo";
        }

        return await _db.ExecuteScalarAsync<int>(
            sql,
            new { UserId = userId, SearchText = $"%{parameters.SearchText}%" }
        );
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
