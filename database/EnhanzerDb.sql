IF DB_ID('EnhanzerDb') IS NULL
    CREATE DATABASE EnhanzerDb;
GO

USE EnhanzerDb;
GO

IF OBJECT_ID('dbo.Location_Details') IS NULL
CREATE TABLE dbo.Location_Details (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Location_Code NVARCHAR(50)  NOT NULL,
    Location_Name NVARCHAR(200) NOT NULL,
    CONSTRAINT UQ_Location_Details_Code UNIQUE (Location_Code)
);
GO

IF OBJECT_ID('dbo.Purchase_Bill') IS NULL
CREATE TABLE dbo.Purchase_Bill (
    Id            INT IDENTITY(1,1) PRIMARY KEY,
    Created_At    DATETIME2      NOT NULL,
    Created_By    NVARCHAR(200)  NOT NULL,
    Total_Items   INT            NOT NULL,
    Total_Qty     DECIMAL(18,2)  NOT NULL,
    Total_Cost    DECIMAL(18,2)  NOT NULL,
    Total_Selling DECIMAL(18,2)  NOT NULL
);
GO

IF OBJECT_ID('dbo.Purchase_Bill_Item') IS NULL
CREATE TABLE dbo.Purchase_Bill_Item (
    Id               INT IDENTITY(1,1) PRIMARY KEY,
    Purchase_Bill_Id INT            NOT NULL,
    Item             NVARCHAR(100)  NOT NULL,
    Batch            NVARCHAR(200)  NOT NULL,
    Standard_Cost    DECIMAL(18,2)  NOT NULL,
    Standard_Price   DECIMAL(18,2)  NOT NULL,
    Margin           DECIMAL(18,2)  NOT NULL,
    Qty              DECIMAL(18,2)  NOT NULL,
    Free_Qty         DECIMAL(18,2)  NOT NULL,
    Discount         DECIMAL(18,2)  NOT NULL,
    Total_Cost       DECIMAL(18,2)  NOT NULL,
    Total_Selling    DECIMAL(18,2)  NOT NULL,
    CONSTRAINT FK_Purchase_Bill_Item_Bill FOREIGN KEY (Purchase_Bill_Id)
        REFERENCES dbo.Purchase_Bill(Id) ON DELETE CASCADE
);
GO
