package accounts

import (
	"database/sql"

	"github.com/labstack/echo/v4"
)

type CreateAccountRequest struct {
	Username string `json:"username"`
	Password string `json:"password"`
}

type CreateAccountResponse struct {
	IsSuccess bool   `json:"isSuccess"`
	Message   string `json:"message"`
}

func CreateAccountHandler(db *sql.DB) echo.HandlerFunc {
	return func(c echo.Context) error {
		var req CreateAccountRequest

		err := c.Bind(&req)
		if err != nil {
			res := CreateAccountResponse{
				IsSuccess: false,
				Message:   "Invalid request.",
			}
			return c.JSON(400, res)
		}

		var count int
		err = db.QueryRow("SELECT COUNT(*) FROM accounts WHERE username = $1", req.Username).
			Scan(&count)
		if err != nil {
			res := CreateAccountResponse{
				IsSuccess: false,
				Message:   "Unable to create account.",
			}
			return c.JSON(500, res)
		}

		if count != 0 {
			response := CreateAccountResponse{
				IsSuccess: false,
				Message:   "Username already exists.",
			}
			return c.JSON(409, response)
		}

		_, err = db.Exec(
			"INSERT INTO accounts (username, password) VALUES ($1, $2)",
			req.Username,
			req.Password,
		)
		if err != nil {
			res := CreateAccountResponse{
				IsSuccess: false,
				Message:   "Unable to create account.",
			}
			return c.JSON(500, res)
		}

		res := CreateAccountResponse{
			IsSuccess: true,
			Message:   "Account created successfully.",
		}

		return c.JSON(201, res)
	}
}
