USE SimpleBetting;
GO

-- Очистка существующих данных (если есть)
DELETE FROM UserBets;
DELETE FROM Bets;
DELETE FROM Users;
DELETE FROM Roles;

-- Сброс автоинкремента
DBCC CHECKIDENT ('Roles', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Bets', RESEED, 0);
DBCC CHECKIDENT ('UserBets', RESEED, 0);

-- Добавление ролей (если их нет)
IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'Admin')
    INSERT INTO Roles (RoleName) VALUES ('Admin');
IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'Editor')
    INSERT INTO Roles (RoleName) VALUES ('Editor');
IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'Director')
    INSERT INTO Roles (RoleName) VALUES ('Director');
IF NOT EXISTS (SELECT * FROM Roles WHERE RoleName = 'User')
    INSERT INTO Roles (RoleName) VALUES ('User');

-- Добавление тестовых пользователей
INSERT INTO Users (Username, PasswordHash, RoleID, Email, Balance, BetsCount) VALUES
('admin', 'admin123', 1, 'admin@test.com', 1000.00, 0),
('editor', 'editor123', 2, 'editor@test.com', 500.00, 0),
('director', 'director123', 3, 'director@test.com', 750.00, 0),
('user1', 'user123', 4, 'user1@test.com', 100.00, 0),
('user2', 'user123', 4, 'user2@test.com', 200.00, 0);

-- Добавление тестовых ставок (используем GETDATE() + дни)
INSERT INTO Bets (Team1, Team2, MatchTime, Sport, Description, Team1Win, Team2Win, Draw) VALUES
('Реал Мадрид', 'Барселона', DATEADD(day, 1, GETDATE()), 'Футбол', 'Эль Класико', 2.50, 2.80, 3.20),
('Манчестер Юнайтед', 'Ливерпуль', DATEADD(day, 2, GETDATE()), 'Футбол', 'Северо-западное дерби', 2.20, 3.10, 3.50),
('Бавария', 'Боруссия Д', DATEADD(day, 3, GETDATE()), 'Футбол', 'Классикер', 1.80, 4.20, 3.80),
('Лейкерс', 'Уорриорз', DATEADD(day, 4, GETDATE()), 'Баскетбол', 'NBA матч', 1.90, 1.90, NULL),
('Новак Джокович', 'Карлос Алькарас', DATEADD(day, 5, GETDATE()), 'Теннис', 'Финал турнира', 1.60, 2.40, NULL);

-- Добавление тестовых ставок пользователей (после создания ставок)
INSERT INTO UserBets (UserID, BetID, Amount, Coefficient, TeamWin, Status) VALUES
(4, 1, 50.00, 2.50, 'Реал Мадрид', 'Active'),
(4, 2, 30.00, 3.10, 'Ливерпуль', 'Active'),
(5, 1, 25.00, 2.50, 'Реал Мадрид', 'Active'),
(5, 3, 40.00, 1.80, 'Бавария', 'Active'),
(4, 4, 20.00, 1.90, 'Лейкерс', 'Active');

-- Обновление количества ставок у пользователей
UPDATE Users SET BetsCount = 3 WHERE UserID = 4;
UPDATE Users SET BetsCount = 2 WHERE UserID = 5;

PRINT 'Тестовые данные успешно добавлены!';
PRINT 'Пользователи: admin/admin123, editor/editor123, director/director123, user1/user123, user2/user123';

-- Получение количества записей для вывода
DECLARE @BetsCount INT = (SELECT COUNT(*) FROM Bets);
DECLARE @UserBetsCount INT = (SELECT COUNT(*) FROM UserBets);

PRINT 'Добавлено ставок: ' + CAST(@BetsCount AS VARCHAR(10));
PRINT 'Добавлено ставок пользователей: ' + CAST(@UserBetsCount AS VARCHAR(10)); 