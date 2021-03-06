CREATE TABLE [dbo].[Cat](
	[Id] [uniqueidentifier] NOT NULL,
	[Color] [varchar](100) NOT NULL,
	[FavoriteSound] [varchar](100) NOT NULL,
	[Gender] [varchar](15) NOT NULL,
 CONSTRAINT [PK_Cat] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]