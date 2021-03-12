USE [MinesweeperWeb]
GO
/****** Object:  Table [dbo].[tbl_User]    Script Date: 3/12/2021 12:11:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbl_User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Gender] [nvarchar](50) NOT NULL,
	[Age] [nvarchar](50) NOT NULL,
	[EmailAddress] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[State] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[usp_GrabUserByID]    Script Date: 3/12/2021 12:11:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_GrabUserByID]
	@UserID int = 0
AS
BEGIN
	SELECT * 
	FROM tbl_User
	WHERE UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[usp_InsertUser]    Script Date: 3/12/2021 12:11:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_InsertUser]
	@FirstName nvarchar(50) = '',
	@LastName nvarchar(50) = '',
	@Gender nvarchar(50) = '',
	@Age nvarchar(50) = '',
	@Email nvarchar(50) = '',
	@Username nvarchar(50) = '',
	@Password nvarchar(50) = '',
	@State nvarchar(50) = '' 
AS
BEGIN
INSERT INTO tbl_User (FirstName, LastName, Gender, Age,
	EmailAddress, Username, Password, State) VALUES(@FirstName, @LastName, @Gender, @Age, @Email, @Username, @Password, @State)
END
GO
/****** Object:  StoredProcedure [dbo].[usp_LoginUser]    Script Date: 3/12/2021 12:11:23 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_LoginUser]
	@Username nvarchar(50) = '',
	@Password nvarchar(50) = ''
AS
BEGIN
	SELECT * 
	FROM tbl_User
	WHERE Username = @Username
	AND Password = @Password
END
GO
