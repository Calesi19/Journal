package auth

import "github.com/labstack/echo/v4"

type LoginRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

func LoginHandler(c echo.Context) error {
	return c.String(200, "token")
}
