package routers

import (
	"journal-api/internal/database"
	"journal-api/internal/handlers/accounts"
	"journal-api/internal/handlers/auth"
	"journal-api/internal/handlers/posts"
	"journal-api/internal/middleware"

	"github.com/labstack/echo/v4"
)

func RegisterRoutes(e *echo.Echo) {
	// auth routes
	e.POST("/login", auth.LoginHandler(database.DB))
	e.POST("/refresh-token", auth.RefreshTokenHandler())

	// account routes
	e.POST("/accounts", accounts.CreateAccountHandler(database.DB))

	// posts routes
	protected := e.Group("/", middleware.AuthMiddleware)
	protected.DELETE("/accounts/:id", accounts.DeleteAccountHandler(database.DB))
	protected.GET("/posts", posts.GetPostsHandler(database.DB))
	protected.POST("/posts", posts.CreatePostHandler(database.DB))
	protected.DELETE("/posts/:id", posts.DeletePostHandler(database.DB))
	protected.PUT("/posts/:id", posts.UpdatePostHandler(database.DB))

	// health check
	e.GET("/health", func(c echo.Context) error {
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
	})
}
