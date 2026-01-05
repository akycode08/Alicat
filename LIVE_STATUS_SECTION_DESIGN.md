# GraphForm - Live Status Section (Графическое представление создания)

## Реальный вид Live Status (по скриншоту)

```
┌──────────────────────────────────────┐
│  ┌────────────────────────────────┐  │
│  │                                │  │
│  │         20.00                  │  │  ← Большая цифра (белый, крупный)
│  │                                │  │
│  │         PSIG                   │  │  ← Единицы (белый)
│  │                                │  │
│  │         20.00                  │  │  ← Другое значение (серый, меньше)
│  │                                │  │
│  │                        Done    │  │  ← Кнопка/статус (справа внизу)
│  └────────────────────────────────┘  │
└──────────────────────────────────────┘
```

**Реальный вид:**
- Большая белая цифра давления (20.00) - `lblCurrentPressureLarge`
- Единицы измерения "PSIG" (белый) - `lblCurrentUnit`
- Вторая цифра "20.00" (серый, меньше) - возможно `lblTargetValue`
- "Done" в правом нижнем углу - статус/кнопка

## Расположение в интерфейсе

```
┌──────────────────────────────────────────────────────────────────────┐
│                        GraphForm (Main Form)                         │
├──────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  ┌──────────────────┐  ┌────────────────────────────────────────┐  │
│  │                  │  │                                        │  │
│  │  LIVE STATUS     │  │     Chart (LiveCharts2)                │  │
│  │  ┌──────────┐    │  │     (Pressure Graph)                   │  │
│  │  │ 20.00    │    │  │                                        │  │
│  │  │ PSIG     │    │  │                                        │  │
│  │  │ 20.00    │    │  │                                        │  │
│  │  │    Done  │    │  │                                        │  │
│  │  └──────────┘    │  │                                        │  │
│  │                  │  │                                        │  │
│  │  SESSION STATS   │  │                                        │  │
│  │  GO TO TARGET    │  │                                        │  │
│  └──────────────────┘  └────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────────┘
```

## Структура Live Status Section (РЕАЛЬНАЯ РЕАЛИЗАЦИЯ)

```
┌──────────────────────────────────────────────────────────────────────┐
│                    panelLeft (Left Panel)                            │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │  tlpLeft (TableLayoutPanel - основной контейнер)            │  │
│  │  ┌──────────────────────────────────────────────────────┐   │  │
│  │  │  Row 0: Spacer (2%)                                 │   │  │
│  │  ├──────────────────────────────────────────────────────┤   │  │
│  │  │  Row 1: tlpLiveStatus (20%) ← LIVE STATUS СЕКЦИЯ   │   │  │
│  │  │  Row 2: tlpSessionStats (18%)                       │   │  │
│  │  │  Row 3: tlpGoToTarget (50%)                         │   │  │
│  │  │  Row 4: btnEmergency (10%)                          │   │  │
│  │  └──────────────────────────────────────────────────────┘   │  │
│  └──────────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────────┘

ДЕТАЛЬНАЯ СТРУКТУРА tlpLiveStatus:
┌──────────────────────────────────────────────────────────────────────┐
│  tlpLiveStatus (TableLayoutPanel)                                    │
│  • ColumnCount: 2                                                    │
│  • RowCount: 3                                                       │
│  • Dock: Fill                                                        │
│  • Size: 183 × 134                                                   │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │  Row 0 (60% высоты):                                          │  │
│  │  ┌──────────────────────────────────────────────────────┐    │  │
│  │  │  tableLayoutPanel3 (ColumnSpan: 2, обе колонки)     │    │  │
│  │  │  ┌──────────────────────────────────────────────┐    │    │  │
│  │  │  │  Row 0 (70%):                               │    │  │  │
│  │  │  │  lblCurrentPressureLarge                    │    │  │  │
│  │  │  │  • Font: Segoe UI, 36pt, Bold              │    │  │  │
│  │  │  │  • Text: "160.50"                          │    │  │  │
│  │  │  │  • TextAlign: MiddleCenter                 │    │  │  │
│  │  │  │  • ForeColor: White                        │    │  │  │
│  │  │  │  • Dock: Fill                              │    │  │  │
│  │  │  │                                            │    │  │  │
│  │  │  │  Row 1 (30%):                              │    │  │  │
│  │  │  │  lblCurrentUnit                            │    │  │  │
│  │  │  │  • Font: Segoe UI, 9pt, Regular           │    │  │  │
│  │  │  │  • Text: "PSIG"                           │    │  │  │
│  │  │  │  • TextAlign: MiddleCenter                │    │  │  │
│  │  │  │  • ForeColor: RGB(120, 125, 140)          │    │  │  │
│  │  │  │  • Dock: Fill                             │    │  │  │
│  │  │  └──────────────────────────────────────────────┘    │    │  │
│  │  └──────────────────────────────────────────────────────┘    │  │
│  │                                                               │  │
│  ├──────────────────────────────────────────────────────────────┤  │
│  │  Row 1 (20% высоты):                                          │  │
│  │  ┌──────────────┬──────────────┐                            │  │
│  │  │  Column 0    │  Column 1    │                            │  │
│  │  ├──────────────┼──────────────┤                            │  │
│  │  │  lblTarget   │  lblTargetValue                            │  │
│  │  │  • Text:     │  • Text: "160.00"                         │  │
│  │  │    "Target"  │  • ForeColor: White                       │  │
│  │  │  • Font:     │  • TextAlign: (default)                   │  │
│  │  │    8.25pt,   │  • Size: 86 × 23                          │  │
│  │  │    Bold      │                                            │  │
│  │  │  • ForeColor:│                                            │  │
│  │  │    RGB(120,  │                                            │  │
│  │  │    125, 140) │                                            │  │
│  │  │  • Dock: Fill│                                            │  │
│  │  └──────────────┴──────────────┘                            │  │
│  │                                                               │  │
│  ├──────────────────────────────────────────────────────────────┤  │
│  │  Row 2 (20% высоты):                                          │  │
│  │  ┌──────────────┬──────────────┐                            │  │
│  │  │  Column 0    │  Column 1    │                            │  │
│  │  ├──────────────┼──────────────┤                            │  │
│  │  │  lblETA      │  lblETAValue                               │  │
│  │  │  • Text:     │  • Text: "2:35" (или "--")                │  │
│  │  │    "ETA:"    │  • ForeColor: White                       │  │
│  │  │  • Font:     │  • TextAlign: MiddleRight                 │  │
│  │  │    (default) │  • Size: 87 × 23                          │  │
│  │  │  • ForeColor:│                                            │  │
│  │  │    RGB(120,  │                                            │  │
│  │  │    125, 140) │                                            │  │
│  │  │  • Dock: Fill│                                            │  │
│  │  │  • BackColor:│                                            │  │
│  │  │    RGB(21,   │                                            │  │
│  │  │    23, 28)   │                                            │  │
│  │  └──────────────┴──────────────┘                            │  │
│  └──────────────────────────────────────────────────────────────┘  │
└──────────────────────────────────────────────────────────────────────┘
```

## Определение в Designer.cs (РЕАЛЬНЫЙ КОД)

```
┌──────────────────────────────────────────────────────────────────────┐
│                    GraphForm.Designer.cs                             │
└──────────────────────────────────────────────────────────────────────┘

1. ОБЪЯВЛЕНИЕ ПЕРЕМЕННЫХ (поля класса):
   ┌────────────────────────────────────────────────────────────┐
   │ private TableLayoutPanel tlpLiveStatus;                    │
   │ private TableLayoutPanel tableLayoutPanel3;                │
   │ private Label lblCurrentPressureLarge;                     │
   │ private Label lblCurrentUnit;                              │
   │ private Label lblTarget;                                   │
   │ private Label lblTargetValue;                              │
   │ private Label lblETA;                                      │
   │ private Label lblETAValue;                                 │
   └────────────────────────────────────────────────────────────┘

2. ИНИЦИАЛИЗАЦИЯ В InitializeComponent():
   ┌────────────────────────────────────────────────────────────┐
   │ private void InitializeComponent()                         │
   │ {                                                           │
   │     // ... другие компоненты ...                           │
   │                                                             │
   │     // ==================================================== │
   │     // tlpLiveStatus - TableLayoutPanel (2 колонки, 3 строки)│
   │     // ==================================================== │
   │                                                             │
   │     this.tlpLiveStatus = new TableLayoutPanel();           │
   │     this.tlpLiveStatus.ColumnCount = 2;                    │
   │     this.tlpLiveStatus.ColumnStyles.Add(                   │
   │         new ColumnStyle(SizeType.Percent, 50F));           │
   │     this.tlpLiveStatus.ColumnStyles.Add(                   │
   │         new ColumnStyle(SizeType.Percent, 50F));           │
   │     this.tlpLiveStatus.RowCount = 3;                       │
   │     this.tlpLiveStatus.RowStyles.Add(                      │
   │         new RowStyle(SizeType.Percent, 60F));              │
   │     this.tlpLiveStatus.RowStyles.Add(                      │
   │         new RowStyle(SizeType.Percent, 20F));              │
   │     this.tlpLiveStatus.RowStyles.Add(                      │
   │         new RowStyle(SizeType.Percent, 20F));              │
   │     this.tlpLiveStatus.Dock = DockStyle.Fill;              │
   │                                                             │
   │     // ==================================================== │
   │     // tableLayoutPanel3 - для Current Pressure (большой)  │
   │     // ==================================================== │
   │                                                             │
   │     this.tableLayoutPanel3 = new TableLayoutPanel();       │
   │     this.tableLayoutPanel3.ColumnCount = 1;                │
   │     this.tableLayoutPanel3.RowCount = 2;                   │
   │     this.tlpLiveStatus.SetColumnSpan(                      │
   │         this.tableLayoutPanel3, 2); // Занимает обе колонки│
   │     this.tlpLiveStatus.Controls.Add(                       │
   │         this.tableLayoutPanel3, 0, 0);                     │
   │                                                             │
   │     // ==================================================== │
   │     // lblCurrentPressureLarge - большое отображение       │
   │     // ==================================================== │
   │                                                             │
   │     this.lblCurrentPressureLarge = new Label();            │
   │     this.lblCurrentPressureLarge.Dock = DockStyle.Fill;    │
   │     this.lblCurrentPressureLarge.Font = new Font(          │
   │         "Segoe UI", 36F, FontStyle.Bold);                  │
   │     this.lblCurrentPressureLarge.ForeColor = Color.White;  │
   │     this.lblCurrentPressureLarge.Text = "0.00";            │
   │     this.lblCurrentPressureLarge.TextAlign =               │
   │         ContentAlignment.MiddleCenter;                     │
   │     this.lblCurrentPressureLarge.Click +=                  │
   │         lblCurrentPressureLarge_Click;                     │
   │     this.tableLayoutPanel3.Controls.Add(                   │
   │         this.lblCurrentPressureLarge, 0, 0);               │
   │                                                             │
   │     // ==================================================== │
   │     // lblCurrentUnit - единицы измерения                  │
   │     // ==================================================== │
   │                                                             │
   │     this.lblCurrentUnit = new Label();                     │
   │     this.lblCurrentUnit.Dock = DockStyle.Fill;             │
   │     this.lblCurrentUnit.Font = new Font(                   │
   │         "Segoe UI", 9F, FontStyle.Regular);                │
   │     this.lblCurrentUnit.ForeColor =                        │
   │         Color.FromArgb(120, 125, 140);                     │
   │     this.lblCurrentUnit.Text = "PSIG";                     │
   │     this.lblCurrentUnit.TextAlign =                        │
   │         ContentAlignment.MiddleCenter;                     │
   │     this.lblCurrentUnit.Click += lblCurrentUnit_Click;     │
   │     this.tableLayoutPanel3.Controls.Add(                   │
   │         this.lblCurrentUnit, 0, 1);                        │
   │                                                             │
   │     // ==================================================== │
   │     // lblTarget - метка "Target"                          │
   │     // ==================================================== │
   │                                                             │
   │     this.lblTarget = new Label();                          │
   │     this.lblTarget.Dock = DockStyle.Fill;                  │
   │     this.lblTarget.Font = new Font(                        │
   │         "Segoe UI", 8.25F, FontStyle.Bold);                │
   │     this.lblTarget.ForeColor =                             │
   │         Color.FromArgb(120, 125, 140);                     │
   │     this.lblTarget.Text = "Target";                        │
   │     this.lblTarget.TextAlign =                             │
   │         ContentAlignment.MiddleLeft;                       │
   │     // НО! lblTarget находится в tableLayoutPanel1,        │
   │     // не напрямую в tlpLiveStatus                         │
   │                                                             │
   │     // ==================================================== │
   │     // lblTargetValue - значение Target                    │
   │     // ==================================================== │
   │                                                             │
   │     this.lblTargetValue = new Label();                     │
   │     this.lblTargetValue.ForeColor = Color.White;           │
   │     this.tlpLiveStatus.Controls.Add(                       │
   │         this.lblTargetValue, 1, 1);                        │
   │                                                             │
   │     // ==================================================== │
   │     // lblETA - метка "ETA:"                               │
   │     // ==================================================== │
   │                                                             │
   │     this.lblETA = new Label();                             │
   │     this.lblETA.Dock = DockStyle.Fill;                     │
   │     this.lblETA.ForeColor =                                │
   │         Color.FromArgb(120, 125, 140);                     │
   │     this.lblETA.Text = "ETA:";                             │
   │     this.lblETA.TextAlign =                                │
   │         ContentAlignment.MiddleLeft;                       │
   │     // НО! lblETA находится в tableLayoutPanel1            │
   │                                                             │
   │     // ==================================================== │
   │     // lblETAValue - значение ETA                          │
   │     // ==================================================== │
   │                                                             │
   │     this.lblETAValue = new Label();                        │
   │     this.lblETAValue.Dock = DockStyle.Fill;                │
   │     this.lblETAValue.ForeColor = Color.White;              │
   │     this.lblETAValue.Text = "--";                          │
   │     this.lblETAValue.TextAlign =                           │
   │         ContentAlignment.MiddleRight;                      │
   │     this.tlpLiveStatus.Controls.Add(                       │
   │         this.lblETAValue, 1, 2);                           │
   │                                                             │
   │     // ==================================================== │
   │     // Добавляем tlpLiveStatus в tlpLeft (левая панель)   │
   │     // ==================================================== │
   │                                                             │
   │     this.tlpLeft.Controls.Add(                             │
   │         this.tlpLiveStatus, 0, 1);                         │
   │                                                             │
   │     // ==================================================== │
   │     // Обработчик для рисования границы                    │
   │     // ==================================================== │
   │                                                             │
   │     // (добавляется в GraphForm.cs конструкторе)          │
   │     // this.tlpLiveStatus.Paint += TlpLiveStatus_Paint;    │
   │                                                             │
   │     // ... остальные компоненты ...                        │
   │ }                                                           │
   └────────────────────────────────────────────────────────────┘
```

## Логика обновления (GraphForm.Statistics.cs)

```
┌──────────────────────────────────────────────────────────────────────┐
│        GraphForm.Statistics.cs - UpdateLiveStatus() (РЕАЛЬНЫЙ КОД)  │
└──────────────────────────────────────────────────────────────────────┘

Метод UpdateLiveStatus():
┌──────────────────────────────────────────────────────────────────┐
│ private void UpdateLiveStatus(                                    │
│     double currentPressure,    // Текущее давление                │
│     double? targetPressure,    // Целевое давление (nullable)     │
│     string unit,               // Единицы измерения (PSIG/PSIA)   │
│     bool isExhaust,            // Режим выхлопа?                  │
│     double rate)                // Скорость изменения (PSI/s)     │
│ {                                                                 │
│     if (InvokeRequired)                                          │
│     {                                                             │
│         BeginInvoke(new Action(() =>                              │
│             UpdateLiveStatus(currentPressure, targetPressure,     │
│                              unit, isExhaust, rate)));            │
│         return;                                                   │
│     }                                                             │
│                                                                   │
│     // ========================================================   │
│     // 1. Обновляем большое отображение текущего давления        │
│     // ========================================================   │
│                                                                   │
│     if (lblCurrentPressureLarge != null)                         │
│     {                                                             │
│         lblCurrentPressureLarge.Text =                            │
│             currentPressure.ToString("F2");  // Формат: 160.50    │
│     }                                                             │
│                                                                   │
│     // ========================================================   │
│     // 2. Обновляем единицы измерения                            │
│     // ========================================================   │
│                                                                   │
│     if (lblCurrentUnit != null)                                  │
│     {                                                             │
│         lblCurrentUnit.Text = unit;  // "PSIG" или "PSIA"        │
│     }                                                             │
│                                                                   │
│     // ========================================================   │
│     // 3. Обновляем значение Target                              │
│     // ========================================================   │
│                                                                   │
│     if (lblTargetValue != null)                                  │
│     {                                                             │
│         if (targetPressure.HasValue)                             │
│         {                                                         │
│             lblTargetValue.Text =                                 │
│                 targetPressure.Value.ToString("F2");              │
│         }                                                         │
│         else                                                      │
│         {                                                         │
│             lblTargetValue.Text = "--";                           │
│         }                                                         │
│     }                                                             │
│                                                                   │
│     // ========================================================   │
│     // 4. Определяем и обновляем индикатор статуса               │
│     // ========================================================   │
│                                                                   │
│     StatusLevel status = DetermineStatusLevel(                   │
│         currentPressure, targetPressure);                        │
│     UpdateStatusIndicator(status);                                │
│                                                                   │
│     // ========================================================   │
│     // 5. Обновляем ETA (Estimated Time to Arrival)              │
│     // ========================================================   │
│                                                                   │
│     if (lblETAValue != null)                                     │
│     {                                                             │
│         if (targetPressure.HasValue && !isExhaust)               │
│         {                                                         │
│             double delta = currentPressure -                      │
│                            targetPressure.Value;                  │
│             if (Math.Abs(rate) > 0.01)                           │
│             {                                                     │
│                 double etaSeconds = Math.Abs(delta / rate);       │
│                 int etaMins = (int)(etaSeconds / 60);            │
│                 int etaSecs = (int)(etaSeconds % 60);            │
│                 lblETAValue.Text = $"{etaMins}:{etaSecs:D2}";    │
│             }                                                     │
│             else                                                  │
│             {                                                     │
│                 lblETAValue.Text = Math.Abs(delta) < 0.1          │
│                     ? "Done" : "Stable";                          │
│             }                                                     │
│         }                                                         │
│         else                                                      │
│         {                                                         │
│             lblETAValue.Text = isExhaust ? "Purging" : "--";      │
│         }                                                         │
│     }                                                             │
│ }                                                                 │
└──────────────────────────────────────────────────────────────────┘

Вызов UpdateLiveStatus():
┌──────────────────────────────────────────────────────────────────┐
│ public void AddSample(double currentPressure, double? targetPressure)│
│ {                                                                 │
│     // ... добавление точек в буфер ...                          │
│                                                                   │
│     // Вычисляем Rate (скорость изменения)                       │
│     double rate = 0;                                             │
│     if (_seriesCurrent.Count > 0)                                │
│     {                                                             │
│         var lastPoint = _seriesCurrent[_seriesCurrent.Count - 1];│
│         rate = (currentPressure - lastPoint.Y.Value) / TimeStep; │
│     }                                                             │
│                                                                   │
│     // Получаем единицы измерения                                │
│     string unit = _dataStore.Points.Count > 0                    │
│         ? _dataStore.Points.Last().Unit                          │
│         : "PSIG";                                                │
│                                                                   │
│     // Обновляем Live Status UI сразу (не в батче)               │
│     UpdateLiveStatus(                                            │
│         currentPressure,                                         │
│         targetPressure,                                          │
│         unit,                                                    │
│         false,  // isExhaust (передается из другого места)       │
│         rate);                                                   │
│ }                                                                 │
└──────────────────────────────────────────────────────────────────┘
```

## Поток данных (РЕАЛЬНЫЙ)

```
┌──────────────────────────────────────────────────────────────────────┐
│                    ПОТОК ОБНОВЛЕНИЯ LIVE STATUS                      │
└──────────────────────────────────────────────────────────────────────┘

1. ПОЛУЧЕНИЕ ДАННЫХ:
   ┌──────────────┐
   │ DataStore    │ → Новые данные давления (current, target)
   │ (Polling)    │   Каждые 500ms
   └──────┬───────┘
          │
          ▼
   ┌─────────────────────────────────────┐
   │ GraphForm.AddSample()               │
   │ ├─ currentPressure: double          │
   │ ├─ targetPressure: double?          │
   │ ├─ Вычисляет rate из истории:       │
   │ │   rate = (current - last) / TimeStep│
   │ └─ Получает unit из DataStore       │
   └──────┬──────────────────────────────┘
          │
          ▼
   ┌─────────────────────────────────────┐
   │ UpdateLiveStatus()                  │
   │ (в GraphForm.Statistics.cs)         │
   │                                     │
   │ ├─ lblCurrentPressureLarge.Text     │
   │ │   = currentPressure.ToString("F2")│
   │ │   → "160.50"                      │
   │ │                                     │
   │ ├─ lblCurrentUnit.Text = unit       │
   │ │   → "PSIG"                        │
   │ │                                     │
   │ ├─ lblTargetValue.Text              │
   │ │   = targetPressure.ToString("F2") │
   │ │   → "160.00" или "--"             │
   │ │                                     │
   │ ├─ DetermineStatusLevel()           │
   │ │   → StatusLevel (ALERT/WARN/OK)   │
   │ │                                     │
   │ ├─ UpdateStatusIndicator()          │
   │ │   → Обновляет индикатор статуса   │
   │ │                                     │
   │ └─ lblETAValue.Text                 │
   │     = CalculateETA(delta, rate)     │
   │     → "2:35" или "Done" или "--"    │
   └──────┬──────────────────────────────┘
          │
          ▼
   ┌─────────────────────────────────────┐
   │ UI Labels (WinForms)                │
   │                                     │
   │ ┌──────────────────────────────┐    │
   │ │ lblCurrentPressureLarge      │    │
   │ │ • Font: 36pt, Bold           │    │
   │ │ • Text: "160.50"             │    │
   │ │ • Color: White               │    │
   │ └──────────────────────────────┘    │
   │                                     │
   │ ┌──────────────────────────────┐    │
   │ │ lblCurrentUnit               │    │
   │ │ • Font: 9pt, Regular         │    │
   │ │ • Text: "PSIG"               │    │
   │ │ • Color: RGB(120,125,140)    │    │
   │ └──────────────────────────────┘    │
   │                                     │
   │ ┌──────────────────────────────┐    │
   │ │ lblTargetValue               │    │
   │ │ • Text: "160.00"             │    │
   │ │ • Color: White               │    │
   │ └──────────────────────────────┘    │
   │                                     │
   │ ┌──────────────────────────────┐    │
   │ │ lblETAValue                  │    │
   │ │ • Text: "2:35"               │    │
   │ │ • Color: White               │    │
   │ └──────────────────────────────┘    │
   └─────────────────────────────────────┘
```

## РЕАЛЬНАЯ КОМПОНОВКА (Используется в проекте)

```
ВАРИАНТ (РЕАЛИЗОВАН): TableLayoutPanel с вложенной структурой
┌──────────────────────────────────────────────────────────┐
│  tlpLiveStatus (TableLayoutPanel: 2 колонки × 3 строки) │
│  ┌──────────────────────────────────────────┐            │
│  │  Row 0 (60% высоты):                     │            │
│  │  ┌────────────────────────────────────┐  │            │
│  │  │ tableLayoutPanel3 (ColumnSpan: 2)  │  │            │
│  │  │  ┌──────────────────────────────┐  │  │            │
│  │  │  │ lblCurrentPressureLarge     │  │  │            │
│  │  │  │ 160.50 (36pt, Bold, White)  │  │  │            │
│  │  │  └──────────────────────────────┘  │  │            │
│  │  │  ┌──────────────────────────────┐  │  │            │
│  │  │  │ lblCurrentUnit              │  │  │            │
│  │  │  │ PSIG (9pt, Regular, Gray)   │  │  │            │
│  │  │  └──────────────────────────────┘  │  │            │
│  │  └────────────────────────────────────┘  │            │
│  ├──────────────────────────────────────────┤            │
│  │  Row 1 (20% высоты):                     │            │
│  │  ┌──────────────┬──────────────┐        │            │
│  │  │  Column 0    │  Column 1    │        │            │
│  │  ├──────────────┼──────────────┤        │            │
│  │  │  (пусто)     │ lblTargetValue│       │            │
│  │  │              │ 160.00       │        │            │
│  │  └──────────────┴──────────────┘        │            │
│  │  (Примечание: lblTarget находится       │            │
│  │   в tableLayoutPanel1, не здесь)        │            │
│  ├──────────────────────────────────────────┤            │
│  │  Row 2 (20% высоты):                     │            │
│  │  ┌──────────────┬──────────────┐        │            │
│  │  │  Column 0    │  Column 1    │        │            │
│  │  ├──────────────┼──────────────┤        │            │
│  │  │  (пусто)     │ lblETAValue  │        │            │
│  │  │              │ 2:35         │        │            │
│  │  └──────────────┴──────────────┘        │            │
│  │  (Примечание: lblETA находится          │            │
│  │   в tableLayoutPanel1, не здесь)        │            │
│  └──────────────────────────────────────────┘            │
└──────────────────────────────────────────────────────────┘

РАСПОЛОЖЕНИЕ В ЛЕВОЙ ПАНЕЛИ:
┌──────────────────────────────────────────────────────────┐
│  panelLeft (Dock: Left, Width: 260)                     │
│  ┌──────────────────────────────────────────┐            │
│  │  tlpLeft (TableLayoutPanel)              │            │
│  │  ┌────────────────────────────────────┐  │            │
│  │  │  Row 0: Spacer (2%)               │  │            │
│  │  ├────────────────────────────────────┤  │            │
│  │  │  Row 1: tlpLiveStatus (20%) ←     │  │            │
│  │  │          LIVE STATUS СЕКЦИЯ       │  │            │
│  │  ├────────────────────────────────────┤  │            │
│  │  │  Row 2: tlpSessionStats (18%)     │  │            │
│  │  │          Statistics               │  │            │
│  │  ├────────────────────────────────────┤  │            │
│  │  │  Row 3: tlpGoToTarget (50%)       │  │            │
│  │  │          GO TO TARGET             │  │            │
│  │  ├────────────────────────────────────┤  │            │
│  │  │  Row 4: btnEmergency (10%)        │  │            │
│  │  └────────────────────────────────────┘  │            │
│  └──────────────────────────────────────────┘            │
└──────────────────────────────────────────────────────────┘
```

## Инициализация при загрузке формы (РЕАЛЬНЫЙ КОД)

```
┌──────────────────────────────────────────────────────────────────────┐
│                    GraphForm Constructor                             │
└──────────────────────────────────────────────────────────────────────┘

public GraphForm(SessionDataStore dataStore)
{
    _dataStore = dataStore;
    
    // 1. Инициализация компонентов (Designer)
    InitializeComponent();
    //    ├─ Создаются все компоненты:
    //    │   • tlpLiveStatus (TableLayoutPanel)
    //    │   • tableLayoutPanel3 (вложенный TableLayoutPanel)
    //    │   • lblCurrentPressureLarge (36pt, Bold)
    //    │   • lblCurrentUnit (9pt, Regular)
    //    │   • lblTargetValue
    //    │   • lblETAValue
    //    ├─ Настраиваются свойства (цвета, шрифты, расположение)
    //    └─ Компоненты добавляются в tlpLeft (левая панель)
    
    // 2. Настройка графика
    ConfigureChart();
    
    // 3. Загрузка истории данных
    LoadHistoryFromStore();
    
    // 4. Подписка на новые точки данных
    _dataStore.OnNewPoint += OnNewPointReceived;
    
    // 5. Инициализация статистики
    CalculateAndUpdateStatistics();
    
    // 6. Инициализация Live Status начальными значениями
    UpdateLiveStatus(0, null, "PSIG", false, 0);
    //    └─ Устанавливает:
    //        • lblCurrentPressureLarge.Text = "0.00"
    //        • lblCurrentUnit.Text = "PSIG"
    //        • lblTargetValue.Text = "--"
    //        • lblETAValue.Text = "--"
    
    // 7. Инициализация других компонентов
    InitializeHeaderFooter();
    InitializeToolbar();
    InitializeGoToTarget();
    
    // 8. Настройка обработчика для границы Live Status панели
    if (tlpLiveStatus != null)
    {
        tlpLiveStatus.Paint += TlpLiveStatus_Paint;
        // Рисует границу вокруг панели
    }
    
    // 9. Запуск таймера обновления
    _updateTimer = new System.Windows.Forms.Timer { Interval = 100 };
    _updateTimer.Tick += UpdateTimer_Tick;
    _updateTimer.Start();
}
```

## Обновление в реальном времени

```
┌──────────────────────────────────────────────────────────────────────┐
│              ОБНОВЛЕНИЕ ПРИ ПОЛУЧЕНИИ НОВЫХ ДАННЫХ                   │
└──────────────────────────────────────────────────────────────────────┘

Каждые 500ms (Polling Timer):
    SessionDataStore → Новые данные
        │
        ▼
    GraphForm.AddSample(currentPressure, targetPressure)
        │
        ├─► Добавление точек в график
        │
        └─► UpdateLiveStatus(current, target, unit, isExhaust, rate)
                │
                ├─► lblCurrent.Text = "Current: 160.5 PSIG"
                ├─► lblTarget.Text = "Target: 160.0 PSIG"
                ├─► lblRate.Text = "Rate: 2.5 PSIG/s"
                └─► lblUnit.Text = "Unit: PSIG"

Результат: UI обновляется в реальном времени каждые 500ms
```

## Ключевые особенности (РЕАЛЬНАЯ РЕАЛИЗАЦИЯ)

```
✅ НАЗНАЧЕНИЕ:
   • Отображение текущего состояния системы в реальном времени
   • Показывает: Current (большое отображение), Target, ETA

✅ РАСПОЛОЖЕНИЕ:
   • В ЛЕВОЙ ПАНЕЛИ (panelLeft), НЕ в Footer
   • Первая секция в tlpLeft (Row 1, 20% высоты)
   • Находится выше Session Stats и GO TO TARGET

✅ ОБНОВЛЕНИЕ:
   • Вызывается из AddSample() при получении новых данных
   • Частота: каждые 500ms (частота polling timer)
   • Обновляется сразу (не батчится с графиком)

✅ КОМПОНЕНТЫ:
   • tlpLiveStatus: TableLayoutPanel (2 колонки × 3 строки)
   • tableLayoutPanel3: вложенный TableLayoutPanel для Current
   • lblCurrentPressureLarge: большое отображение (36pt, Bold)
   • lblCurrentUnit: единицы измерения (9pt)
   • lblTargetValue: значение Target (F2 формат)
   • lblETAValue: значение ETA (формат "M:SS" или "Done"/"Stable")

✅ ФОРМАТИРОВАНИЕ:
   • Current: F2 формат (160.50) - большое отображение
   • Target: F2 формат (160.00) или "--"
   • ETA: "M:SS" (2:35) или "Done"/"Stable"/"Purging"/"--"
   • Unit: Текстовое значение (PSIG/PSIA) - отдельный Label

✅ ДОПОЛНИТЕЛЬНЫЕ ФУНКЦИИ:
   • Click handlers: lblCurrentPressureLarge_Click, lblCurrentUnit_Click
   • Граница панели: TlpLiveStatus_Paint (рисует границу)
   • Status Indicator: визуальный индикатор статуса (ALERT/WARN/OK)
   • Thread-safe: проверка InvokeRequired в UpdateLiveStatus()
```

