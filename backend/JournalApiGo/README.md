# Journal API

Allows users to create, read, update and delete journal entries and accounts. Meant to be used as a backend for a journaling app.

## Built With

- Go : for source code
- PostgreSQL : database to store journal entries.
- JWT: for authentication.

## Endpoints

### Create a new account

```
POST /accounts
```

Request body:

```
{
"request":{
    "username": "username",
    "password": "password"
  }
}
```
