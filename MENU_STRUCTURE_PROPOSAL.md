# Профессиональная структура меню для Alicat Controller

## Стандарты Windows приложений (Microsoft UI Guidelines)

### Общие принципы:
1. **File** - работа с данными/сессиями, экспорт, выход
2. **Edit** - редактирование (если применимо)
3. **View** - отображение, панели, темы
4. **Tools/Device** - управление устройством, инструменты
5. **Settings/Options** - настройки приложения
6. **Window** - управление окнами (если MDI)
7. **Help** - справка, документация, о программе

---

## Рекомендуемая структура для Alicat Controller

### 📁 **File** (Работа с данными и сессиями)

```
File
├── New Session...          (Ctrl+N)  ✅ Есть
├── Open Session...          (Ctrl+O)  ➕ Добавить
├── Save Session...          (Ctrl+S)  ➕ Добавить
├── Export Data...          (Ctrl+E)  ➕ Добавить
├── ────────────────────────
├── Recent Sessions         ➕ Добавить (последние 5)
│   ├── session_2025-12-28_14-30-00.csv
│   └── session_2025-12-27_10-15-00.csv
├── ────────────────────────
├── Exit                    (Alt+F4)  ➕ Добавить
```

**Обоснование:**
- Стандартные операции с файлами
- Экспорт данных критичен для лабораторий
- История сессий упрощает работу

---

### ⚙️ **Settings** (Настройки приложения)

```
Settings
├── Options...              (Ctrl+,)  ✅ Есть (переименовать в "Preferences")
│   ├── Units Tab
│   ├── Limits Tab
│   └── Advanced Tab
├── Communication...        (Ctrl+K)  ✅ Есть
├── ────────────────────────
├── Auto-save Settings      ➕ Добавить (чекбокс)
└── Reset to Defaults       ➕ Добавить
```

**Обоснование:**
- Options → Preferences (более стандартное название)
- Communication отдельно (часто используется)
- Auto-save для удобства

---

### 👁️ **View** (Отображение и интерфейс)

```
View
├── Theme
│   ├── Light Theme         ✅ Есть
│   └── Dark Theme          ✅ Есть
├── ────────────────────────
├── Show/Hide
│   ├── Status Bar          ➕ Добавить
│   ├── Toolbar             ➕ Добавить
│   └── System Settings      ➕ Добавить
├── ────────────────────────
├── Windows
│   ├── Graph               (Alt+G)  ➕ Добавить
│   ├── Table                (Alt+T)  ➕ Добавить
│   ├── Terminal             (Alt+M)  ➕ Добавить
│   └── Cascade / Tile      ➕ Добавить (если несколько окон)
└── ────────────────────────
└── Refresh                  (F5)     ➕ Добавить
```

**Обоснование:**
- Группировка по категориям
- Горячие клавиши для быстрого доступа
- Управление видимостью элементов

---

### 🔧 **Device** (Управление устройством) - НОВОЕ МЕНЮ

```
Device
├── Connect...              (Ctrl+K)  ➕ Переместить из Settings
├── Disconnect              ➕ Добавить
├── ────────────────────────
├── Emergency Stop          (Ctrl+Shift+E)  ➕ Добавить
├── ────────────────────────
├── Calibration...          ➕ Добавить (если поддерживается)
└── Device Info...          ➕ Добавить
```

**Обоснование:**
- Логично выделить управление устройством отдельно
- Emergency Stop - критичная функция безопасности
- Упрощает навигацию

---

### ❓ **Help** (Справка и информация)

```
Help
├── User Guide...           (F1)      ➕ Добавить
├── Command Reference...    ➕ Добавить
├── ────────────────────────
├── Check for Updates...    ➕ Добавить
├── ────────────────────────
├── Keyboard Shortcuts...   ➕ Добавить
├── ────────────────────────
├── About Alicat Controller...  ➕ Добавить
│   └── Version, License, Credits
```

**Обоснование:**
- Стандартная структура Help меню
- F1 для справки - стандарт Windows
- Обновления важны для научного ПО

---

## Сравнение: Текущее vs Рекомендуемое

### Текущая структура:
```
File
├── New Session
└── Start Test Mode

Settings
├── Options
└── Communication

View
├── Light Theme
└── Dark Theme

Help
└── (пусто)
```

### Рекомендуемая структура:
```
File
├── New Session
├── Open Session
├── Save Session
├── Export Data
├── Recent Sessions
└── Exit

Settings
├── Preferences (Options)
└── Auto-save Settings

Device
├── Connect
├── Disconnect
└── Emergency Stop

View
├── Theme
├── Show/Hide
├── Windows
└── Refresh

Help
├── User Guide
├── Command Reference
├── Check for Updates
└── About
```

---

## Приоритеты реализации

### Высокий приоритет (Must Have):
1. ✅ File → Exit
2. ✅ Help → About
3. ✅ View → Windows (Graph, Table, Terminal)
4. ✅ Device → Emergency Stop

### Средний приоритет (Should Have):
5. File → Export Data
6. File → Recent Sessions
7. Help → User Guide (F1)
8. View → Show/Hide панелей

### Низкий приоритет (Nice to Have):
9. File → Open/Save Session
10. Device → Device Info
11. Help → Check for Updates
12. Горячие клавиши для всех пунктов

---

## Вопросы для обсуждения

1. **Device меню**: Создавать отдельное меню или оставить Communication в Settings?
2. **Test Mode**: Оставить в File или перенести в Device/Tools?
3. **Горячие клавиши**: Нужны ли для всех функций или только для основных?
4. **Recent Sessions**: Сколько последних сессий показывать (5, 10)?

---

## Рекомендация команды

**Вариант A: Минимальные изменения (РЕКОМЕНДУЕТСЯ)**
- Добавить только критичные пункты
- Не создавать новое меню Device
- Communication оставить в Settings

**Вариант B: Полная реорганизация**
- Создать меню Device
- Переместить Communication
- Добавить все рекомендуемые пункты

**Вариант C: Поэтапная реализация**
- Фаза 1: Критичные пункты (Exit, About, Emergency Stop)
- Фаза 2: Экспорт и Recent Sessions
- Фаза 3: Остальные функции


