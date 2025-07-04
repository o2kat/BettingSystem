USE SimpleBetting;
GO

PRINT '=== ПРОВЕРКА БАЗЫ ДАННЫХ SIMPLEBETTING ===';
PRINT '';

-- Проверка таблиц
PRINT '1. Проверка существования таблиц:';
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Roles')
    PRINT '   ✓ Таблица Roles существует';
ELSE
    PRINT '   ✗ Таблица Roles НЕ существует';

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Users')
    PRINT '   ✓ Таблица Users существует';
ELSE
    PRINT '   ✗ Таблица Users НЕ существует';

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Bets')
    PRINT '   ✓ Таблица Bets существует';
ELSE
    PRINT '   ✗ Таблица Bets НЕ существует';

IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'UserBets')
    PRINT '   ✓ Таблица UserBets существует';
ELSE
    PRINT '   ✗ Таблица UserBets НЕ существует';

PRINT '';

-- Проверка данных
PRINT '2. Количество записей в таблицах:';
DECLARE @RolesCount INT = (SELECT COUNT(*) FROM Roles);
DECLARE @UsersCount INT = (SELECT COUNT(*) FROM Users);
DECLARE @BetsCount INT = (SELECT COUNT(*) FROM Bets);
DECLARE @UserBetsCount INT = (SELECT COUNT(*) FROM UserBets);

PRINT '   Roles: ' + CAST(@RolesCount AS VARCHAR(10));
PRINT '   Users: ' + CAST(@UsersCount AS VARCHAR(10));
PRINT '   Bets: ' + CAST(@BetsCount AS VARCHAR(10));
PRINT '   UserBets: ' + CAST(@UserBetsCount AS VARCHAR(10));

PRINT '';

-- Проверка ролей
PRINT '3. Роли в системе:';
SELECT RoleID, RoleName FROM Roles ORDER BY RoleID;

PRINT '';

-- Проверка пользователей
PRINT '4. Пользователи в системе:';
SELECT UserID, Username, Email, RoleID, Balance, BetsCount FROM Users ORDER BY UserID;

PRINT '';

-- Проверка ставок
PRINT '5. Ставки в системе:';
SELECT BetID, Team1, Team2, Sport, Team1Win, Team2Win, Draw FROM Bets ORDER BY BetID;

PRINT '';

-- Проверка ставок пользователей
PRINT '6. Ставки пользователей:';
SELECT UserBetID, UserID, BetID, Amount, Coefficient, TeamWin, Status FROM UserBets ORDER BY UserBetID;

PRINT '';
PRINT '=== ПРОВЕРКА ЗАВЕРШЕНА ==='; 