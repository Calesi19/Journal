package posts

import (
	"database/sql"
	"net/http"
	"time"

	"github.com/google/uuid"
	"github.com/labstack/echo/v4"
)

type CreatePostRequest struct {
	Content string `json:"content"`
}

type CreatePostResponse struct {
	IsSuccess bool `json:"isSuccess"`
	post      Post
}

func CreatePostHandler(db *sql.DB) echo.HandlerFunc {
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

		var req CreatePostRequest

		err := c.Bind(&req)
		if err != nil {
			return c.JSON(400, map[string]string{
				"message": "Invalid request.",
			})
		}

		newPost := Post{
			Id:          uuid.New().String(),
			Content:     req.Content,
			DateCreated: time.Now().String(),
			DateUpdated: time.Now().String(),
			UserId:      userIdStr,
		}

		query := `INSERT INTO posts (id, content, date_created, date_updated, user_id) VALUES ($1, $2, $3, $4, $5)`
		_, err = db.Exec(
			query,
			newPost.Id,
			newPost.Content,
			newPost.DateCreated,
			newPost.DateCreated,
			newPost.UserId,
		)
		if err != nil {
			return c.String(http.StatusInternalServerError, "Error inserting post.")
		}

		response := CreatePostResponse{
			IsSuccess: true,
			post:      newPost,
		}

		return c.JSON(http.StatusOK, response)
	}
}
