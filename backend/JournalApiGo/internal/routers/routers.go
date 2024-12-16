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
	e.DELETE("/accounts", accounts.DeleteAccountHandler(database.DB))

	// posts routes
	postsGroup := e.Group("/posts", middleware.AuthMiddleware)
	postsGroup.GET("", posts.GetPostsHandler(database.DB))
	postsGroup.POST("", posts.CreatePostHandler)
	postsGroup.DELETE("/:id", posts.DeletePostHandler(database.DB))
	postsGroup.PUT("/:id", posts.UpdatePostHandler)

	// health check
	e.GET("/health", func(c echo.Context) error {
		response := map[string]string{
			"status":   "ok",
			"database": "ok",
		}
		return c.JSON(200, response)
	})
}
