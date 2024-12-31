FROM golang:1.21 AS build

# Set environment variables
ENV CGO_ENABLED=0 \
    GOOS=linux \
    GOARCH=amd64

# Set the working directory inside the container
WORKDIR /app

# Copy go.mod and go.sum to download dependencies
COPY go.mod go.sum ./
RUN go mod download

# Copy the rest of the source code
COPY . .

# Build the Go app
RUN go build -o main ./

# Create a minimal image for running the Go app
FROM gcr.io/distroless/base-debian12

# Set the working directory in the final image
WORKDIR /root/

# Copy the compiled binary from the build stage
COPY --from=build /app/main .

# Command to run the application
CMD ["./main"]
