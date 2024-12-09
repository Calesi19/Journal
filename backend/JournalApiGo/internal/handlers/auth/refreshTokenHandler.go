package auth

import "github.com/labstack/echo/v4"

type RefreshTokenRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

func RefreshTokenHandler(c echo.Context) error {
	return c.String(200, "token")
}
