# Решение проблемы с SQL скриптами

## Проблема
При выполнении `test_data.sql` возникают ошибки:
1. "Преобразование типа данных varchar в datetime привело к выходу значения за пределы диапазона"
2. "Конфликт инструкции INSERT с ограничением FOREIGN KEY"

## Решение

### Шаг 1: Проверьте базу данных
Выполните скрипт `check_database.sql` для проверки состояния базы данных:
```sql
-- В SQL Server Management Studio выполните:
check_database.sql
```

### Шаг 2: Очистите базу данных (если нужно)
Если в базе есть старые данные, выполните:
```sql
USE SimpleBetting;
GO

-- Очистка данных
DELETE FROM UserBets;
DELETE FROM Bets;
DELETE FROM Users;
DELETE FROM Roles;

-- Сброс автоинкремента
DBCC CHECKIDENT ('Roles', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Bets', RESEED, 0);
DBCC CHECKIDENT ('UserBets', RESEED, 0);
```

### Шаг 3: Выполните исправленный скрипт
**Используйте `test_data_simple_fixed.sql`** (самый простой и надежный):
```sql
-- В SQL Server Management Studio выполните:
test_data_simple_fixed.sql
```

**Альтернативно**: Можно использовать `test_data_fixed.sql` (исправленная версия с переменными).

### Шаг 4: Проверьте результат
Снова выполните `check_database.sql` для проверки:
```sql
check_database.sql
```

## Ожидаемый результат

После выполнения `test_data_simple_fixed.sql` вы должны увидеть:
```
Тестовые данные успешно добавлены!
Логины: admin/admin123, editor/editor123, director/director123, user1/user123, user2/user123
```

## Почему возникала ошибка

1. **Ошибка с датами**: В старом `test_data.sql` использовались фиксированные даты в формате, который не поддерживается вашей версией SQL Server
2. **Ошибка FOREIGN KEY**: Попытка вставить записи в `UserBets` до создания записей в `Bets`

## Исправления в test_data_simple_fixed.sql

1. ✅ **Динамические даты**: Использование `DATEADD(day, N, GETDATE())` вместо фиксированных дат
2. ✅ **Правильный порядок**: Сначала создаются роли, потом пользователи, потом ставки, потом ставки пользователей
3. ✅ **Проверка ролей**: Добавлена проверка существования ролей перед вставкой
4. ✅ **Очистка данных**: Автоматическая очистка старых данных
5. ✅ **Сброс автоинкремента**: Корректная работа с ID

## Тестовые аккаунты

После успешного выполнения скрипта используйте:

| Логин | Пароль | Роль |
|-------|--------|------|
| admin | admin123 | Администратор |
| editor | editor123 | Редактор |
| director | director123 | Директор |
| user1 | user123 | Пользователь |
| user2 | user123 | Пользователь |

## Если проблема остается

1. **Проверьте версию SQL Server**: Убедитесь, что используется SQL Server 2012 или выше
2. **Проверьте права доступа**: Убедитесь, что у пользователя есть права на создание и изменение данных
3. **Проверьте строку подключения**: Убедитесь, что приложение подключается к правильной базе данных
4. **Очистите кэш**: В Visual Studio выполните Clean Solution и Rebuild Solution 