package middleware

import (
	"net/http"

	"github.com/labstack/echo/v4"
)

func RequestUnwrapper(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		reqBody := map[string]interface{}{}
		if err := c.Bind(&reqBody); err != nil {
			return c.JSON(
				http.StatusBadRequest,
				map[string]string{"error": "Invalid request format"},
			)
		}

		if requestData, ok := reqBody["request"].(map[string]interface{}); ok {
			c.Set("request", requestData)
		} else {
			return c.JSON(http.StatusBadRequest, map[string]string{"error": "Missing 'request' field"})
		}

		return next(c)
	}
}
