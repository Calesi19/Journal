package middleware

import (
	"net/http"

	"github.com/labstack/echo/v4"
)

func ResponseWrapper(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		responseMap := map[string]interface{}{}
		if err := next(c); err != nil {
			c.Error(err)
		}

		if body, ok := c.Get("response").(map[string]interface{}); ok {
			responseMap["response"] = body
			return c.JSON(http.StatusOK, responseMap)
		}
		return nil
	}
}
