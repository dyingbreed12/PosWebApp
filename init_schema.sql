CREATE DATABASE PosDb;

USE PosDb;

CREATE TABLE Users (
    Id INT IDENTITY PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL, -- 'Admin' or 'Counter'
    CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME(),
    IsActive BIT DEFAULT 1
);


-- Placeholder password hash (will be replaced from C#)
INSERT INTO Users (Username, PasswordHash, Role)
VALUES ('admin', '$2a$11$lDL3aqrmavQjM51s.DAIg.V4kGqwa/gIb.PSXFfJctcXOBlPa28pW', 'Admin');


-- Inventory table
CREATE TABLE Inventory (
    ItemId INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL DEFAULT 0,
    Price DECIMAL(18,2) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

INSERT INTO Users (Username, PasswordHash, Role)
VALUES  
       ('counter', '$2a$11$spH3VnoAm9KjHxbUYHJczOEhUvG7kWHw/3zDMUWc1A3eF6/jTVjAu', 'Counter');

-- SalesTransactions table
CREATE TABLE SalesTransactions (
    TransactionId INT IDENTITY PRIMARY KEY,
    TransactionDate DATETIME2 DEFAULT SYSUTCDATETIME(),
    TotalAmount DECIMAL(18, 2) NOT NULL,
    UserId INT,
    FOREIGN KEY (UserId) REFERENCES Users(Id)
);

-- TransactionItems table
CREATE TABLE TransactionItems (
    TransactionItemId INT IDENTITY PRIMARY KEY,
    TransactionId INT,
    ItemId INT,
    ItemName NVARCHAR(100) NOT NULL,
    Quantity INT NOT NULL,
    PricePerItem DECIMAL(18, 2) NOT NULL,
    FOREIGN KEY (TransactionId) REFERENCES SalesTransactions(TransactionId),
    FOREIGN KEY (ItemId) REFERENCES Inventory(ItemId)
);