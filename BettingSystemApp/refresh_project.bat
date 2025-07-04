@echo off
echo Обновление проекта BettingSystemApp...

REM Удаляем временные файлы
if exist "BettingSystemApp\obj" rmdir /s /q "BettingSystemApp\obj"
if exist "BettingSystemApp\bin" rmdir /s /q "BettingSystemApp\bin"

REM Удаляем файлы .suo и .user если они существуют
if exist "BettingSystemApp.suo" del "BettingSystemApp.suo"
if exist "BettingSystemApp\BettingSystemApp.csproj.user" del "BettingSystemApp\BettingSystemApp.csproj.user"

echo Временные файлы удалены.
echo Теперь откройте проект в Visual Studio и выполните:
echo 1. Build -> Clean Solution
echo 2. Build -> Rebuild Solution
echo 3. View -> Refresh

pause 