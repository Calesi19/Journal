package posts

import (
	"database/sql"
	"net/http"
	"time"

	"github.com/labstack/echo/v4"
)

type GetPostsResponse struct {
	Posts []Post `json:"posts"`
}

type Post struct {
	Id          string `json:"id"`
	Content     string `json:"content"`
	DateCreated string `json:"dateCreated"`
	DateUpdated string `json:"dateUpdated"`
	UserId      string `json:"userId"`
}

type QueryParams struct {
	UserId     string `query:"userId"`
	SearchText string `query:"searchText"`
	DateFrom   string `query:"dateFrom"`
	DateTo     string `query:"dateTo"`
	PageNumber int    `query:"pageNumber"`
	PageSize   int    `query:"pageSize"`
}

func GetPostsHandler(db *sql.DB) echo.HandlerFunc {
	return func(c echo.Context) error {
		// Get userId from context
		userId := c.Get("userId")
		if userId == nil {
			return c.String(http.StatusUnauthorized, "User ID not found in context")
		}

		// Ensure userId is a string
		userIdStr, ok := userId.(string)
		if !ok {
			return c.String(http.StatusInternalServerError, "Invalid user ID format")
		}

		var params QueryParams
		if err := c.Bind(&params); err != nil {
			return c.String(http.StatusBadRequest, "Invalid query parameters.")
		}

		args := []interface{}{userIdStr}
		query := `SELECT id, content, date_created AS dateCreated, date_updated AS dateUpdated, user_id AS userId
		          FROM posts
		          WHERE user_id = $1`

		if params.SearchText != "" {
			query += " AND content ILIKE $2"
			args = append(args, "%"+params.SearchText+"%")
		}

		if params.DateFrom != "" {
			dateFrom, err := time.Parse("2006-01-02", params.DateFrom)
			if err != nil {
				return c.String(http.StatusBadRequest, "Invalid DateFrom format. Use YYYY-MM-DD.")
			}
			query += " AND date_created >= $3"
			args = append(args, dateFrom)
		}

		if params.DateTo != "" {
			dateTo, err := time.Parse("2006-01-02", params.DateTo)
			if err != nil {
				return c.String(http.StatusBadRequest, "Invalid DateTo format. Use YYYY-MM-DD.")
			}
			query += " AND date_created <= $4"
			args = append(args, dateTo)
		}

		query += " ORDER BY date_created DESC"

		if params.PageNumber > 0 && params.PageSize > 0 {
			offset := (params.PageNumber - 1) * params.PageSize
			query += " LIMIT $5 OFFSET $6"
			args = append(args, params.PageSize, offset)
		}

		// Execute the query
		rows, err := db.Query(query, args...)
		if err != nil {
			return c.String(http.StatusInternalServerError, "Error querying posts: "+err.Error())
		}
		defer rows.Close()

		var posts []Post

		for rows.Next() {
			var post Post
			err := rows.Scan(
				&post.Id,
				&post.Content,
				&post.DateCreated,
				&post.DateUpdated,
				&post.UserId,
			)
			if err != nil {
				return c.String(http.StatusInternalServerError, "Error scanning posts.")
			}

			posts = append(posts, post)
		}

		if err := rows.Err(); err != nil {
			return c.String(http.StatusInternalServerError, "Error with result set.")
		}

		// Construct response
		response := GetPostsResponse{
			Posts: posts,
		}

		return c.JSON(http.StatusOK, response)
	}
}
