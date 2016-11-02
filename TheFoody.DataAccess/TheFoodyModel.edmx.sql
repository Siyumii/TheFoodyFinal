
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/22/2016 11:10:24
-- Generated from EDMX file: C:\Users\Dure\Documents\Visual Studio 2013\Projects\TheFoodyFinal\TheFoody.DataAccess\TheFoodyModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [TheFoody];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_Menu_Restaurant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Menu] DROP CONSTRAINT [FK_Menu_Restaurant];
GO
IF OBJECT_ID(N'[dbo].[FK_Restaurant_Type_Category]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Restaurant_Type] DROP CONSTRAINT [FK_Restaurant_Type_Category];
GO
IF OBJECT_ID(N'[dbo].[FK_Restaurant_Type_Restaurant]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Restaurant_Type] DROP CONSTRAINT [FK_Restaurant_Type_Restaurant];
GO
IF OBJECT_ID(N'[dbo].[FK_Restaurant_User]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Restaurant] DROP CONSTRAINT [FK_Restaurant_User];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Category]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Category];
GO
IF OBJECT_ID(N'[dbo].[Menu]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Menu];
GO
IF OBJECT_ID(N'[dbo].[Restaurant]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restaurant];
GO
IF OBJECT_ID(N'[dbo].[Restaurant_Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Restaurant_Type];
GO
IF OBJECT_ID(N'[dbo].[User]', 'U') IS NOT NULL
    DROP TABLE [dbo].[User];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Menus'
CREATE TABLE [dbo].[Menus] (
    [Menu_id] int IDENTITY(1,1) NOT NULL,
    [Menu_name] varchar(200)  NULL,
    [Description] varchar(1000)  NULL,
    [Price] decimal(6,2)  NULL,
    [Photo] varchar(200)  NULL,
    [Rest_id] int  NULL
);
GO

-- Creating table 'Users'
CREATE TABLE [dbo].[Users] (
    [email] varchar(200)  NOT NULL,
    [password] varchar(50)  NOT NULL,
    [fname] varchar(50)  NULL,
    [lname] varchar(100)  NOT NULL,
    [phone] char(10)  NULL,
    [photo] varchar(50)  NULL,
    [address] varchar(200)  NULL,
    [city] varchar(50)  NULL,
    [postcode] decimal(5,0)  NULL,
    [district] varchar(100)  NULL,
    [user_type] varchar(20)  NOT NULL,
    [status] varchar(20)  NOT NULL,
    [created_date] datetime  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [id] int IDENTITY(1,1) NOT NULL,
    [category1] varchar(50)  NULL
);
GO

-- Creating table 'Restaurants'
CREATE TABLE [dbo].[Restaurants] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OwnerEmail] varchar(200)  NOT NULL,
    [RestaurantName] varchar(100)  NOT NULL,
    [Logo] varchar(200)  NULL,
    [Address] varchar(100)  NOT NULL,
    [City] varchar(50)  NOT NULL,
    [District] varchar(100)  NOT NULL,
    [PostCode] decimal(5,0)  NOT NULL,
    [Website] varchar(200)  NULL,
    [Phone] varchar(50)  NOT NULL,
    [CompanyBackground] varchar(1000)  NULL,
    [OpeningTime] time  NOT NULL,
    [ClosingTime] time  NOT NULL,
    [DeliveryStartingTime] time  NOT NULL,
    [DeliveryEndingTime] time  NOT NULL,
    [TimetakentoDeliver] varchar(50)  NOT NULL
);
GO

-- Creating table 'Restaurant_Type'
CREATE TABLE [dbo].[Restaurant_Type] (
    [Type_id] int IDENTITY(1,1) NOT NULL,
    [Rest_id] int  NULL,
    [Category_id] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Menu_id] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [PK_Menus]
    PRIMARY KEY CLUSTERED ([Menu_id] ASC);
GO

-- Creating primary key on [email] in table 'Users'
ALTER TABLE [dbo].[Users]
ADD CONSTRAINT [PK_Users]
    PRIMARY KEY CLUSTERED ([email] ASC);
GO

-- Creating primary key on [id] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [Id] in table 'Restaurants'
ALTER TABLE [dbo].[Restaurants]
ADD CONSTRAINT [PK_Restaurants]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Type_id] in table 'Restaurant_Type'
ALTER TABLE [dbo].[Restaurant_Type]
ADD CONSTRAINT [PK_Restaurant_Type]
    PRIMARY KEY CLUSTERED ([Type_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Category_id] in table 'Restaurant_Type'
ALTER TABLE [dbo].[Restaurant_Type]
ADD CONSTRAINT [FK_Restaurant_Type_Category]
    FOREIGN KEY ([Category_id])
    REFERENCES [dbo].[Categories]
        ([id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Restaurant_Type_Category'
CREATE INDEX [IX_FK_Restaurant_Type_Category]
ON [dbo].[Restaurant_Type]
    ([Category_id]);
GO

-- Creating foreign key on [Menu_id] in table 'Menus'
ALTER TABLE [dbo].[Menus]
ADD CONSTRAINT [FK_Menu_Restaurant]
    FOREIGN KEY ([Menu_id])
    REFERENCES [dbo].[Restaurants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [Rest_id] in table 'Restaurant_Type'
ALTER TABLE [dbo].[Restaurant_Type]
ADD CONSTRAINT [FK_Restaurant_Type_Restaurant]
    FOREIGN KEY ([Rest_id])
    REFERENCES [dbo].[Restaurants]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Restaurant_Type_Restaurant'
CREATE INDEX [IX_FK_Restaurant_Type_Restaurant]
ON [dbo].[Restaurant_Type]
    ([Rest_id]);
GO

-- Creating foreign key on [OwnerEmail] in table 'Restaurants'
ALTER TABLE [dbo].[Restaurants]
ADD CONSTRAINT [FK_Restaurant_User]
    FOREIGN KEY ([OwnerEmail])
    REFERENCES [dbo].[Users]
        ([email])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Restaurant_User'
CREATE INDEX [IX_FK_Restaurant_User]
ON [dbo].[Restaurants]
    ([OwnerEmail]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------