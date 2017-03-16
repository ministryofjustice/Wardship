IF NOT EXISTS (SELECT TOP 1 * FROM dbo.Users)
BEGIN
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (1, N'matthewhearn', N'Matt', NULL, 100)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (2, N'mdhearn', N'Matt', NULL, 100)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (3, N'matthew.hearn@justice.gov.uk', N'Matt', NULL, 100)

SET IDENTITY_INSERT [dbo].[Users] OFF
END