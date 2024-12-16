package middleware

import (
	"net/http"
	"strings"

	"github.com/golang-jwt/jwt/v5"
	"github.com/labstack/echo/v4"
)

const secret = "your-secret-key" // Use the same key as in LoginHandler

func Test(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		return next(c)
	}
}

func AuthMiddleware(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {
		authHeader := c.Request().Header.Get("Authorization")

		if authHeader == "" {
			return c.String(http.StatusUnauthorized, "Missing Authorization header")
		}

		tokenString := strings.TrimPrefix(authHeader, "Bearer ")

		token, err := parseToken(tokenString)
		if err != nil || !token.Valid {
			return c.String(http.StatusUnauthorized, "Invalid or expired token")
		}

		claims, ok := token.Claims.(jwt.MapClaims)
		if !ok {
			return c.String(http.StatusUnauthorized, "Invalid token claims")
		}

		userId, ok := claims["userId"].(string)
		if !ok || userId == "" {
			return c.String(http.StatusUnauthorized, "userId not found in token")
		}

		// Store userId in the context
		c.Set("userId", userId)

		return next(c)
	}
}

func parseToken(tokenString string) (*jwt.Token, error) {
	return jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, echo.NewHTTPError(401, "Unexpected signing method")
		}

		return []byte(secret), nil
	})
}
