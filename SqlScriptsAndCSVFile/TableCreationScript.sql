

---- CUSTOMER TABLE CREATION SCRIPT-----

USE [CustomerDetails]
GO

CREATE TABLE [dbo].[Customer](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](50) NULL,
	[LastName] [varchar](max) NULL,
	[Address] [varchar](max) NULL,
	[City] [varchar](max) NULL,
	[Province] [varchar](max) NULL,
	[Country] [varchar](max) NULL,
	[PostalCode] [varchar](max) NULL,
	[PhoneNumber] [varchar](max) NULL,
	[EmailAddress] [varchar](max) NULL,
	[StreetNumber] [varchar](max) NULL
)
GO