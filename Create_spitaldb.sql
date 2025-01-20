USE [spitaldb]
GO
/****** Object:  Table [dbo].[Consultatie]    Script Date: 20/01/2025 03:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Consultatie](
	[ConsultID] [bigint] IDENTITY(1,1) NOT NULL,
	[MedicID] [bigint] NOT NULL,
	[PacientID] [bigint] NOT NULL,
	[MedicamentID] [bigint] NOT NULL,
	[Data] [date] NULL,
	[Diagnostic] [varchar](50) NULL,
	[DozaMedicament] [varchar](50) NULL,
 CONSTRAINT [PK_Consultatie] PRIMARY KEY CLUSTERED 
(
	[ConsultID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medic]    Script Date: 20/01/2025 03:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medic](
	[MedicID] [bigint] IDENTITY(1,1) NOT NULL,
	[NumeMedic] [varchar](50) NOT NULL,
	[PrenumeMedic] [varchar](50) NOT NULL,
	[Specializare] [varchar](50) NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK_Medic] PRIMARY KEY CLUSTERED 
(
	[MedicID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Medicamente]    Script Date: 20/01/2025 03:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Medicamente](
	[MedicamentID] [bigint] IDENTITY(1,1) NOT NULL,
	[Denumire] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Medicamente] PRIMARY KEY CLUSTERED 
(
	[MedicamentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pacient]    Script Date: 20/01/2025 03:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pacient](
	[PacientID] [bigint] IDENTITY(1,1) NOT NULL,
	[CNP] [varchar](13) NOT NULL,
	[NumePacient] [varchar](50) NOT NULL,
	[PrenumePacient] [varchar](50) NOT NULL,
	[Adresa] [varchar](50) NULL,
	[Asigurare] [varchar](50) NULL,
	[UserID] [bigint] NOT NULL,
 CONSTRAINT [PK_Pacient] PRIMARY KEY CLUSTERED 
(
	[PacientID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 20/01/2025 03:00:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[PasswordHash] [nvarchar](256) NOT NULL,
	[Role] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK__Users__1788CCAC4AD6751C] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ__Users__536C85E40FBDEEB5] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Consultatie]  WITH CHECK ADD  CONSTRAINT [FK_Consultatie_Medic] FOREIGN KEY([MedicID])
REFERENCES [dbo].[Medic] ([MedicID])
GO
ALTER TABLE [dbo].[Consultatie] CHECK CONSTRAINT [FK_Consultatie_Medic]
GO
ALTER TABLE [dbo].[Consultatie]  WITH CHECK ADD  CONSTRAINT [FK_Consultatie_Medicamente] FOREIGN KEY([MedicamentID])
REFERENCES [dbo].[Medicamente] ([MedicamentID])
GO
ALTER TABLE [dbo].[Consultatie] CHECK CONSTRAINT [FK_Consultatie_Medicamente]
GO
ALTER TABLE [dbo].[Consultatie]  WITH CHECK ADD  CONSTRAINT [FK_Consultatie_Pacient] FOREIGN KEY([PacientID])
REFERENCES [dbo].[Pacient] ([PacientID])
GO
ALTER TABLE [dbo].[Consultatie] CHECK CONSTRAINT [FK_Consultatie_Pacient]
GO
ALTER TABLE [dbo].[Medic]  WITH CHECK ADD  CONSTRAINT [FK_Medic_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Medic] CHECK CONSTRAINT [FK_Medic_UserID]
GO
ALTER TABLE [dbo].[Pacient]  WITH CHECK ADD  CONSTRAINT [FK_Pacient_UserID] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Pacient] CHECK CONSTRAINT [FK_Pacient_UserID]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [CK__Users__Role__7C4F7684] CHECK  (([Role]='Pacient' OR [Role]='User' OR [Role]='Admin'))
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [CK__Users__Role__7C4F7684]
GO
