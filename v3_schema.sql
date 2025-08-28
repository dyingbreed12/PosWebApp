-- Use the existing database
USE PosDb;
GO

-- 1. Refined Inventory table with a Unique SKU
-- The SKU is crucial for efficient barcoding and lookups.
-- We'll also add cost price to calculate profits.
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'SKU' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD SKU NVARCHAR(50) UNIQUE;
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CostPrice' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD CostPrice DECIMAL(18,2) NOT NULL DEFAULT 0;
END

-- 2. Enhanced SalesTransactions Table
-- A customer table is essential for lending and better reporting.
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'Customers') AND type in (N'U'))
BEGIN
CREATE TABLE Customers (
    CustomerId INT IDENTITY PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100),
    PhoneNumber NVARCHAR(20),
    Email NVARCHAR(100)
);
END

-- Add CustomerId and a column for tracking the sale's profit.
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CustomerId' AND Object_ID = OBJECT_ID(N'SalesTransactions'))
BEGIN
    ALTER TABLE SalesTransactions ADD CustomerId INT NULL FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId);
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'Profit' AND Object_ID = OBJECT_ID(N'SalesTransactions'))
BEGIN
    ALTER TABLE SalesTransactions ADD Profit DECIMAL(18,2) NOT NULL DEFAULT 0;
END

-- 3. Advanced Payroll Management
-- We'll add a timesheet table to enable hourly pay and more accurate payroll.
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'TimeSheets') AND type in (N'U'))
BEGIN
CREATE TABLE TimeSheets (
    TimeSheetId INT IDENTITY PRIMARY KEY,
    EmployeeId INT NOT NULL FOREIGN KEY (EmployeeId) REFERENCES Employees(EmployeeId),
    WorkDate DATE NOT NULL,
    HoursWorked DECIMAL(5,2) NOT NULL,
    Notes NVARCHAR(255)
);
END

-- We'll also refine the Paychecks table to better track deductions.
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'PaycheckDetails') AND type in (N'U'))
BEGIN
CREATE TABLE PaycheckDetails (
    PaycheckDetailId INT IDENTITY PRIMARY KEY,
    PaycheckId INT NOT NULL FOREIGN KEY (PaycheckId) REFERENCES Paychecks(PaycheckId),
    Description NVARCHAR(100),
    Amount DECIMAL(18,2) NOT NULL,
    IsDeduction BIT NOT NULL
);
END

-- 4. Complete Lending System
-- Add a dedicated table for tracking payments on loans.
IF NOT EXISTS(SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'LoanPayments') AND type in (N'U'))
BEGIN
CREATE TABLE LoanPayments (
    PaymentId INT IDENTITY PRIMARY KEY,
    LoanId INT NOT NULL FOREIGN KEY (LoanId) REFERENCES Loans(LoanId),
    PaymentDate DATETIME2 DEFAULT SYSUTCDATETIME(),
    Amount DECIMAL(18,2) NOT NULL,
    Notes NVARCHAR(255)
);
END

PRINT 'Refined database schema updated successfully.';
GO