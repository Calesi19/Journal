package posts

import (
	"database/sql"
	"net/http"

	"github.com/labstack/echo/v4"
)

type DeletePostResponse struct {
	IsSuccess bool   `json:"isSuccess"`
	Message   string `json:"message"`
}

func DeletePostHandler(db *sql.DB) echo.HandlerFunc {
	return func(c echo.Context) error {
		userId := c.Get("userId")
		if userId == nil {
			return c.String(http.StatusUnauthorized, "User ID not found in context")
		}

		userIdStr, ok := userId.(string)
		if !ok {
			return c.String(http.StatusInternalServerError, "Invalid user ID format")
		}

		query := `DELETE FROM posts WHERE user_id = $1 AND id = $2`
		result, err := db.Exec(query, userIdStr, c.Param("id"))
		if err != nil {
			return c.String(http.StatusInternalServerError, "Error deleting post: "+err.Error())
		}

		rowsAffected, err := result.RowsAffected()
		if err != nil {
			return c.String(
				http.StatusInternalServerError,
				"Error fetching rows affected: "+err.Error(),
			)
		}
		if rowsAffected == 0 {
			return c.JSON(http.StatusNotFound, DeletePostResponse{
				IsSuccess: false,
				Message:   "Post not found or not authorized to delete",
			})
		}

		response := DeletePostResponse{
			IsSuccess: true,
			Message:   "Post deleted successfully",
		}
		return c.JSON(http.StatusOK, response)
	}
}
