IF NOT EXISTS (SELECT TOP 1 * FROM dbo.Users)
BEGIN
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (1, N'carlosfernandez', N'Carlos local', NULL, 100)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (2, N'eab10e', N'Carlos dom1', NULL, 100)
INSERT INTO [dbo].[Users] ([UserID], [Name], [DisplayName], [LastActive], [RoleStrength]) VALUES (3, N'carlos.fernandez@justice.gov.uk', N'Carlos JUSTICE', NULL, 100)

SET IDENTITY_INSERT [dbo].[Users] OFF
END