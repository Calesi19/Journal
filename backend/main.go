package main

import (
	"github.com/golang-jwt/jwt/v5"
	"github.com/jackc/pgx/v5/pgxpool"
	"github.com/labstack/echo/v4"
)

// Replace this with your secret key
var jwtSecret = []byte("your-secret-key")

func authMiddleware(next echo.HandlerFunc) echo.HandlerFunc {
	return func(c echo.Context) error {

		// Get the token from the Authorization header
		authHeader := c.Request().Header.Get("Authorization")
		if authHeader == "" {
			return c.String(401, "Unauthorized")
		}

		// Split the header into "Bearer" and the token
		tokenString := authHeader[len("Bearer "):]

		// Parse and validate the token
		token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
			// Ensure the token method conforms to expected method
			if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
				return nil, echo.NewHTTPError(401, "Unexpected signing method")
			}

			return jwtSecret, nil
		})

		if err != nil || !token.Valid {
			return c.String(401, "Invalid or expired token")
		}

		// Token is valid, proceed to the next handler
		return next(c)
	}
}

func login(c echo.Context) error {
	// Do some authentication stuff

	// Check username and password
	// If username and password is invalid, return error

	username := c.FormValue("username")
	password := c.FormValue("password")

	if username != "admin" || password != "admin" {
		return c.String(401, "Unauthorized")
	}

	// Create Jwt token
	// Return token

	return c.String(200, "token")
}

func main() {
	e := echo.New()

	e.Use(authMiddleware)

	e.GET("/", func(c echo.Context) error {
		return c.String(200, "Hello, World!")
	})

	e.Start(":8080")
}
