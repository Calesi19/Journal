package main

import (
	"journal-api/internal/database"
	"journal-api/internal/middleware"
	"journal-api/internal/routers"
	"log"

	"github.com/labstack/echo/v4"
)

func main() {
	e := echo.New()

	e.Use(middleware.Test)

	// Initialize the database
	database.Initialize(
		"postgresql://postgres:wUmJPNNJmhUSxQvlnBupWsZArtpGtoZR@autorack.proxy.rlwy.net:40331/railway",
	)
	defer database.Close()

	routers.RegisterRoutes(e)

	// Start the server
	err := e.Start(":8080")
	if err != nil {
		log.Fatalf("Error starting server: %v", err)
	}
}
