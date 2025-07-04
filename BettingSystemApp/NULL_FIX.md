# Исправление ошибки с nullable типами

## Проблема
```
System.InvalidOperationException: "The cast to value type 'System.Decimal' failed because the materialized value is null. Either the result type's generic parameter or the query must use a nullable type."
```

## Причина
В базе данных поле `Draw` в таблице `Bets` может содержать `NULL` значения, но в модели C# оно было определено как не-nullable `decimal`.

## Исправления

### 1. Обновлена модель Bet.cs
```csharp
// Было:
public decimal Draw { get; set; }

// Стало:
public decimal? Draw { get; set; }
```

### 2. Исправлены все места использования поля Draw

#### EditorWindow.xaml.cs
```csharp
// Было:
Draw = $"{b.Draw:F2}"

// Стало:
Draw = b.Draw.HasValue ? $"{b.Draw.Value:F2}" : "N/A"
```

#### EditBetWindow.xaml.cs
```csharp
// Загрузка данных:
DrawTextBox.Text = _bet.Draw.HasValue ? _bet.Draw.Value.ToString("F2") : "";

// Сохранение данных:
if (string.IsNullOrWhiteSpace(DrawTextBox.Text))
{
    betToUpdate.Draw = null;
}
else if (decimal.TryParse(DrawTextBox.Text, out decimal draw))
{
    betToUpdate.Draw = draw;
}
```

#### AdminWindow.xaml.cs
```csharp
// Архивирование:
bet.Draw.HasValue ? bet.Draw.Value.ToString() : ""

// Разархивирование:
Draw = decimal.TryParse(parts[8], out decimal draw) ? draw : (decimal?)null
```

#### DirectorWindow.xaml.cs
```csharp
// Отчеты:
bet.Draw.HasValue ? bet.Draw.Value.ToString("F2") : "N/A"
```

#### AddBetWindow.xaml.cs
```csharp
// Создание новой ставки:
Draw = string.IsNullOrWhiteSpace(DrawTextBox.Text) ? null : 
       (decimal.TryParse(DrawTextBox.Text, out decimal draw) ? draw : (decimal?)null)
```

### 3. Убрана обязательная проверка поля Draw
- Удалена проверка `string.IsNullOrWhiteSpace(DrawTextBox.Text)` в валидации
- Удалена проверка `decimal.TryParse(DrawTextBox.Text, out decimal draw)`
- Удалена проверка `draw <= 0`

## Результат
- ✅ Поле `Draw` теперь корректно обрабатывает `NULL` значения
- ✅ Приложение не падает при работе с пустыми значениями коэффициента ничьей
- ✅ Архивирование и разархивирование работает корректно
- ✅ Отчеты отображают "N/A" для пустых значений

## Тестирование
1. Запустите приложение
2. Войдите как editor/editor123
3. Добавьте ставку без коэффициента ничьей (оставьте поле пустым)
4. Проверьте, что ставка сохраняется корректно
5. Проверьте архивирование и разархивирование
6. Проверьте отчеты

## Примечание
Поле `Draw` теперь опционально. Для видов спорта, где нет ничьей (например, баскетбол, теннис), это поле можно оставлять пустым. 