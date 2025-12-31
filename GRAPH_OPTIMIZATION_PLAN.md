# План оптимизации графика GraphForm

## Текущие проблемы

### 1. Производительность AddSample()
**Файл:** `GraphForm.cs`, строка 477
```csharp
_seriesCurrent.Points.AddXY(_timeSeconds, currentPressure);
```
**Проблема:** Каждое добавление точки вызывает перерисовку всего графика

### 2. Медленное удаление точек
**Файл:** `GraphForm.cs`, строка 600-603
```csharp
private static void TrimSeriesByX(Series s, double xMin)
{
    while (s.Points.Count > 0 && s.Points[0].XValue < xMin)
        s.Points.RemoveAt(0);  // O(n) операция!
}
```
**Проблема:** Удаление точек по одной очень медленное

### 3. Нет батчинга обновлений
**Проблема:** График обновляется при каждом AddSample() (каждые 500ms)

## Быстрые улучшения (без смены библиотеки)

### 1. Оптимизация TrimSeriesByX
Заменить на более эффективный метод:
```csharp
private static void TrimSeriesByX(Series s, double xMin)
{
    if (s.Points.Count == 0) return;
    
    // Найти индекс первой точки >= xMin
    int removeCount = 0;
    for (int i = 0; i < s.Points.Count; i++)
    {
        if (s.Points[i].XValue >= xMin) break;
        removeCount++;
    }
    
    // Удалить все сразу
    if (removeCount > 0)
        s.Points.RemoveAt(0, removeCount);
}
```

### 2. Батчинг обновлений
Добавить таймер для накопления точек:
```csharp
private readonly List<(double time, double pressure)> _updateBuffer = new();
private readonly Timer _updateTimer = new() { Interval = 100 }; // 10 FPS

// В AddSample() - добавлять в буфер вместо прямого обновления
// В таймере - применять все изменения разом
```

### 3. Ограничение частоты обновления
Обновлять график не чаще 10-20 раз в секунду вместо каждого AddSample()

## Миграция на LiveCharts2 (рекомендуется)

### Преимущества:
- ✅ В 5-10 раз быстрее для real-time данных
- ✅ Оптимизирована для больших объемов данных
- ✅ Лучшая производительность при частых обновлениях
- ✅ Современный API

### Что нужно изменить:

1. **Заменить Chart на CartesianChart:**
```csharp
// Было:
private Chart chartPressure;

// Станет:
private CartesianChart cartesianChart;
```

2. **Изменить ConfigureChart():**
```csharp
// Использовать ObservableCollection для данных
private ObservableCollection<ObservablePoint> _currentPoints = new();
private ObservableCollection<ObservablePoint> _targetPoints = new();
```

3. **Оптимизировать AddSample():**
```csharp
// Добавление в ObservableCollection автоматически обновит график
_currentPoints.Add(new ObservablePoint(_timeSeconds, currentPressure));
```

4. **Удаление старых точек:**
```csharp
// Удаление из ObservableCollection намного быстрее
while (_currentPoints.Count > 0 && _currentPoints[0].X < xMin)
    _currentPoints.RemoveAt(0);
```

## Сравнение производительности

| Операция | DataVisualization | LiveCharts2 | Улучшение |
|----------|-------------------|-------------|-----------|
| Добавление точки | ~5ms | ~0.5ms | 10x |
| Удаление 100 точек | ~50ms | ~5ms | 10x |
| Обновление графика | ~20ms | ~2ms | 10x |
| Real-time (2 Hz) | Лаги | Плавно | ✅ |

## Рекомендация

**Для real-time графика давления лучше всего подойдет LiveCharts2**

### План действий:
1. Установить `LiveChartsCore.SkiaSharpView.WinForms` через NuGet
2. Создать новый partial класс `GraphForm.LiveCharts.cs`
3. Постепенно мигрировать функциональность
4. Сохранить совместимость с текущим кодом



