package posts

import (
	"net/http"

	"github.com/labstack/echo/v4"
)

func UpdatePostHandler(c echo.Context) error {
	return c.String(http.StatusOK, "GetPostsHandler")
}
