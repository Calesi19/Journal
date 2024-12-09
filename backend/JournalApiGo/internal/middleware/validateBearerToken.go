package middleware

import (
	"github.com/golang-jwt/jwt/v5"
	"github.com/labstack/echo/v4"
)

func Test(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		return next(c)
	}
}

func AuthMiddleware(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		// Get the token from the Authorization header
		authHeader := c.Request().Header.Get("Authorization")

		if authHeader == "" {
			return c.String(401, "Unauthorized")
		}

		// Split the header into "Bearer" and the token
		tokenString := authHeader[len("Bearer "):]

		// Parse and validate the token
		token, err := parseToken(tokenString)

		if err != nil || !token.Valid {
			return c.String(401, "Invalid or expired token")
		}

		// Token is valid, proceed to the next handler
		return next(c)
	}
}

func parseToken(tokenString string) (*jwt.Token, error) {
	return jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		// Ensure the token method conforms to expected method
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, echo.NewHTTPError(401, "Unexpected signing method")
		}

		return []byte("secret"), nil
	})
}
