USE SimpleBetting;
GO

-- Добавление тестовых пользователей
INSERT INTO Users (Username, PasswordHash, RoleID, Email, Balance, BetsCount) VALUES
('admin', 'admin123', 1, 'admin@test.com', 1000.00, 0),
('editor', 'editor123', 2, 'editor@test.com', 500.00, 0),
('director', 'director123', 3, 'director@test.com', 750.00, 0),
('user1', 'user123', 4, 'user1@test.com', 100.00, 0),
('user2', 'user123', 4, 'user2@test.com', 200.00, 0);

-- Добавление тестовых ставок (исправленные даты)
INSERT INTO Bets (Team1, Team2, MatchTime, Sport, Description, Team1Win, Team2Win, Draw) VALUES
('Реал Мадрид', 'Барселона', '2025-01-15 20:00:00', 'Футбол', 'Эль Класико', 2.50, 2.80, 3.20),
('Манчестер Юнайтед', 'Ливерпуль', '2025-01-16 21:00:00', 'Футбол', 'Северо-западное дерби', 2.20, 3.10, 3.50),
('Бавария', 'Боруссия Д', '2025-01-17 19:30:00', 'Футбол', 'Классикер', 1.80, 4.20, 3.80),
('Лейкерс', 'Уорриорз', '2025-01-18 22:00:00', 'Баскетбол', 'NBA матч', 1.90, 1.90, NULL),
('Новак Джокович', 'Карлос Алькарас', '2025-01-19 15:00:00', 'Теннис', 'Финал турнира', 1.60, 2.40, NULL);

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