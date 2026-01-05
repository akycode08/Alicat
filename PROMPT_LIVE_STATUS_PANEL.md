# Промпт для Cursor AI - Live Status Panel (WinForms)

## Задача
Создай секцию Live Status для WinForms приложения с точным дизайном. Это панель отображения текущего давления и статуса контроллера Alicat.

```
┌─ LIVE STATUS ──────────────────────────────────────┐
│                                                    │
│                    50.00                           │  ← Большое число (давление)
│                     PSIG                           │  ← Единица измерения
│                                                    │
│  Target:                              100.00       │  ← Золотой цвет
│  ─────────────────────────────────────────────    │
│  ETA:                                    --        │  ← Зелёный цвет
│                                                    │
└────────────────────────────────────────────────────┘
```

## Точные размеры и расположение

**Общий размер панели:** 240 x 140 px

```
┌─ LIVE STATUS ──────────────────────────────────────┐  Y=0
│                                                    │
│                                                    │  Y=20
│                   50.00                            │  Y=30 (центр ~55)
│                    PSIG                            │  Y=75
│                                                    │  Y=95
│  Target:                              100.00       │  Y=100
│  ───────────────────────────────────────────────  │  Y=118 (разделитель)
│  ETA:                                    --        │  Y=122
│                                                    │
└────────────────────────────────────────────────────┘  Y=140
```

## Цветовая схема

```csharp
// Background
Color BgPanel = Color.FromArgb(15, 23, 42);         // #0f172a - фон панели

// Text colors
Color TextPressure = Color.FromArgb(228, 231, 235); // #e4e7eb - большое число давления
Color TextUnit = Color.FromArgb(107, 114, 128);     // #6b7280 - "PSIG"
Color TextLabel = Color.FromArgb(107, 114, 128);    // #6b7280 - "Target:", "ETA:"
Color TextTarget = Color.FromArgb(245, 158, 11);    // #f59e0b - значение Target (ЗОЛОТОЙ)
Color TextETA = Color.FromArgb(16, 185, 129);       // #10b981 - значение ETA (ЗЕЛЁНЫЙ)

// Border
Color BorderDivider = Color.FromArgb(38, 45, 62);   // #262d3e - разделительная линия
```

## Детальная спецификация элементов

### 1. GroupBox "LIVE STATUS"

```csharp
GroupBox grpLiveStatus = new GroupBox();
grpLiveStatus.Text = "LIVE STATUS";
grpLiveStatus.Location = new Point(0, 0);
grpLiveStatus.Size = new Size(240, 140);
grpLiveStatus.ForeColor = Color.FromArgb(156, 163, 175);  // Цвет заголовка
grpLiveStatus.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
grpLiveStatus.BackColor = Color.FromArgb(15, 23, 42);
```

### 2. Большое число давления (центр)

```csharp
Label lblPressureValue = new Label();
lblPressureValue.Text = "50.00";
lblPressureValue.Location = new Point(0, 25);
lblPressureValue.Size = new Size(240, 45);
lblPressureValue.ForeColor = Color.FromArgb(228, 231, 235);  // Белый
lblPressureValue.Font = new Font("Consolas", 32F, FontStyle.Bold);
lblPressureValue.TextAlign = ContentAlignment.MiddleCenter;
lblPressureValue.BackColor = Color.Transparent;
```

### 3. Единица измерения "PSIG"

```csharp
Label lblUnit = new Label();
lblUnit.Text = "PSIG";
lblUnit.Location = new Point(0, 70);
lblUnit.Size = new Size(240, 20);
lblUnit.ForeColor = Color.FromArgb(107, 114, 128);  // Серый
lblUnit.Font = new Font("Segoe UI", 10F);
lblUnit.TextAlign = ContentAlignment.MiddleCenter;
lblUnit.BackColor = Color.Transparent;
```

### 4. Target строка

```csharp
// Label "Target:"
Label lblTargetLabel = new Label();
lblTargetLabel.Text = "Target:";
lblTargetLabel.Location = new Point(10, 98);
lblTargetLabel.Size = new Size(50, 18);
lblTargetLabel.ForeColor = Color.FromArgb(107, 114, 128);  // Серый
lblTargetLabel.Font = new Font("Segoe UI", 9F);
lblTargetLabel.TextAlign = ContentAlignment.MiddleLeft;

// Value "100.00" (справа, ЗОЛОТОЙ)
Label lblTargetValue = new Label();
lblTargetValue.Text = "100.00";
lblTargetValue.Location = new Point(120, 98);
lblTargetValue.Size = new Size(110, 18);
lblTargetValue.ForeColor = Color.FromArgb(245, 158, 11);  // ЗОЛОТОЙ
lblTargetValue.Font = new Font("Consolas", 10F, FontStyle.Bold);
lblTargetValue.TextAlign = ContentAlignment.MiddleRight;
```

### 5. Разделительная линия

```csharp
// Можно использовать Panel как линию
Panel divider = new Panel();
divider.Location = new Point(10, 118);
divider.Size = new Size(220, 1);
divider.BackColor = Color.FromArgb(38, 45, 62);  // Тёмная линия
```

### 6. ETA строка

```csharp
// Label "ETA:"
Label lblETALabel = new Label();
lblETALabel.Text = "ETA:";
lblETALabel.Location = new Point(10, 122);
lblETALabel.Size = new Size(35, 18);
lblETALabel.ForeColor = Color.FromArgb(107, 114, 128);  // Серый
lblETALabel.Font = new Font("Segoe UI", 9F);
lblETALabel.TextAlign = ContentAlignment.MiddleLeft;

// Value "--" или "2:45" (справа, ЗЕЛЁНЫЙ)
Label lblETAValue = new Label();
lblETAValue.Text = "--";
lblETAValue.Location = new Point(120, 122);
lblETAValue.Size = new Size(110, 18);
lblETAValue.ForeColor = Color.FromArgb(16, 185, 129);  // ЗЕЛЁНЫЙ
lblETAValue.Font = new Font("Consolas", 10F, FontStyle.Bold);
lblETAValue.TextAlign = ContentAlignment.MiddleRight;
```

## Ключевые моменты

| Элемент | Шрифт | Размер | Цвет | Выравнивание |
|---------|-------|--------|------|--------------|
| Давление | Consolas Bold | 32pt | #e4e7eb (белый) | Центр |
| PSIG | Segoe UI | 10pt | #6b7280 (серый) | Центр |
| Target: | Segoe UI | 9pt | #6b7280 (серый) | Лево |
| 100.00 | Consolas Bold | 10pt | #f59e0b (золотой) | Право |
| ETA: | Segoe UI | 9pt | #6b7280 (серый) | Лево |
| -- | Consolas Bold | 10pt | #10b981 (зелёный) | Право |

## Дополнительные требования

1. **Обновление значений:**
   - `lblPressureValue.Text` обновляется из `UpdateLiveStatus(currentPressure, ...)`
   - `lblTargetValue.Text` обновляется из `UpdateLiveStatus(..., targetPressure, ...)`
   - `lblETAValue.Text` обновляется из `UpdateLiveStatus(..., ..., ..., ..., rate)`
   - `lblUnit.Text` обновляется из `UpdateLiveStatus(..., ..., unit, ...)`

2. **Форматирование:**
   - Давление: `currentPressure.ToString("F2")` → "50.00"
   - Target: `targetPressure?.ToString("F2") ?? "--"` → "100.00" или "--"
   - ETA: вычисляется из `delta / rate`, формат "M:SS" или "Done"/"Stable"/"--"

3. **Расположение:**
   - Панель должна находиться в левой панели (`panelLeft`), в `tlpLeft` на Row 1 (20% высоты)
   - Или может быть встроена в существующий `tlpLiveStatus`

4. **Интеграция:**
   - Метод `UpdateLiveStatus()` в `GraphForm.Statistics.cs` должен обновлять все Label'ы
   - Thread-safe: использовать `InvokeRequired` и `BeginInvoke` для обновления UI

