# GO TO TARGET - Логика работы (Графическое представление)

## Архитектура системы

```
┌─────────────────────────────────────────────────────────────────────┐
│                          MainPresenter                               │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │  InitializeSequenceService()                                  │  │
│  │  ├─ Создает SequenceService                                   │  │
│  │  ├─ Передает GetCurrentPressure() делегат                     │  │
│  │  ├─ Передает SetTargetPressure() делегат                      │  │
│  │  └─ Вызывает LoadTargetsFromFile() при старте                │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                          │                                           │
│                          │ владеет                                  │
│                          ▼                                           │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │              SequenceService (фоновая логика)                 │  │
│  │  • _targets: List<TargetItem>                                 │  │
│  │  • _currentTargetIndex: int                                   │  │
│  │  • _sequenceState: SequenceState                              │  │
│  │  • _holdTimer: Timer (Interval: 1000ms)                      │  │
│  │  • _holdStartTime: DateTime                                   │  │
│  │  • _holdDurationSeconds: int                                  │  │
│  │  • _holdTimerStarted: bool                                    │  │
│  │                                                               │  │
│  │  Методы:                                                      │  │
│  │  • Start() → StartCurrentTarget()                            │  │
│  │  • Pause() → останавливает _holdTimer                        │  │
│  │  • Stop() → сбрасывает статусы, останавливает таймер        │  │
│  │  • Skip() → MoveToNextTarget()                               │  │
│  │  • SetTargets() → обновляет _targets, сохраняет в файл      │  │
│  │  • HoldTimer_Tick() → проверяет давление, отсчитывает Hold  │  │
│  │                                                               │  │
│  │  События:                                                     │  │
│  │  • OnSequenceStateChanged                                    │  │
│  │  • OnTargetChanged                                           │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
                          │
                          │ передается через
                          │ SetSequenceService()
                          ▼
┌─────────────────────────────────────────────────────────────────────┐
│                          GraphForm                                   │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │              GraphForm.GoToTarget.cs (UI слой)               │  │
│  │  • _sequenceService: SequenceService? (ссылка)               │  │
│  │  • _targets: List<TargetItem> (копия для UI)                 │  │
│  │  • _currentTargetIndex: int (копия)                          │  │
│  │  • _sequenceState: SequenceState (копия)                     │  │
│  │  • _holdTimer: Timer (UI таймер для обновления)              │  │
│  │                                                               │  │
│  │  Методы:                                                      │  │
│  │  • SetSequenceService() → подписывается на события           │  │
│  │  • SyncTargetsFromService() → копирует данные из сервиса     │  │
│  │  • InitializeGoToTarget() → настройка UI, запуск таймера     │  │
│  │  • HoldTimer_Tick() → SyncTargets + UpdateHoldTimerFromService││
│  │  • BtnPlay_Click() → _sequenceService.Start()                │  │
│  │  • BtnPause_Click() → _sequenceService.Pause()               │  │
│  │  • BtnStop_Click() → _sequenceService.Stop()                 │  │
│  │  • BtnSkip_Click() → _sequenceService.Skip()                 │  │
│  │                                                               │  │
│  │  Обработчики событий:                                         │  │
│  │  • OnSequenceServiceStateChanged() → SyncTargetsFromService()││
│  │  • OnSequenceServiceTargetChanged() → SyncTargetsFromService()││
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

## Жизненный цикл последовательности

```
┌─────────────────────────────────────────────────────────────────────┐
│                    СОЗДАНИЕ И ИНИЦИАЛИЗАЦИЯ                          │
└─────────────────────────────────────────────────────────────────────┘

1. MainPresenter.Constructor()
   │
   ├─► InitializeSequenceService()
       │
       ├─► new SequenceService(GetCurrentPressure, SetTargetPressure)
       │   │
       │   ├─► Инициализирует _holdTimer (Interval: 1000ms)
       │   └─► Подписывает HoldTimer_Tick()
       │
       └─► _sequenceService.LoadTargetsFromFile()
           │
           ├─► Читает settings.json
           ├─► Восстанавливает _targets (со статусами)
           └─► RestoreSequenceState()
               │
               └─► Если есть Active target → запускает _holdTimer

2. MainPresenter.ShowGraph()
   │
   └─► new GraphForm(_dataStore)
       │
       ├─► InitializeComponent()
       └─► InitializeGoToTarget()
           │
           ├─► Настраивает DataGridView (колонки)
           ├─► Подписывает обработчики кнопок
           ├─► Создает _holdTimer (UI таймер)
           │
           ├─► ЕСЛИ _sequenceService != null:
           │   │
           │   ├─► SyncTargetsFromService()
           │   │   ├─► Копирует _targets из сервиса
           │   │   ├─► Копирует _currentTargetIndex
           │   │   ├─► Копирует _sequenceState
           │   │   ├─► Обновляет таблицу UI
           │   │   └─► Запускает UI таймер, если State == Playing
           │   │
           │   └─► (НЕ загружает из файла!)
           │
           └─► ИНАЧЕ (старый способ):
               │
               ├─► LoadTargetsFromFile()
               └─► RestoreSequenceState()

3. MainPresenter.ShowGraph() (продолжение)
   │
   └─► _graphForm.SetSequenceService(_sequenceService)
       │
       ├─► Сохраняет ссылку на сервис
       ├─► Подписывается на OnSequenceStateChanged
       ├─► Подписывается на OnTargetChanged
       │
       └─► SyncTargetsFromService()
           └─► Запускает UI таймер, если State == Playing
```

## Запуск последовательности

```
┌─────────────────────────────────────────────────────────────────────┐
│                      НАЖАТИЕ КНОПКИ ▶ PLAY                           │
└─────────────────────────────────────────────────────────────────────┘

GraphForm.GoToTarget.cs
│
└─► BtnPlay_Click()
    │
    └─► _sequenceService.SetTargets(_targets)
        │
        ├─► Копирует targets в SequenceService._targets
        └─► SaveTargetsToFile() → запись в settings.json
    │
    └─► _sequenceService.Start()
        │
        ├─► FindFirstIncompleteTarget()
        │   └─► Находит первый target со статусом != Completed
        │
        └─► StartCurrentTarget()
            │
            ├─► target.Status = Active
            ├─► SaveTargetsToFile() → сохраняет статус
            ├─► _setTargetPressure(target.PSI) → устанавливает давление
            ├─► _holdDurationSeconds = target.HoldMinutes * 60
            ├─► _holdTimerStarted = false (еще не достигли цели)
            │
            ├─► OnTargetChanged?.Invoke() → событие
            └─► OnSequenceStateChanged?.Invoke() → событие
        │
        ├─► _sequenceState = Playing
        └─► OnSequenceStateChanged?.Invoke() → событие

События обрабатываются:
│
├─► GraphForm.OnSequenceServiceStateChanged()
│   │
│   └─► SyncTargetsFromService()
│       │
│       ├─► Копирует данные из сервиса
│       ├─► Обновляет UI (таблица, прогресс, кнопки)
│       └─► Запускает UI таймер, если State == Playing
│
└─► GraphForm.OnSequenceServiceTargetChanged()
    │
    └─► SyncTargetsFromService() → то же самое
```

## Работа Hold Timer (SequenceService)

```
┌─────────────────────────────────────────────────────────────────────┐
│              SequenceService._holdTimer Tick (каждую 1 сек)         │
└─────────────────────────────────────────────────────────────────────┘

HoldTimer_Tick()
│
├─► Проверка: _currentTargetIndex валидный?
│   └─► НЕТ → останавливает таймер, выходит
│
├─► Получает target = _targets[_currentTargetIndex]
│
├─► currentPressure = _getCurrentPressure()
│   └─► Вызывает делегат из MainPresenter
│       └─► Возвращает последнее значение из SessionDataStore
│
├─► atTarget = |currentPressure - target.PSI| <= 2.0 PSI
│
├─► ЕСЛИ НЕ atTarget:
│   │
│   ├─► _holdTimerStarted = false
│   └─► ВЫХОД (ждем следующего тика)
│
├─► ЕСЛИ atTarget И _holdTimerStarted == false:
│   │
│   ├─► _holdTimerStarted = true
│   └─► _holdStartTime = DateTime.Now
│
├─► Продолжаем отсчет:
│   │
│   ├─► elapsed = (DateTime.Now - _holdStartTime).TotalSeconds
│   ├─► remaining = _holdDurationSeconds - elapsed
│   │
│   └─► ЕСЛИ remaining <= 0:
│       │
│       ├─► _holdTimer.Stop()
│       └─► MoveToNextTarget()
│           │
│           ├─► _targets[_currentTargetIndex].Status = Completed
│           ├─► SaveTargetsToFile()
│           ├─► _currentTargetIndex++
│           │
│           ├─► ЕСЛИ _currentTargetIndex >= _targets.Count:
│           │   │
│           │   └─► Stop() → завершение последовательности
│           │
│           └─► ИНАЧЕ:
│               │
│               └─► StartCurrentTarget() → следующая цель
```

## Обновление UI (GraphForm)

```
┌─────────────────────────────────────────────────────────────────────┐
│           GraphForm._holdTimer Tick (каждую 1 сек, UI только)       │
└─────────────────────────────────────────────────────────────────────┘

HoldTimer_Tick()
│
├─► ЕСЛИ _sequenceService != null:
│   │
│   ├─► SyncTargetsFromService()
│   │   │
│   │   ├─► Копирует _targets из сервиса
│   │   ├─► Копирует _currentTargetIndex
│   │   ├─► Копирует _sequenceState
│   │   │
│   │   ├─► Управление UI таймером:
│   │   │   ├─► Если State == Playing И таймер не запущен → Start()
│   │   │   └─► Если State == Stopped И таймер запущен → Stop()
│   │   │
│   │   ├─► UpdateTargetsTable() → обновляет DataGridView
│   │   ├─► UpdateProgress() → обновляет Progress bar
│   │   └─► UpdateControlButtons() → обновляет кнопки
│   │
│   └─► UpdateHoldTimerFromService()
│       │
│       ├─► ЕСЛИ _sequenceService.CurrentTargetIndex < 0:
│       │   │
│       │   └─► lblHoldTimer.Text = "Hold: 00:00"
│       │
│       ├─► isAtTarget = _sequenceService.IsAtTarget
│       │   └─► Проверяет |currentPressure - target.PSI| <= 2.0
│       │
│       ├─► ЕСЛИ НЕ isAtTarget:
│       │   │
│       │   └─► lblHoldTimer.Text = "Hold: Approaching..."
│       │
│       └─► ЕСЛИ isAtTarget:
│           │
│           ├─► remaining = _sequenceService.HoldRemainingSeconds
│           │   └─► Вычисляет: max(0, _holdDurationSeconds - elapsed)
│           │
│           ├─► totalSeconds = _sequenceService.HoldDurationSeconds
│           │
│           ├─► Форматирует: minutes:seconds
│           ├─► lblHoldTimer.Text = $"Hold: {mm:ss}"
│           └─► progressBarHold.Value = (elapsed / total) * 100
│
└─► ИНАЧЕ (старый способ, если сервис не установлен):
    └─► Старая логика (не используется, если сервис установлен)
```

## Сохранение и загрузка состояния

```
┌─────────────────────────────────────────────────────────────────────┐
│                      СОХРАНЕНИЕ В ФАЙЛ                               │
└─────────────────────────────────────────────────────────────────────┘

SequenceService.SaveTargetsToFile()
│
├─► Читает settings.json (если существует)
│
├─► Создает JsonObject (если файл не существует)
│
├─► Создает JsonArray из _targets:
│   │
│   ├─► Для каждого target:
│   │   ├─► PSI
│   │   ├─► HoldMinutes
│   │   └─► Status (Active/Completed/Waiting)
│
├─► Сохраняет состояние:
│   ├─► SequenceTargets → массив targets
│   ├─► SequenceState → текущее состояние (Stopped/Playing/Paused)
│   ├─► CurrentTargetIndex → индекс текущего target
│   ├─► HoldStartTime → время начала Hold (ISO 8601)
│   ├─► HoldDurationSeconds → длительность Hold
│   └─► HoldTimerStarted → флаг запуска таймера
│
└─► Записывает в Settings/settings.json

┌─────────────────────────────────────────────────────────────────────┐
│                      ЗАГРУЗКА ИЗ ФАЙЛА                               │
└─────────────────────────────────────────────────────────────────────┘

SequenceService.LoadTargetsFromFile()
│
├─► Очищает _targets
│
├─► Читает Settings/settings.json
│
├─► Восстанавливает _targets (со статусами!)
│
└─► RestoreSequenceState()
    │
    ├─► Ищет target со статусом Active
    │
    ├─► ЕСЛИ нашел:
    │   │
    │   ├─► _currentTargetIndex = индекс активного target
    │   ├─► _sequenceState = Playing
    │   └─► Запускает _holdTimer (если не запущен)
    │       └─► _holdTimerStarted = false (начнется после достижения цели)
    │
    └─► ИНАЧЕ:
        └─► _sequenceState = Stopped
```

## Закрытие и повторное открытие GraphForm

```
┌─────────────────────────────────────────────────────────────────────┐
│              ЗАКРЫТИЕ GraphForm (окно закрывается)                   │
└─────────────────────────────────────────────────────────────────────┘

GraphForm закрывается
│
├─► _holdTimer.Stop() → UI таймер останавливается
│
└─► SequenceService._holdTimer ПРОДОЛЖАЕТ РАБОТУ
    └─► Последовательность выполняется в фоне!

┌─────────────────────────────────────────────────────────────────────┐
│          ПОВТОРНОЕ ОТКРЫТИЕ GraphForm (ShowGraph())                  │
└─────────────────────────────────────────────────────────────────────┘

1. new GraphForm(_dataStore)
   │
   └─► InitializeGoToTarget()
       │
       ├─► Создает новый _holdTimer (UI таймер)
       │
       └─► ЕСЛИ _sequenceService != null:
           │
           └─► SyncTargetsFromService()
               │
               ├─► Копирует данные из SequenceService
               │   └─► SequenceService продолжает работать!
               │
               ├─► Если State == Playing:
               │   └─► Запускает UI таймер → обновления начинаются
               │
               └─► Обновляет UI (таблица, прогресс, кнопки)

2. _graphForm.SetSequenceService(_sequenceService)
   │
   ├─► Подписывается на события
   │
   └─► SyncTargetsFromService()
       │
       └─► Если State == Playing:
           └─► Запускает UI таймер (если еще не запущен)
```

## Закрытие приложения

```
┌─────────────────────────────────────────────────────────────────────┐
│              ЗАКРЫТИЕ ПРИЛОЖЕНИЯ (File → Exit)                       │
└─────────────────────────────────────────────────────────────────────┘

MainPresenter.menuFileExit_Click()
│
├─► Подтверждение от пользователя
│
└─► ЕСЛИ подтверждено:
    │
    ├─► _sequenceService.ClearTargetsAndFile()
    │   │
    │   ├─► Останавливает последовательность
    │   ├─► Очищает _targets
    │   │
    │   └─► Удаляет из settings.json:
    │       ├─► SequenceTargets
    │       ├─► SequenceState
    │       ├─► CurrentTargetIndex
    │       ├─► HoldStartTime
    │       ├─► HoldDurationSeconds
    │       └─► HoldTimerStarted
    │
    └─► Application.Exit()
```

## Ключевые моменты архитектуры

```
1. ДВА ТАЙМЕРА:
   ┌─────────────────────────────────────────┐
   │ SequenceService._holdTimer              │
   │ • Работает в фоне всегда                │
   │ • Управляет логикой последовательности  │
   │ • Продолжает работать, даже если        │
   │   GraphForm закрыт                      │
   └─────────────────────────────────────────┘
   
   ┌─────────────────────────────────────────┐
   │ GraphForm._holdTimer (UI таймер)        │
   │ • Запускается только когда форма открыта│
   │ • Только для обновления UI              │
   │ • Синхронизируется с SequenceService    │
   └─────────────────────────────────────────┘

2. ДВА ИСТОЧНИКА ДАННЫХ:
   ┌─────────────────────────────────────────┐
   │ SequenceService._targets                │
   │ • Единственный источник истины          │
   │ • Сохраняется в settings.json           │
   │ • Продолжает работать при закрытии UI   │
   └─────────────────────────────────────────┘
   
   ┌─────────────────────────────────────────┐
   │ GraphForm._targets                      │
   │ • Копия для UI                          │
   │ • Синхронизируется через                │
   │   SyncTargetsFromService()              │
   └─────────────────────────────────────────┘

3. СОБЫТИЯ (Events):
   SequenceService → GraphForm
   │
   ├─► OnSequenceStateChanged
   │   └─► GraphForm.SyncTargetsFromService()
   │
   └─► OnTargetChanged
       └─► GraphForm.SyncTargetsFromService()

4. ДЕЛЕГАТЫ (Delegates):
   MainPresenter → SequenceService
   │
   ├─► GetCurrentPressure()
   │   └─► Возвращает текущее давление из SessionDataStore
   │
   └─► SetTargetPressure(double)
       └─► Устанавливает целевое давление через SetTargetSilent()
```

## Поток данных при выполнении последовательности

```
┌──────────────┐
│ User Action  │  Нажатие кнопки ▶ Play
└──────┬───────┘
       │
       ▼
┌─────────────────────────────────┐
│ GraphForm.BtnPlay_Click()       │
│  └─► _sequenceService.Start()   │
└──────┬──────────────────────────┘
       │
       ▼
┌─────────────────────────────────┐
│ SequenceService.Start()         │
│  └─► StartCurrentTarget()       │
│      ├─► Status = Active        │
│      ├─► SetTargetPressure()    │
│      └─► OnTargetChanged()      │
└──────┬──────────────────────────┘
       │
       ├──────────────────────────┐
       │                          │
       ▼                          ▼
┌─────────────────────┐  ┌──────────────────────┐
│ SequenceService     │  │ GraphForm            │
│ _holdTimer Tick     │  │ (событие получено)   │
│ (каждую 1 сек)      │  │                      │
│                     │  │ SyncTargetsFrom...() │
│ ├─► Проверка        │  │ ├─► Копирует данные  │
│ │   давления        │  │ ├─► Обновляет UI     │
│ │                   │  │ └─► Запускает UI     │
│ ├─► Если достигли:  │  │    таймер            │
│ │   _holdTimerStart │  │                      │
│ │   = true          │  │                      │
│ │                   │  │                      │
│ ├─► Отсчет времени  │  │ GraphForm._holdTimer │
│ │                   │  │ Tick (каждую 1 сек)  │
│ └─► Если время      │  │                      │
│     истекло:        │  │ ├─► SyncTargets...() │
│     MoveToNext...() │  │ └─► UpdateHoldTimer  │
│                     │  │    FromService()     │
└─────────────────────┘  └──────────────────────┘
       │                          │
       │                          │
       └──────────┬───────────────┘
                  │
                  ▼
         ┌────────────────┐
         │ UI обновлен    │
         │ • Таблица      │
         │ • Прогресс     │
         │ • Hold Timer   │
         └────────────────┘
```

## Важные особенности

```
✅ SequenceService работает НЕЗАВИСИМО от UI
   → Последовательность продолжается, даже если GraphForm закрыт

✅ Состояние сохраняется в settings.json
   → После перезапуска GraphForm состояние восстанавливается

✅ Два таймера работают синхронно
   → SequenceService._holdTimer: логика
   → GraphForm._holdTimer: только UI обновление

✅ При закрытии GraphForm:
   → UI таймер останавливается
   → SequenceService продолжает работать
   → Состояние сохраняется в файл

✅ При открытии GraphForm:
   → Данные синхронизируются из SequenceService
   → UI таймер запускается, если последовательность активна
   → UI обновляется через SyncTargetsFromService()

✅ При закрытии приложения:
   → ClearTargetsAndFile() очищает данные из settings.json
   → Последовательность НЕ сохраняется между запусками приложения
```

