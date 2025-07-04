-- Создание базы данных
CREATE DATABASE SimpleBetting;
GO

USE SimpleBetting;
GO

-- Таблицы Users и Roles оставляем как есть
CREATE TABLE Roles (
    RoleID INT PRIMARY KEY IDENTITY(1,1),
    RoleName NVARCHAR(50) NOT NULL
);

CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(10) NOT NULL,
    PasswordHash NVARCHAR(64) NOT NULL,
    RoleID INT NOT NULL,
    Email NVARCHAR(50),
    CreatedDate DATETIME DEFAULT GETDATE(),
    Balance DECIMAL(10,2) DEFAULT 0.00, -- Добавим баланс прямо в Users для простоты
    BetsCount INT DEFAULT 0,
    FOREIGN KEY (RoleID) REFERENCES Roles(RoleID)
);

-- 2. Таблица ставок (коэффициентов)
CREATE TABLE Bets (
    BetID INT PRIMARY KEY IDENTITY(1,1),
    Team1 NVARCHAR(50) NOT NULL, -- Просто названия команд
    Team2 NVARCHAR(50) NOT NULL,
    MatchTime DATETIME NOT NULL,
    Sport NVARCHAR(50),
    Description NVARCHAR(50),
    Team1Win DECIMAL(5,2),
    Team2Win DECIMAL(5,2),
    Draw DECIMAL(5,2)
);

-- 3. Таблица ставок пользователей
CREATE TABLE UserBets (
    UserBetID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL,
    BetID INT NOT NULL,
    Amount DECIMAL(10,2) NOT NULL,
    DatePlaced DATETIME DEFAULT GETDATE(),
    Status NVARCHAR(20) DEFAULT 'Active', -- Active, Won, Lost
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    FOREIGN KEY (BetID) REFERENCES Bets(BetID)
);

-- Начальные роли (как в исходном коде)
INSERT INTO Roles (RoleName) VALUES ('Admin');
INSERT INTO Roles (RoleName) VALUES ('Editor');
INSERT INTO Roles (RoleName) VALUES ('Director');
INSERT INTO Roles (RoleName) VALUES ('User');
GO

-- Создание индексов для улучшения производительности
CREATE INDEX IX_Users_RoleID ON Users(RoleID);
CREATE INDEX IX_UserBets_UserID ON UserBets(UserID);
CREATE INDEX IX_UserBets_BetID ON UserBets(BetID);
CREATE INDEX IX_Bets_MatchTime ON Bets(MatchTime);
GO

PRINT 'База данных SimpleBetting успешно создана!'; 