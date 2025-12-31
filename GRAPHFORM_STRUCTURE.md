# 📐 Полный чертеж окна GraphForm

## 🏗️ Структура окна

```
GraphForm (954 x 533)
├── panelHeader (Dock: Top, 954 x 50) - Заголовок
│   ├── headerLeftPanel (FlowLayoutPanel, Dock: Left)
│   │   ├── appIcon (Panel, 20x20) - Иконка приложения (3 вертикальные полосы)
│   │   ├── lblAppTitle (Label) - "Alicat Pressure Controller:"
│   │   ├── _connectionStatusPanel (Panel, 200x28) - Индикатор подключения (создается программно)
│   │   └── lblSessionTime (Label) - "Session: 00:00:00"
│   ├── btnExport (Button, 75x23, Location: 697,12) - Кнопка Export
│   └── btnPause (Button, 75x23, Location: 796,12) - Кнопка Pause
│
├── panelLeft (Dock: Left, 200 x 483) - Левая панель
│   └── tlpLeft (TableLayoutPanel)
│       ├── tlpLiveStatus (TableLayoutPanel) - LIVE STATUS
│       │   ├── tableLayoutPanel3 - Большое отображение давления
│       │   │   ├── lblCurrentPressureLarge (Label, Font: 36pt) - "0.00"
│       │   │   ├── lblCurrentUnit (Label) - "PSIG"
│       │   │   └── pnlWarnIndicator (Panel) - Красный индикатор предупреждения
│       │   ├── tableLayoutPanel1 - Данные
│       │   │   ├── lblTarget (Label) - "Target"
│       │   │   ├── lblDelta (Label) - "Delta"
│       │   │   └── lblRate (Label) - "Rate"
│       │   └── tableLayoutPanel2 - ETA и Trend
│       │       ├── lblETA (Label) - "ETA"
│       │       └── lblTrend (Label) - "Trend"
│       ├── tlpSessionStats (TableLayoutPanel) - SESSION STATISTICS
│       │   ├── lblSessionStatsTitle (Label) - "SESSION STATS"
│       │   ├── lblMinLabel / lblMinValue - Min: 0.00
│       │   ├── lblMaxLabel / lblMaxValue - Max: 0.00
│       │   ├── lblAvgLabel / lblAvgValue - Average: 0.00
│       │   ├── lblStdDevLabel / lblStdDevValue - Std Dev (σ): 0.00
│       │   ├── lblPointsLabel / lblPointsValue - Points: 0
│       │   ├── lblDurationLabel / lblDurationValue - Duration: 00:00
│       │   └── lblSampleRateLabel / lblSampleRateValue - Sample Rate: 0 Hz
│       └── btnEmergency (Button, 150x43) - "Emergency Vent"
│
├── panelChartHeader (Dock: Top, 554 x 40) - Заголовок графика
│   ├── flowLegend (FlowLayoutPanel, Dock: Left) - Легенда
│   │   ├── lblLegendCurrent (Label) - "Current" (синий квадрат)
│   │   ├── _chkLegendTarget (CheckBox) + lblLegendTarget (Label) - "Target" (желтый квадрат)
│   │   ├── _chkLegendMin (CheckBox) + lblLegendMin (Label) - "Min" (зеленый квадрат)
│   │   └── _chkLegendMax (CheckBox) + lblLegendMax (Label) - "Max" (красный квадрат)
│   └── panelChartButtons (FlowLayoutPanel, Dock: Right) - Панель инструментов
│       └── [7 кнопок создаются программно в InitializeToolbar()]
│           ├── _btnCamera (Button, 32x32) - 📷 Камера (экспорт графика)
│           ├── _btnZoomToSelection (Button, 32x32) - 🔍 Zoom to Selection
│           ├── _btnPan (Button, 32x32) - ⇄ Pan
│           ├── _btnZoomIn (Button, 32x32) - ➕ Zoom In
│           ├── _btnZoomOut (Button, 32x32) - ➖ Zoom Out
│           ├── _btnFitToScreen (Button, 32x32) - ⬜ Fit to Screen
│           └── _btnHome (Button, 32x32) - 🏠 Home (Reset View)
│
├── panelCenter (Dock: Fill) - Центральная область с графиком
│   └── chartPressure (CartesianChart) - График давления (LiveCharts2)
│       └── _cursorInfoPanel (Panel, 160x95) - Панель информации при наведении (создается программно)
│
├── panelRight (Dock: Right, 200 x 483) - Правая панель настроек
│   └── tableSettings (TableLayoutPanel)
│       ├── tlpTimeWindow (TableLayoutPanel) - TIME WINDOW
│       │   ├── lblTimeWindowTitle (Label) - "TIME WINDOW"
│       │   ├── lblDuration (Label) - "Duration"
│       │   └── cmbDuration (ComboBox) - Выбор длительности (1 min, 5 min, etc.)
│       ├── tlpGrid (TableLayoutPanel) - GRID
│       │   ├── lblGridTitle (Label) - "GRID"
│       │   ├── lblYStep (Label) - "X Step"
│       │   ├── cmbXStep (ComboBox) - Шаг сетки X
│       │   ├── lblXStep (Label) - "Y Step"
│       │   └── cmbYStep (ComboBox) - Шаг сетки Y
│       ├── tlpThresholds (TableLayoutPanel) - THRESHOLDS
│       │   ├── lblThresholdsTitle (Label) - "THRESHOLDS"
│       │   ├── lblMaximum (Label) - "Maximum"
│       │   ├── nudMaximum (NumericUpDown) - Максимальное значение (красный)
│       │   ├── lblMinimum (Label) - "Minimum"
│       │   └── numericUpDown2 (NumericUpDown) - Минимальное значение (желтый)
│       ├── tlpDisplay (TableLayoutPanel) - DISPLAY
│       │   ├── lblDisplayTitle (Label) - "DISPLAY"
│       │   ├── chkShowGrid (CheckBox) - "Show Grid"
│       │   └── chkSmoothing (CheckBox) - "Smoothing"
│       ├── tlpAlerts (TableLayoutPanel) - ALERTS
│       │   ├── lblAlertsTitle (Label) - "ALERTS"
│       │   ├── lblSound (Label) - "🔔 Sound"
│       │   ├── chkSound (CheckBox)
│       │   ├── lblFlash (Label) - "⚡Flash"
│       │   └── chkFlash (CheckBox)
│       └── tlpTargetControl (TableLayoutPanel) - TARGET CONTROL
│           ├── lblTargetControlTitle (Label) - "TARGET CONTROL"
│           ├── txtTargetValue (TextBox) - Поле ввода целевого значения
│           └── btnGoTarget (Button) - "GO TARGET" (желтая кнопка)
│
└── panelBottom (Dock: Bottom, 554 x 70) - Футер
    └── footerLayout (TableLayoutPanel)
        ├── lblAutoSaveStatus (Label) - "Auto-save • Enabled" (зеленый)
        ├── lblFooterMin (Label) - "Min: 0.00"
        ├── lblFooterAvg (Label) - "Avg: 0.00"
        ├── lblFooterPoints (Label) - "Points: 0"
        └── lblThemeIndicator (Label) - "✓ Light theme" / "✓ Dark theme"
```

---

## 🔘 Кнопки (Buttons)

### В Designer (GraphForm.Designer.cs):

1. **btnPause** (Button)
   - Расположение: `panelHeader`, Location: (796, 12)
   - Размер: 75 x 23
   - Цвет: `#2A2D35` (темно-серый)
   - Создание: В Designer, инициализируется в `InitializeComponent()`
   - Иконка: Рисуется программно в `SetupHeaderLayout()` (Paint event)

2. **btnExport** (Button)
   - Расположение: `panelHeader`, Location: (697, 12)
   - Размер: 75 x 23
   - Цвет: `#2A2D35` (темно-серый)
   - Создание: В Designer, инициализируется в `InitializeComponent()`
   - Иконка: Рисуется программно в `SetupHeaderLayout()` (Paint event)

3. **btnEmergency** (Button)
   - Расположение: `panelLeft` → `tlpLeft` → Row 3
   - Размер: 150 x 43
   - Цвет: `#BF0000` (красный)
   - Текст: "Emergency Vent"
   - Создание: В Designer

4. **btnGoTarget** (Button)
   - Расположение: `panelRight` → `tableSettings` → `tlpTargetControl`
   - Размер: 64 x 43
   - Цвет: `#F0C800` (желтый)
   - Текст: "GO TARGET"
   - Создание: В Designer

5. **btnChartReset** (Button)
   - Расположение: `panelChartButtons` (старая кнопка, не используется)
   - Создание: В Designer, но заменена на тулбар

6. **btnFullscreen** (Button)
   - Расположение: `panelChartButtons` (старая кнопка, не используется)
   - Создание: В Designer, но заменена на тулбар

7. **btnReset** (Button)
   - Объявлена, но не используется в текущем дизайне
   - Создание: В Designer

8. **btnFullscreenHeader** (Button)
   - Объявлена, но не используется в текущем дизайне
   - Создание: В Designer

### Программно созданные (GraphForm.Toolbar.cs):

9. **_btnCamera** (Button)
   - Расположение: `panelChartButtons` (FlowLayoutPanel)
   - Размер: 32 x 32
   - Создание: Программно в `CreateToolbarButtonsDirectly()`
   - Функция: Экспорт графика как изображение (PNG)
   - Иконка: 📷 (эмодзи)

10. **_btnZoomToSelection** (Button)
    - Расположение: `panelChartButtons`
    - Размер: 32 x 32
    - Создание: Программно в `CreateToolbarButtonsDirectly()`
    - Функция: Zoom к выделенной области
    - Иконка: 🔍 (эмодзи)

11. **_btnPan** (Button)
    - Расположение: `panelChartButtons`
    - Размер: 32 x 32
    - Создание: Программно в `CreateToolbarButtonsDirectly()`
    - Функция: Панорамирование графика
    - Иконка: ⇄ (эмодзи)

12. **_btnZoomIn** (Button)
    - Расположение: `panelChartButtons`
    - Размер: 32 x 32
    - Создание: Программно в `CreateToolbarButtonsDirectly()`
    - Функция: Увеличение масштаба
    - Иконка: ➕ (эмодзи)

13. **_btnZoomOut** (Button)
    - Расположение: `panelChartButtons`
    - Размер: 32 x 32
    - Создание: Программно в `CreateToolbarButtonsDirectly()`
    - Функция: Уменьшение масштаба
    - Иконка: ➖ (эмодзи)

14. **_btnFitToScreen** (Button)
    - Расположение: `panelChartButtons`
    - Размер: 32 x 32
    - Создание: Программно в `CreateToolbarButtonsDirectly()`
    - Функция: Подгонка графика под экран
    - Иконка: ⬜ (эмодзи)

15. **_btnHome** (Button)
    - Расположение: `panelChartButtons`
    - Размер: 32 x 32
    - Создание: Программно в `CreateToolbarButtonsDirectly()`
    - Функция: Сброс вида графика
    - Иконка: 🏠 (эмодзи)

---

## 📋 Панели (Panels)

1. **panelHeader** (Panel)
   - Dock: Top
   - Размер: 954 x 50
   - Цвет: `#1A1D24` (темно-синий)
   - Содержит: headerLeftPanel, btnPause, btnExport

2. **panelLeft** (Panel)
   - Dock: Left
   - Размер: 200 x 483
   - Цвет: `#15171C` (темный)
   - Содержит: tlpLeft

3. **panelRight** (Panel)
   - Dock: Right
   - Размер: 200 x 483
   - Цвет: `#15171C` (темный)
   - Содержит: tableSettings

4. **panelChartHeader** (Panel)
   - Dock: Top
   - Размер: 554 x 40
   - Цвет: `#15171C` (темный)
   - Содержит: flowLegend, panelChartButtons

5. **panelCenter** (Panel)
   - Dock: Fill
   - Цвет: `#15171C` (темный)
   - Содержит: chartPressure

6. **panelBottom** (Panel)
   - Dock: Bottom
   - Размер: 554 x 70
   - Цвет: `#15171C` (темный)
   - Содержит: footerLayout

7. **appIcon** (Panel)
   - Размер: 20 x 20
   - Расположение: headerLeftPanel
   - Рисуется программно (Paint event) - 3 вертикальные полосы

8. **pnlWarnIndicator** (Panel)
   - Расположение: tlpLiveStatus → tableLayoutPanel3
   - Цвет: `#BF0000` (красный)
   - Видимость: false по умолчанию

9. **_connectionStatusPanel** (Panel)
   - Размер: 200 x 28
   - Расположение: headerLeftPanel (добавляется программно)
   - Цвет: `#1A3D35` (темно-зеленый)
   - Рисуется программно (Paint event) - зеленый кружок + текст

10. **_zoomOverlayPanel** (Panel)
    - Создается программно для отображения прямоугольника выделения при zoom
    - Прозрачный overlay поверх графика

---

## 🏷️ Метки (Labels)

### Заголовок:
- **lblAppTitle**: "Alicat Pressure Controller:"
- **lblSessionTime**: "Session: 00:00:00"

### Левая панель (Live Status):
- **lblCurrentPressureLarge**: Большое отображение давления (36pt)
- **lblCurrentUnit**: "PSIG"
- **lblTarget**: "Target"
- **lblDelta**: "Delta"
- **lblRate**: "Rate"
- **lblETA**: "ETA"
- **lblTrend**: "Trend"

### Левая панель (Session Stats):
- **lblSessionStatsTitle**: "SESSION STATS"
- **lblMinLabel / lblMinValue**: Min: 0.00
- **lblMaxLabel / lblMaxValue**: Max: 0.00
- **lblAvgLabel / lblAvgValue**: Average: 0.00
- **lblStdDevLabel / lblStdDevValue**: Std Dev (σ): 0.00
- **lblPointsLabel / lblPointsValue**: Points: 0
- **lblDurationLabel / lblDurationValue**: Duration: 00:00
- **lblSampleRateLabel / lblSampleRateValue**: Sample Rate: 0 Hz

### Легенда:
- **lblLegendCurrent**: "Current" (синий)
- **lblLegendTarget**: "Target" (желтый)
- **lblLegendMin**: "Min" (зеленый)
- **lblLegendMax**: "Max" (красный)

### Правая панель:
- **lblTimeWindowTitle**: "TIME WINDOW"
- **lblDuration**: "Duration"
- **lblGridTitle**: "GRID"
- **lblYStep**: "X Step"
- **lblXStep**: "Y Step"
- **lblThresholdsTitle**: "THRESHOLDS"
- **lblMaximum**: "Maximum"
- **lblMinimum**: "Minimum"
- **lblDisplayTitle**: "DISPLAY"
- **lblAlertsTitle**: "ALERTS"
- **lblSound**: "🔔 Sound"
- **lblFlash**: "⚡Flash"
- **lblTargetControlTitle**: "TARGET CONTROL"

### Футер:
- **lblAutoSaveStatus**: "Auto-save • Enabled"
- **lblFooterMin**: "Min: 0.00"
- **lblFooterAvg**: "Avg: 0.00"
- **lblFooterPoints**: "Points: 0"
- **lblThemeIndicator**: "✓ Light theme" / "✓ Dark theme"

### Неиспользуемые:
- **lblComPort**: Объявлена, но не используется
- **lblHotkeys**: Объявлена, но не используется
- **lblFooterMax**: Объявлена, но не используется

---

## ☑️ Чекбоксы (CheckBoxes)

1. **chkShowGrid** (CheckBox)
   - Расположение: tlpDisplay
   - Текст: "Show Grid"
   - По умолчанию: Checked

2. **chkSmoothing** (CheckBox)
   - Расположение: tlpDisplay
   - Текст: "Smoothing"
   - По умолчанию: Checked

3. **chkSound** (CheckBox)
   - Расположение: tlpAlerts
   - По умолчанию: Unchecked

4. **chkFlash** (CheckBox)
   - Расположение: tlpAlerts
   - По умолчанию: Unchecked

5. **_chkLegendTarget** (CheckBox)
   - Расположение: flowLegend
   - Создание: Программно в `SetupChartHeaderLegend()`
   - По умолчанию: Checked
   - Функция: Показать/скрыть линию Target

6. **_chkLegendMin** (CheckBox)
   - Расположение: flowLegend
   - Создание: Программно в `SetupChartHeaderLegend()`
   - По умолчанию: Checked
   - Функция: Показать/скрыть линию Min

7. **_chkLegendMax** (CheckBox)
   - Расположение: flowLegend
   - Создание: Программно в `SetupChartHeaderLegend()`
   - По умолчанию: Checked
   - Функция: Показать/скрыть линию Max

---

## 📊 Комбобоксы (ComboBoxes)

1. **cmbDuration** (ComboBox)
   - Расположение: tlpTimeWindow
   - Функция: Выбор временного окна (1 min, 5 min, 15 min, etc.)
   - Цвет фона: `#282B34`

2. **cmbXStep** (ComboBox)
   - Расположение: tlpGrid
   - Функция: Шаг сетки по оси X
   - Цвет фона: `#282B34`

3. **cmbYStep** (ComboBox)
   - Расположение: tlpGrid
   - Функция: Шаг сетки по оси Y
   - Цвет фона: `#282B34`

---

## 🔢 NumericUpDown

1. **nudMaximum** (NumericUpDown)
   - Расположение: tlpThresholds
   - Функция: Максимальное значение порога
   - Цвет текста: `#E65050` (красный)
   - По умолчанию: 150.0

2. **numericUpDown2** (NumericUpDown)
   - Расположение: tlpThresholds
   - Функция: Минимальное значение порога
   - Цвет текста: `#FFD645` (желтый)
   - По умолчанию: 10.0

---

## 📝 TextBox

1. **txtTargetValue** (TextBox)
   - Расположение: tlpTargetControl
   - Функция: Ввод целевого значения давления
   - По умолчанию: "120"
   - Цвет фона: `#2D303A`

---

## 📐 TableLayoutPanel

1. **tlpLeft** - Основной layout левой панели (4 строки)
2. **tlpLiveStatus** - Layout для LIVE STATUS (3 строки)
3. **tableLayoutPanel1** - Layout для Target/Delta/Rate (3 строки, 2 колонки)
4. **tableLayoutPanel2** - Layout для ETA/Trend (1 строка, 2 колонки)
5. **tableLayoutPanel3** - Layout для большого отображения давления (3 строки)
6. **tlpSessionStats** - Layout для статистики сессии (8 строк, 2 колонки)
7. **tableSettings** - Основной layout правой панели (6 строк)
8. **tlpTimeWindow** - Layout для TIME WINDOW (3 строки, 2 колонки)
9. **tlpGrid** - Layout для GRID (3 строки, 2 колонки)
10. **tlpThresholds** - Layout для THRESHOLDS (3 строки, 2 колонки)
11. **tlpDisplay** - Layout для DISPLAY (3 строки, 2 колонки)
12. **tlpAlerts** - Layout для ALERTS (3 строки, 2 колонки)
13. **tlpTargetControl** - Layout для TARGET CONTROL (2 строки, 2 колонки)
14. **footerLayout** - Layout для футера (1 строка, 5 колонок)

---

## 🔄 FlowLayoutPanel

1. **headerLeftPanel** (FlowLayoutPanel)
   - Расположение: panelHeader
   - Dock: Left
   - Содержит: appIcon, lblAppTitle, lblSessionTime, _connectionStatusPanel

2. **flowLegend** (FlowLayoutPanel)
   - Расположение: panelChartHeader
   - Dock: Left
   - Содержит: lblLegendCurrent, _chkLegendTarget, lblLegendTarget, _chkLegendMin, lblLegendMin, _chkLegendMax, lblLegendMax

3. **panelChartButtons** (FlowLayoutPanel)
   - Расположение: panelChartHeader
   - Dock: Right
   - Содержит: 7 кнопок тулбара (создаются программно)

---

## 📊 График

**chartPressure** (CartesianChart)
- Библиотека: LiveCharts2 (SkiaSharp)
- Расположение: panelCenter
- Dock: Fill
- Цвет фона: `#15171C`
- Серии данных:
  - _seriesCurrent (ObservableCollection<ObservablePoint>) - синяя линия
  - _seriesTarget (ObservableCollection<ObservablePoint>) - желтая линия
  - _seriesMin (ObservableCollection<ObservablePoint>) - зеленая линия
  - _seriesMax (ObservableCollection<ObservablePoint>) - красная линия

---

## 🎨 Цветовая схема

### Фоны:
- **Основной фон формы**: `#111317` (17, 19, 23)
- **Панели**: `#15171C` (21, 23, 28)
- **Заголовок**: `#1A1D24` (26, 29, 36)
- **Группы настроек**: `#20232C` (32, 35, 44)
- **Кнопки**: `#2A2D35` (42, 45, 53)
- **Индикатор подключения**: `#1A3D35` (26, 61, 53)

### Текст:
- **Основной**: `#FFFFFF` (255, 255, 255)
- **Вторичный**: `#787D8C` (120, 125, 140)
- **Светло-серый**: `#C8CFD8` (200, 205, 215)

### Серии графика:
- **Current**: `#00C8F0` (0, 200, 240) - светло-синий
- **Target**: `#F0C800` (240, 200, 0) - желтый
- **Min**: `#4CAF50` (76, 175, 80) - зеленый
- **Max**: `#F44336` (244, 67, 54) - красный

---

## 📁 Файлы структуры

1. **GraphForm.Designer.cs** - Designer-код, инициализация всех элементов
2. **GraphForm.cs** - Основная логика формы, настройка графика
3. **GraphForm.HeaderFooter.cs** - Логика заголовка и футера
4. **GraphForm.Toolbar.cs** - Логика панели инструментов (7 кнопок)

---

## 🔧 Программно создаваемые элементы

Элементы, которые создаются не в Designer, а программно:

1. **_connectionStatusPanel** - Создается в `SetupHeaderLayout()`
2. **_btnCamera, _btnZoomToSelection, _btnPan, _btnZoomIn, _btnZoomOut, _btnFitToScreen, _btnHome** - Создаются в `InitializeToolbar()` → `CreateToolbarButtonsDirectly()`
3. **_chkLegendTarget, _chkLegendMin, _chkLegendMax** - Создаются в `SetupChartHeaderLegend()`
4. **_cursorInfoPanel** - Создается в `CreateCursorInfoPanel()`
5. **_zoomOverlayPanel** - Создается в `CreateZoomOverlayPanel()`

---

## 📏 Размеры окна

- **ClientSize**: 954 x 533 пикселей
- **panelHeader**: 954 x 50
- **panelLeft**: 200 x 483
- **panelRight**: 200 x 483
- **panelChartHeader**: 554 x 40
- **panelCenter**: 554 x 373 (вычисляется автоматически)
- **panelBottom**: 554 x 70

