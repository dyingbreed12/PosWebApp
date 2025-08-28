# PosWebApp

A web-based Point of Sale (POS) system built on the .NET 8 Framework. This project is designed with a clean, layered architecture to separate business logic from the presentation and data layers, ensuring reusability and maintainability.

## Technology Stack

* **Backend:** ASP.NET Core 8, C#, Dapper
* **Database:** SQL Server
* **Frontend:** Razor Pages, Bootstrap
* **Architecture:** Layered Architecture (Common, BusinessLogic, Presentation)

## Features

-   User Authentication and Authorization (Admin roles)
-   Inventory Management (Add, Update, Delete products)
-   Sales and Transaction Processing
-   Dashboard with key business metrics (Total Sales, Total Transactions)

## Getting Started

### Prerequisites

* .NET 8 SDK
* SQL Server
* Visual Studio or Visual Studio Code

### Installation

1.  **Clone the repository:**
    `git clone https://github.com/your-username/PosWebApp.git`
2.  **Navigate to the project directory:**
    `cd PosWebApp`
3.  **Update the database connection string:**
    Open `appsettings.json` in the `PosWebApp` project and update the `DefaultConnection` string to point to your SQL Server instance.
4.  **Run the database schema script:**
    Execute the `sql/init_schema.sql` script on your SQL Server to create the necessary tables.
5.  **Run the application:**
    `dotnet run`

## Project Structure

* `PosWebApp/`: The main ASP.NET Core project, acting as the presentation layer. It contains the controllers, views, and view models.
* `PosWebApp.BusinessLogic/`: Contains the core business logic, services, and repository implementations.
* `PosWebApp.Common/`: Holds shared models and interfaces used across the application layers.
* `sql/`: Contains the database schema script.