package accounts

import (
	"github.com/labstack/echo/v4"
)

func CreateAccountHandler(c echo.Context) error {
	return c.String(200, "token")
}