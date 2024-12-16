package auth

import (
	"github.com/golang-jwt/jwt"
	"github.com/labstack/echo/v4"
)

type RefreshTokenRequest struct {
	RefreshToken string `json:"refreshToken"`
}

type RefreshTokenResponse struct {
	IsSuccess    bool   `json:"isSuccess"`
	BearerToken  string `json:"bearerToken"`
	RefreshToken string `json:"refreshToken"`
}

func RefreshTokenHandler() echo.HandlerFunc {
	return func(c echo.Context) error {
		var req RefreshTokenRequest
		var user User

		err := c.Bind(&req)
		if err != nil {
			return c.JSON(400, map[string]string{
				"message": "Invalid request.",
			})
		}

		// Decode the refresh token
		claims := jwt.MapClaims{}
		_, err = jwt.ParseWithClaims(req.RefreshToken, claims, func(token *jwt.Token) (interface{}, error) {
			return []byte(secret), nil
		})

		if err != nil {
			return c.JSON(401, map[string]string{
				"message": "Invalid refresh token.",
			})
		}

		user.Id = claims["id"].(string)
		user.Email = claims["email"].(string)

		bearerToken, err := generateBearerToken(user)

		if err != nil {
			return c.JSON(500, map[string]string{
				"message": "Error occurred while generating the bearer token.",
			})
		}

		refreshToken, err := generateRefreshToken(user)

		if err != nil {
			return c.JSON(500, map[string]string{
				"message": "Error occurred while generating the refresh token.",
			})
		}

		res := LoginResponse{
			IsSuccess:    true,
			BearerToken:  bearerToken,
			RefreshToken: refreshToken,
		}

		return c.JSON(200, res)
	}
}
