# Read-Only Mode Implementation

## Реализовано

### 1. Enum SessionState
- **Файл**: `Domain/SessionState.cs`
- **Значения**: None, Created, Active, Completed

### 2. Модель данных SessionData
- **Файл**: `Services/Data/SessionData.cs`
- **Добавлено**: 
  - `State` (SessionState)
  - `IsReadOnly` (bool, computed property)

### 3. Сохранение/Загрузка сессий
- **Файл**: `UI/Main/AlicatForm.ReadOnlyMode.cs`
- **Методы**:
  - `SaveSessionToFile()` - сохраняет сессию в JSON файл
  - `LoadSessionFromFile()` - загружает сессию из файла
  - `CreateSessionDataFromCurrent()` - создает SessionData из текущей сессии
  - `LoadSessionDataIntoStore()` - загружает данные в DataStore

### 4. Read-Only режим
- **Файл**: `UI/Main/AlicatForm.ReadOnlyMode.cs`
- **Методы**:
  - `UpdateReadOnlyMode()` - обновляет состояние read-only режима
  - `UpdateUIForReadOnlyMode()` - обновляет UI элементы
  - `UpdateWindowTitle()` - обновляет заголовок окна
  - `UpdateReadOnlyBanner()` - показывает/скрывает баннер

### 5. Обработчики меню
- **Файл**: `UI/Main/AlicatForm.MenuExtensions.cs`
- **Обновлено**:
  - `MenuFileOpenSession_Click()` - загружает сессию и включает read-only если нужно
  - `MenuFileSaveSession_Click()` - блокирует сохранение в read-only режиме
  - `MenuFileSaveSessionAs_Click()` - позволяет создать копию read-only сессии

### 6. Визуальные индикаторы
- **Баннер**: Оранжевая панель вверху формы с текстом "⚠ READ-ONLY MODE"
- **Заголовок окна**: Добавляется "[Read-Only]" к имени файла
- **Отключенные элементы**: 
  - btnIncrease
  - btnDecrease
  - btnGoToTarget
  - txtTargetInput
  - menuFileSaveSession

### 7. Автоматическое сохранение
- При завершении сессии (OnSessionEnded) автоматически сохраняется как Completed
- Read-only режим включается автоматически

## Формат файла сессии (.als)

JSON файл с полями:
```json
{
  "SessionName": "Session_2026-01-07_18-01-57",
  "CreatedDate": "2026-01-07T18:01:57",
  "LastModified": "2026-01-07T01:04:52",
  "Duration": "07:03:00",
  "Status": "Completed",
  "State": 3,  // Completed
  "Operator": "User",
  "DeviceModel": "ALICAT PC-15PSIG-D",
  "ComPort": "COM3",
  "BaudRate": 19200,
  "PressureUnit": "PSIG",
  "MaxPressure": 200.0,
  "MinPressure": 0.0,
  "TotalDataPoints": 10200,
  ...
}
```

## Использование

1. **Создание новой сессии**: File → New Session
2. **Сохранение сессии**: File → Save Session (автоматически помечается как Completed при завершении)
3. **Открытие сессии**: File → Open Session
   - Если State == Completed → включается read-only режим
   - Показывается предупреждение
4. **Сохранение копии**: File → Save Session As (работает даже для read-only сессий)

## TODO (будущие улучшения)

1. Загрузка точек данных из файла сессии в DataStore
2. Добавление панели read-only баннера в Designer (вместо программного создания)
3. Сохранение состояния read-only в статус-баре
4. Обработка закрытия формы с активной сессией (предложение сохранить)

