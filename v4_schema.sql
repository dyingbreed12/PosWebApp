USE PosDb;
GO

-------------------------------------------------------------------------------
-- Safely rename primary key columns to 'Id' across all tables
-- This script uses dynamic SQL to handle variable object names.
-------------------------------------------------------------------------------

-- Function to find the primary key constraint name
CREATE FUNCTION dbo.GetPrimaryKeyConstraintName (@TableName NVARCHAR(128))
RETURNS NVARCHAR(128)
AS
BEGIN
    DECLARE @ConstraintName NVARCHAR(128);
    SELECT @ConstraintName = name
    FROM sys.key_constraints
    WHERE type = 'PK' AND parent_object_id = OBJECT_ID(@TableName);
    RETURN @ConstraintName;
END;
GO

-- Inventory table
DECLARE @InventoryPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('Inventory');
DECLARE @sql NVARCHAR(MAX);
IF @InventoryPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE Inventory DROP CONSTRAINT ' + QUOTENAME(@InventoryPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'Inventory.ItemId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE Inventory ADD CONSTRAINT ' + QUOTENAME(@InventoryPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed Inventory.ItemId to Id and recreated PK.';
END
GO

-- SalesTransactions table
DECLARE @SalesTransactionsPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('SalesTransactions');
IF @SalesTransactionsPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE SalesTransactions DROP CONSTRAINT ' + QUOTENAME(@SalesTransactionsPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'SalesTransactions.TransactionId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE SalesTransactions ADD CONSTRAINT ' + QUOTENAME(@SalesTransactionsPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed SalesTransactions.TransactionId to Id and recreated PK.';
END
GO

-- TransactionItems table
DECLARE @TransactionItemsPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('TransactionItems');
IF @TransactionItemsPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE TransactionItems DROP CONSTRAINT ' + QUOTENAME(@TransactionItemsPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'TransactionItems.TransactionItemId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE TransactionItems ADD CONSTRAINT ' + QUOTENAME(@TransactionItemsPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed TransactionItems.TransactionItemId to Id and recreated PK.';
END
GO

-- Employees table
DECLARE @EmployeesPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('Employees');
IF @EmployeesPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE Employees DROP CONSTRAINT ' + QUOTENAME(@EmployeesPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'Employees.EmployeeId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE Employees ADD CONSTRAINT ' + QUOTENAME(@EmployeesPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed Employees.EmployeeId to Id and recreated PK.';
END
GO

-- Paychecks table
DECLARE @PaychecksPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('Paychecks');
IF @PaychecksPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE Paychecks DROP CONSTRAINT ' + QUOTENAME(@PaychecksPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'Paychecks.PaycheckId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE Paychecks ADD CONSTRAINT ' + QUOTENAME(@PaychecksPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed Paychecks.PaycheckId to Id and recreated PK.';
END
GO

-- Loans table
DECLARE @LoansPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('Loans');
IF @LoansPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE Loans DROP CONSTRAINT ' + QUOTENAME(@LoansPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'Loans.LoanId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE Loans ADD CONSTRAINT ' + QUOTENAME(@LoansPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed Loans.LoanId to Id and recreated PK.';
END
GO

-- Customers table
DECLARE @CustomersPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('Customers');
IF @CustomersPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE Customers DROP CONSTRAINT ' + QUOTENAME(@CustomersPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'Customers.CustomerId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE Customers ADD CONSTRAINT ' + QUOTENAME(@CustomersPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed Customers.CustomerId to Id and recreated PK.';
END
GO

-- TimeSheets table
DECLARE @TimeSheetsPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('TimeSheets');
IF @TimeSheetsPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE TimeSheets DROP CONSTRAINT ' + QUOTENAME(@TimeSheetsPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'TimeSheets.TimeSheetId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE TimeSheets ADD CONSTRAINT ' + QUOTENAME(@TimeSheetsPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed TimeSheets.TimeSheetId to Id and recreated PK.';
END
GO

-- PaycheckDetails table
DECLARE @PaycheckDetailsPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('PaycheckDetails');
IF @PaycheckDetailsPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE PaycheckDetails DROP CONSTRAINT ' + QUOTENAME(@PaycheckDetailsPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'PaycheckDetails.PaycheckDetailId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE PaycheckDetails ADD CONSTRAINT ' + QUOTENAME(@PaycheckDetailsPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed PaycheckDetails.PaycheckDetailId to Id and recreated PK.';
END
GO

-- LoanPayments table
DECLARE @LoanPaymentsPK NVARCHAR(128) = dbo.GetPrimaryKeyConstraintName('LoanPayments');
IF @LoanPaymentsPK IS NOT NULL
BEGIN
    SET @sql = 'ALTER TABLE LoanPayments DROP CONSTRAINT ' + QUOTENAME(@LoanPaymentsPK) + ';';
    EXEC sp_executesql @sql;
    EXEC sp_rename 'LoanPayments.PaymentId', 'Id', 'COLUMN';
    SET @sql = 'ALTER TABLE LoanPayments ADD CONSTRAINT ' + QUOTENAME(@LoanPaymentsPK) + ' PRIMARY KEY CLUSTERED (Id);';
    EXEC sp_executesql @sql;
    PRINT 'Renamed LoanPayments.PaymentId to Id and recreated PK.';
END
GO

-- Clean up the temporary function
DROP FUNCTION dbo.GetPrimaryKeyConstraintName;
GO