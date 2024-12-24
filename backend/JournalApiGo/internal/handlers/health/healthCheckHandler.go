package health

import (
	"database/sql"
	"journal-api/internal/database"

	"github.com/labstack/echo/v4"
)

func HealthCheckHandler(db *sql.DB) echo.HandlerFunc {
	return func(c echo.Context) error {
		err := database.DB.Ping()
		if err != nil {
			return c.JSON(500, map[string]string{
				"status":   "error",
				"database": "error",
			})
		}

		response := map[string]string{
			"status":   "ok",
			"database": "ok",
		}
		return c.JSON(200, response)
	}
}
