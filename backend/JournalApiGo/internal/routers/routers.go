package routers

import (
	"journal-api/internal/database"
	"journal-api/internal/handlers/accounts"
	"journal-api/internal/handlers/auth"
	"journal-api/internal/handlers/health"
	"journal-api/internal/handlers/posts"
	"journal-api/internal/middleware"

	"github.com/labstack/echo/v4"
)

func RegisterRoutes(e *echo.Echo) {
	e.GET("/health", health.HealthCheckHandler(database.DB))

	e.POST("/login", auth.LoginHandler(database.DB))
	e.POST("/refresh-token", auth.RefreshTokenHandler())

	e.POST("/accounts", accounts.CreateAccountHandler(database.DB))

	// protected routes
	protected := e.Group("/", middleware.AuthMiddleware)
	protected.DELETE("/accounts/:id", accounts.DeleteAccountHandler(database.DB))
	protected.GET("/posts", posts.GetPostsHandler(database.DB))
	protected.POST("/posts", posts.CreatePostHandler(database.DB))
	protected.DELETE("/posts/:id", posts.DeletePostHandler(database.DB))
	protected.PUT("/posts/:id", posts.UpdatePostHandler(database.DB))
}
