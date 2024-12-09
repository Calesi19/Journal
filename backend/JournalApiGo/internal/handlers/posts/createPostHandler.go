package posts

import (
	"journal-api/internal/database"
	"net/http"

	"github.com/labstack/echo/v4"
)

func CreatePostHandler(c echo.Context) error {
	query := "SELECT * FROM users"

	results, err := database.DB.Exec(query)
	if err != nil {
		return c.String(http.StatusInternalServerError, "Error creating post")
	}

	// return the results
	return c.JSON(http.StatusOK, results)
}
