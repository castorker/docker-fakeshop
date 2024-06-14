USE master
GO
DROP DATABASE IF EXISTS FakeStore
GO

CREATE DATABASE FakeStore
GO 
USE FakeStore
GO 
----------------------------------------------------------------------------
--- TABLE CREATION
----------------------------------------------------------------------------
CREATE TABLE [dbo].[Product](
	[Id] [INT] NOT NULL,
	[Name] [NVARCHAR](100) NOT NULL,
	[Description] [NVARCHAR](MAX) NULL,
	[Price] [NUMERIC](14, 2) NOT NULL,
	[Category] [NVARCHAR](30) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Order](
	[OrderId] [UNIQUEIDENTIFIER] NOT NULL,
	[CustomerId] [INT] NOT NULL,
	[ProductId] [INT] NOT NULL,
	[QuantityOrdered] [SMALLINT] NOT NULL,
	[OrderTotal] [NUMERIC](14, 2) NOT NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Inventory](
	[ProductId] [INT] NOT NULL,
	[QuantityOnHand] [INT] NOT NULL
) ON [PRIMARY]
GO

----------------------------------------------------------------------------
--- INITIAL DATA
----------------------------------------------------------------------------
INSERT INTO dbo.Product
( Id, Name, Description, Price, Category )
VALUES
( 1, 'Mountaineering', 'Perfect for rugged terrain adventures and alpine trekking.' ,200.00, 'boots' )
INSERT INTO dbo.Inventory ( ProductId, QuantityOnHand ) VALUES ( 1, 50 )

INSERT INTO dbo.Product
( Id, Name, Description, Price, Category )
VALUES
( 2, 'Leather', 'Hiking boots designed to go further.' ,180.00, 'boots' )
INSERT INTO dbo.Inventory ( ProductId, QuantityOnHand ) VALUES ( 2, 150 )

INSERT INTO dbo.Product
( Id, Name, Description, Price, Category )
VALUES
( 3, 'FutureLight', 'Optimized for hiking over uneven terrain and slippery rocks.' ,165.00, 'boots' )
INSERT INTO dbo.Inventory ( ProductId, QuantityOnHand ) VALUES ( 3, 70 )

INSERT INTO dbo.Product
( Id, Name, Description, Price, Category )
VALUES
( 4, 'Waterproof', 'They look like regular hiking boots, but the updated Chilkat Vs takes advantage of a waterproof and breathable construction to keep you dry.' ,140.00, 'boots' )
INSERT INTO dbo.Inventory ( ProductId, QuantityOnHand ) VALUES ( 4, 35 )

----------------------------------------------------------------------------
--- DB USER CREATION
----------------------------------------------------------------------------
USE master;
GO
CREATE LOGIN [cr_dbuser] WITH PASSWORD=N'Sequel#22DockerContainers!', CHECK_EXPIRATION=OFF, CHECK_POLICY=ON;
GO
USE FakeStore;
GO
CREATE USER [cr_dbuser] FOR LOGIN [cr_dbuser];
GO
EXEC sp_addrolemember N'db_owner', [cr_dbuser];
GO
