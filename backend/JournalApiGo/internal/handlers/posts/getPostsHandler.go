package posts

import (
	"journal-api/internal/database"
	"net/http"

	"github.com/labstack/echo/v4"
)

type GetPostsResponse struct {
	Posts []Post `json:"posts"`
}

type Post struct {
	Id          string `json:"id"`
	Content     string `json:"content"`
	DateCreated string `json:"date_created"`
	DateUpdated string `json:"date_updated"`
	UserId      string `json:"user_id"`
}

func GetPostsHandler(c echo.Context) error {
	query := "SELECT id, content, date_created, date_updated, user_id FROM posts"

	rows, err := database.DB.Query(query)
	if err != nil {
		return c.String(http.StatusInternalServerError, "Error getting posts.")
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
			return c.String(http.StatusInternalServerError, "Error scanning1 posts.")
		}

		posts = append(posts, post)
	}

	if err := rows.Err(); err != nil {
		return c.String(http.StatusInternalServerError, "Error scanning posts.")
	}

	response := GetPostsResponse{
		Posts: posts,
	}

	// return the results
	return c.JSON(http.StatusOK, response)
}
