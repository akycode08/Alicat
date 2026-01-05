# Implementation Notes - Alicat Controller Menu Enhancement

## ✅ Completed Components

1. **SessionConfigurationForm** (`UI/Options/SessionConfigurationForm.cs`)
   - Форма для просмотра JSON конфигурации сессии
   - Кнопки: Copy, Save, Close
   - Размер: 700x550px

2. **ThemeManager** (`Services/Settings/ThemeManager.cs`)
   - Управление Light/Dark темами
   - Статические классы для цветов
   - Методы ApplyTheme для различных контролов

## 📋 To Do - Menu Enhancement

### File Menu Structure Required:
```
File
├── New Session (Ctrl+N)
├── Open Session... (Ctrl+O)
├── ─────────────
├── Save Session (Ctrl+S)
├── Save Session As... (Ctrl+Shift+S)
├── ─────────────
├── Export (submenu)
│   ├── Table (CSV)... (Ctrl+E)
│   ├── Graph Image (PNG)...
│   └── Session Report (PDF)...
├── ─────────────
├── Recent Sessions (submenu)
│   ├── Session_2025-01-05_14-30.als
│   ├── Session_2025-01-05_10-15.als
│   ├── Session_2025-01-04_16-45.als
│   ├── ─────────────
│   └── Clear Recent List
├── ─────────────
├── Session Configuration... (Alt+Enter)
├── ─────────────
└── Exit (Alt+F4)
```

### Implementation Steps:

1. **Update AlicatForm.Designer.cs**
   - Add new menu items to menuFile.DropDownItems
   - Add separators (ToolStripSeparator)
   - Set ShortcutKeys for each item
   - Add submenus (ToolStripMenuItem with DropDownItems)

2. **Update AlicatForm.cs**
   - Add event handlers for new menu items
   - Implement Save/Open session functionality
   - Implement Export functionality
   - Implement Recent Sessions tracking

3. **SettingsForm with Tabs**
   - Create new version or update existing FormOptions
   - Add TabControl with 3 tabs: Units, Limits, Connection
   - Move existing controls to appropriate tabs

## 🔧 Quick Integration Guide

### To add SessionConfigurationForm to menu:
```csharp
private void MenuFileSessionConfiguration_Click(object sender, EventArgs e)
{
    using var form = new SessionConfigurationForm();
    form.ShowDialog(this);
}
```

### To use ThemeManager:
```csharp
using Alicat.Services.Settings;

// Toggle theme
ThemeManager.ToggleTheme();

// Apply theme to form
ThemeManager.ApplyTheme(this, ThemeManager.IsDarkMode);
```

