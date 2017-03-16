IF NOT EXISTS (SELECT TOP 1 * FROM dbo.Roles)
BEGIN

INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (-1, 'Denied')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (0, 'Deactive')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (25, 'ReadOnly')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (50, 'User')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (75, 'Manager')
INSERT INTO [dbo].[Roles] ([strength], [Detail]) VALUES (100, 'Admin')

END