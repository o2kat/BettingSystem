-- Скрипт для проверки содержимого архивного файла BetsArchive.csv
-- Выполните этот скрипт в SQL Server Management Studio

-- Проверка текущих данных в базе
SELECT 'Текущие данные в базе:' AS Info;
SELECT COUNT(*) AS BetsCount FROM Bets;
SELECT * FROM Bets;

-- Проверка файла архива (если он существует)
-- Примечание: SQL Server не может напрямую читать файлы, 
-- но вы можете открыть файл BetsArchive.csv в блокноте и проверить его содержимое

PRINT 'Для проверки архива откройте файл BetsArchive.csv в блокноте';
PRINT 'Файл должен содержать:';
PRINT '1. Заголовок: BetID,Team1,Team2,MatchTime,Sport,Description,Team1Win,Team2Win,Draw';
PRINT '2. Данные ставок (по одной на строку)';
PRINT '3. Поле Draw может быть пустым для некоторых ставок'; 