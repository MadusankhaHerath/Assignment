# Enhanzer Purchase Bill – Angular & .NET Core

A 2-page web app: **Login** and **Purchase Bill**.

- **Frontend:** Angular 21, TypeScript, Reactive Forms, Signals
- **Backend:** ASP.NET Core 8 Web API, Entity Framework Core, JWT
- **Database:** SQL Server

## Features

**Login**
- Email and password validation with error messages
- Calls the Enhanzer API (`GetLoginData`) through the .NET backend
- On success, saves `User_Locations` to the `Location_Details` table
- Returns a JWT stored in `sessionStorage`
- Route guard blocks the Purchase Bill page until the user logs in. A 401 response logs the user out.

**Purchase Bill**
- Item autocomplete (Mango, Apple, Banana, Orange, Grapes, Kiwi, Strawberry)
- Batch dropdown filled from `Location_Details`
- Calculated fields:
  - Margin = Standard Price − Standard Cost
  - Total Cost = (Standard Cost × Qty) − Discount %
  - Total Selling = Standard Price × Qty
- **Add** puts the item in the table and updates the Item Summary (Total Items, Total Qty)
- **Save** sends the bill to the API, which stores it in `Purchase_Bill` / `Purchase_Bill_Item`
- Loading indicators and error messages

## Project Structure

```
backend/                 ASP.NET Core 8 Web API
  Controllers/           Auth, Locations, PurchaseBills
  Data/                  AppDbContext
  Dtos/                  Request/response models
  Models/                Database entities
  Services/              Enhanzer API client, JWT, locations, purchase bills
database/
  EnhanzerDb.sql         SQL Server script
frontend/                Angular app
  src/app/core/          models, services, guard, interceptor
  src/app/shared/        loading spinner
  src/app/features/
    login/
    purchase-bill/       page + item-table + item-summary components
```

## Setup

### Requirements
- .NET 8 SDK
- Node.js 20.19+ or 22.12+
- SQL Server (local install, or Docker on macOS/Linux)

### 1. Database

Start SQL Server. On macOS/Linux you can use Docker:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong@Passw0rd" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest
```

Then run `database/EnhanzerDb.sql`. The backend also creates the database on startup if it does not exist.

Set the connection string in `backend/appsettings.json` if yours is different:

```json
"Default": "Server=localhost,1433;Database=EnhanzerDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
```

On Windows with a local instance you can use:
`Server=localhost;Database=EnhanzerDb;Trusted_Connection=True;TrustServerCertificate=True`

### 2. Backend

```bash
cd backend
dotnet run
```

The API runs at `http://localhost:5047`. Swagger is at `http://localhost:5047/swagger`.

### 3. Frontend

```bash
cd frontend
npm install
npm start
```

Open `http://localhost:4200`.

Test login: `info@enhanzer.com` / `Welcome#5`

## API Endpoints

| Method | URL                   | Auth | Description                                         |
|--------|-----------------------|------|-----------------------------------------------------|
| POST   | `/api/auth/login`     | No   | Log in through the Enhanzer API, save locations, return a JWT |
| GET    | `/api/locations`      | Yes  | Get saved locations (batch dropdown)                |
| POST   | `/api/purchase-bills` | Yes  | Save a purchase bill with its items                 |
