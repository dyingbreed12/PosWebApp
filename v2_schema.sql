USE PosDb;
GO

-- 1. ALTER EXISTING TABLES TO ADD NEW COLUMNS

-- Add new columns to the Inventory table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Description' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD Description NVARCHAR(500);
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'SKU' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD SKU NVARCHAR(50) UNIQUE;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'ReorderLevel' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD ReorderLevel INT NOT NULL DEFAULT 10;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PurchasePrice' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD PurchasePrice DECIMAL(18,2) NOT NULL DEFAULT 0;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'SellingPrice' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    -- Rename the existing 'Price' column to 'SellingPrice'
    EXEC sp_rename 'Inventory.Price', 'SellingPrice', 'COLUMN';
END
ELSE
BEGIN
    -- If 'SellingPrice' already exists, you don't need to rename.
    -- This handles cases where you might have run part of the script before.
    PRINT 'SellingPrice column already exists.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'IsActive' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD IsActive BIT DEFAULT 1;
END

-- Add new columns to the SalesTransactions table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Discount' AND Object_ID = OBJECT_ID(N'SalesTransactions'))
BEGIN
    ALTER TABLE SalesTransactions ADD Discount DECIMAL(18,2) DEFAULT 0;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'PaymentMethod' AND Object_ID = OBJECT_ID(N'SalesTransactions'))
BEGIN
    ALTER TABLE SalesTransactions ADD PaymentMethod NVARCHAR(50);
END

-- 2. CREATE NEW TABLES (Payroll and Lending)

-- Create the Employees table if it does not exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Employees') AND type in (N'U'))
BEGIN
CREATE TABLE Employees (
    EmployeeId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    Position NVARCHAR(100),
    Salary DECIMAL(18,2) NOT NULL,
    IsActive BIT DEFAULT 1
);
END

-- Create the Paychecks table if it does not exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Paychecks') AND type in (N'U'))
BEGIN
CREATE TABLE Paychecks (
    PaycheckId INT IDENTITY PRIMARY KEY,
    EmployeeId INT NOT NULL,
    PayPeriodStart DATE NOT NULL,
    PayPeriodEnd DATE NOT NULL,
    GrossPay DECIMAL(18,2) NOT NULL,
    Deductions DECIMAL(18,2) NOT NULL DEFAULT 0,
    NetPay DECIMAL(18,2) NOT NULL,
    PayDate DATE NOT NULL,
    FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId)
);
END

-- Create the Loans table if it does not exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Loans') AND type in (N'U'))
BEGIN
CREATE TABLE Loans (
    LoanId INT IDENTITY PRIMARY KEY,
    CustomerId INT, -- Optional: Can be linked to a future Customers table
    LoanDate DATETIME2 DEFAULT SYSUTCDATETIME(),
    DueDate DATE NOT NULL,
    TotalAmount DECIMAL(18,2) NOT NULL,
    Balance DECIMAL(18,2) NOT NULL,
    Status NVARCHAR(50) NOT NULL, -- 'Active', 'Paid', 'Overdue'
    Notes NVARCHAR(500)
);
END

PRINT 'Database schema updated successfully.';
GO