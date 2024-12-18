package posts

import (
	"database/sql"
	"net/http"
	"time"

	"github.com/labstack/echo/v4"
)

type UpdatePostRequest struct {
	Content string `json:"content"`
}

type UpdatePostResponse struct {
	IsSuccess bool `json:"isSuccess"`
	Post      Post `json:"post"`
}

func UpdatePostHandler(db *sql.DB) echo.HandlerFunc {
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

		// Get the post ID from the URL parameter
		postId := c.Param("id")
		if postId == "" {
			return c.String(http.StatusBadRequest, "Post ID is required")
		}

		// Bind the request payload
		var req UpdatePostRequest
		if err := c.Bind(&req); err != nil {
			return c.JSON(http.StatusBadRequest, map[string]string{
				"message": "Invalid request.",
			})
		}

		// Prepare the query to update the post
		query := `UPDATE posts SET content = $1, date_updated = $2 WHERE id = $3 AND user_id = $4`
		result, err := db.Exec(query, req.Content, time.Now(), postId, userIdStr)
		if err != nil {
			return c.String(http.StatusInternalServerError, "Error updating post.")
		}

		// Check if a post was updated
		rowsAffected, err := result.RowsAffected()
		if err != nil || rowsAffected == 0 {
			return c.JSON(http.StatusNotFound, map[string]string{
				"message": "Post not found or no changes made.",
			})
		}

		// Create a response with the updated post
		updatedPost := Post{
			Id:          postId,
			Content:     req.Content,
			DateUpdated: time.Now().String(),
			UserId:      userIdStr,
		}

		response := UpdatePostResponse{
			IsSuccess: true,
			Post:      updatedPost,
		}

		return c.JSON(http.StatusOK, response)
	}
}
