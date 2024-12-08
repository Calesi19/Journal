package routers

import (
	"journal-api/internal/handlers/accounts"
	"journal-api/internal/handlers/auth"
	"journal-api/internal/handlers/posts"

	"github.com/labstack/echo/v4"
)

func RegisterRoutes(e *echo.Echo) {
	// auth routes
	e.POST("/login", auth.LoginHandler)
	e.POST("refresh-token", auth.RefreshTokenHandler)

	// account routes
	e.POST("/accounts", accounts.CreateAccountHandler)
	e.DELETE("/accounts", accounts.DeleteAccountHandler)

	// posts routes
	e.GET("/posts", posts.GetPostsHandler)
	e.POST("/posts", posts.CreatePostHandler)
	e.DELETE("/posts/:id", posts.DeletePostHandler)
	e.PUT("/posts/:id", posts.UpdatePostHandler)

	// health check
	e.GET("/health", func(c echo.Context) error {
		response := map[string]string{
			"status":   "ok",
			"database": "ok",
		}
		return c.JSON(200, response)
	})
}
