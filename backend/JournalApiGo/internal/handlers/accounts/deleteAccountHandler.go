package accounts

import (
	"github.com/labstack/echo/v4"
)

func DeleteAccountHandler(c echo.Context) error {
	return c.String(200, "token")
}
