-- filename: update_schema_timestamps.sql
-- This script adds 'CreatedAt' and 'UpdatedAt' columns to all relevant tables.
-- It is designed to be idempotent and can be run multiple times without causing errors.

USE PosDb;
GO

-------------------------------------------------------------------------------
-- Add CreatedAt and UpdatedAt columns to tables
-------------------------------------------------------------------------------

-- SalesTransactions table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'SalesTransactions'))
BEGIN
    ALTER TABLE SalesTransactions ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to SalesTransactions.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'SalesTransactions'))
BEGIN
    ALTER TABLE SalesTransactions ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to SalesTransactions.';
END
GO

-- TransactionItems table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'TransactionItems'))
BEGIN
    ALTER TABLE TransactionItems ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to TransactionItems.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'TransactionItems'))
BEGIN
    ALTER TABLE TransactionItems ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to TransactionItems.';
END
GO

-- Employees table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'Employees'))
BEGIN
    ALTER TABLE Employees ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to Employees.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'Employees'))
BEGIN
    ALTER TABLE Employees ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to Employees.';
END
GO

-- Paychecks table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'Paychecks'))
BEGIN
    ALTER TABLE Paychecks ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to Paychecks.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'Paychecks'))
BEGIN
    ALTER TABLE Paychecks ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to Paychecks.';
END
GO

-- Loans table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'Loans'))
BEGIN
    ALTER TABLE Loans ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to Loans.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'Loans'))
BEGIN
    ALTER TABLE Loans ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to Loans.';
END
GO

-- Customers table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'Customers'))
BEGIN
    ALTER TABLE Customers ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to Customers.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'Customers'))
BEGIN
    ALTER TABLE Customers ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to Customers.';
END
GO

-- TimeSheets table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'TimeSheets'))
BEGIN
    ALTER TABLE TimeSheets ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to TimeSheets.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'TimeSheets'))
BEGIN
    ALTER TABLE TimeSheets ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to TimeSheets.';
END
GO

-- PaycheckDetails table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'PaycheckDetails'))
BEGIN
    ALTER TABLE PaycheckDetails ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to PaycheckDetails.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'PaycheckDetails'))
BEGIN
    ALTER TABLE PaycheckDetails ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to PaycheckDetails.';
END
GO

-- LoanPayments table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'LoanPayments'))
BEGIN
    ALTER TABLE LoanPayments ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to LoanPayments.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'LoanPayments'))
BEGIN
    ALTER TABLE LoanPayments ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to LoanPayments.';
END


-- LoanPayments table
IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'CreatedAt' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD CreatedAt DATETIME2 DEFAULT SYSUTCDATETIME();
    PRINT 'Added CreatedAt to Inventory.';
END

IF NOT EXISTS(SELECT * FROM sys.columns WHERE Name = N'UpdatedAt' AND Object_ID = OBJECT_ID(N'Inventory'))
BEGIN
    ALTER TABLE Inventory ADD UpdatedAt DATETIME2 NULL;
    PRINT 'Added UpdatedAt to Inventory.';
END
GO

PRINT 'All necessary timestamp columns have been added.';