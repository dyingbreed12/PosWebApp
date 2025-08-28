-- Drop the existing database if it exists to start fresh
IF DB_ID('PosDb') IS NOT NULL
BEGIN
    ALTER DATABASE PosDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE PosDb;
END
GO

-- Create the new database
CREATE DATABASE PosDb;
GO

USE PosDb;
GO

-------------------------------------------------------------------------------
-- Consolidated and Final Database Schema
-------------------------------------------------------------------------------

-- Users table
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

-- Customers table
CREATE TABLE Customers (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

-- Employees table
CREATE TABLE Employees (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100),
    Salary DECIMAL(18,2) NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

-- Inventory table
CREATE TABLE Inventory (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    SellingPrice DECIMAL(18,2) NOT NULL,
    Description NVARCHAR(500),
    SKU NVARCHAR(50) UNIQUE,
    ReorderLevel INT NOT NULL DEFAULT 10,
    CostPrice DECIMAL(18,2) NOT NULL DEFAULT 0,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL
);
GO

-- SalesTransactions table
CREATE TABLE SalesTransactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TransactionDate DATETIME2 DEFAULT SYSUTCDATETIME(),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    Discount DECIMAL(18,2) DEFAULT 0,
    PaymentMethod NVARCHAR(50),
    Profit DECIMAL(18,2) NOT NULL DEFAULT 0,
    CustomerId INT,
    UserId INT,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (UserId) REFERENCES Users(Id),
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

-- TransactionItems table
CREATE TABLE TransactionItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TransactionId INT NOT NULL,
    ItemId INT NOT NULL,
    ItemName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    PricePerItem DECIMAL(18, 2) NOT NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (TransactionId) REFERENCES SalesTransactions(Id),
    FOREIGN KEY (ItemId) REFERENCES Inventory(Id)
);
GO

-- Paychecks table
CREATE TABLE Paychecks (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    PayPeriodStart DATE NOT NULL,
    PayPeriodEnd DATE NOT NULL,
    GrossPay DECIMAL(18,2) NOT NULL,
    Deductions DECIMAL(18,2) NOT NULL DEFAULT 0,
    NetPay DECIMAL(18,2) NOT NULL,
    PayDate DATE NOT NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);
GO

-- Loans table
CREATE TABLE Loans (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT,
    LoanDate DATETIME2 DEFAULT SYSUTCDATETIME(),
    DueDate DATE NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    Notes NVARCHAR(500),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
);
GO

-- TimeSheets table
CREATE TABLE TimeSheets (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    EmployeeId INT NOT NULL,
    WorkDate DATE NOT NULL,
    HoursWorked DECIMAL(5,2) NOT NULL,
    Notes NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(Id)
);
GO

-- PaycheckDetails table
CREATE TABLE PaycheckDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    PaycheckId INT NOT NULL,
    Description NVARCHAR(100),
    Amount DECIMAL(18,2) NOT NULL,
    IsDeduction BIT NOT NULL,
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (PaycheckId) REFERENCES Paychecks(Id)
);
GO

-- LoanPayments table
CREATE TABLE LoanPayments (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    LoanId INT NOT NULL,
    PaymentDate DATETIME2 DEFAULT SYSUTCDATETIME(),
    Amount DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(255),
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    UpdatedAt DATETIME2 NULL,
    FOREIGN KEY (LoanId) REFERENCES Loans(Id)
);
GO

-- Insert initial data
INSERT INTO Users (Username, PasswordHash, Role)
VALUES 
    ('admin', '$2a$11$lDL3aqrmavQjM51s.DAIg.V4kGqwa/gIb.PSXFfJctcXOBlPa28pW', 'Admin'),
    ('counter', '$2a$11$spH3VnoAm9KjHxbUYHJczOEhUvG7kWHw/3zDMUWc1A3eF6/jTVjAu', 'Counter');
GO

PRINT 'Final database schema created successfully. All tables and constraints are in place.';