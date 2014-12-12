
/****** Object:  Table [dbo].[legacyAcProduct]    Script Date: 07/08/2014 10:11:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[legacyAcProduct](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[acProductId] [int] NOT NULL,
	[acProductVariantId] [int] NULL,
	[acOption1] [int] NULL,
	[productKey] [uniqueidentifier] NOT NULL,
	[productVariantKey] [uniqueidentifier] NOT NULL,
	[acUrl] [nvarchar](255) NULL
) ON [PRIMARY]

GO
