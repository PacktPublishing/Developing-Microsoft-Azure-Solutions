-- Replace with MERGE
DELETE FROM [Wicked].[Favorite]
GO
DELETE FROM [Wicked].[Category]
GO
INSERT INTO [Wicked].[Category]
([Id], [Name])
VALUES
(1, 'Restaurants'),
(2, 'Internet'),
(3, 'Programming'),
(4, 'Hotel')
GO
INSERT INTO [Wicked].[Favorite]
([Name], [CategoryId], [ImageUrl], [Url])
VALUES
('Alma Nove', 1, null, 'https://www.almanovehingham.com/'),
('EF Core Power Tools', 3, null, 'https://marketplace.visualstudio.com/items?itemName=ErikEJ.EFCorePowerTools'),
('Verizon FIOS', 2, null, 'https://verizon-internet.com/'),
('AutoMapper', 3, null, 'https://automapper.org/'),
('Shouldly', 3, null, 'https://shouldly.io/'),
('SnagIt', 3, null, 'https://www.techsmith.com/screen-capture.html'),
('TaskBarX', 3, null, 'https://github.com/ChrisAnd1998/TaskbarX'),
('Residence Inn Portland Maine', 4, null, 'https://www.marriott.com/hotels/travel/pwmdt-reside'),
('ZZZ Projects', 3, null, 'https://zzzprojects.com/'),
('Gibbet Hill Grill', 1, null, 'https://www.gibbethillgrill.com/'),
('Filho Cucina', 1, null, 'https://www.filhoscucina.com/')

GO

