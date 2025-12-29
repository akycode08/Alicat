# План улучшений GraphForm

## ✅ Выполнено:
1. ✅ Создан GraphForm.Statistics.cs с логикой расчета статистики
2. ✅ Интегрированы вызовы CalculateAndUpdateStatistics() и UpdateLiveStatus()
3. ✅ Методы расчета Min, Max, Average, StdDev, Duration, Sample Rate

## 🔄 В процессе:
4. ⏳ Добавление Session Stats панели в Designer
5. ⏳ Улучшение Live Status панели (большое отображение давления)
6. ⏳ Добавление Header панели
7. ⏳ Добавление Footer панели
8. ⏳ Добавление Target Control панели
9. ⏳ Интеграция с главной формой (Pause, темы)

## 📝 Детали реализации:

### Session Stats панель (строка 2 в tlpLeft):
- TableLayoutPanel tlpSessionStats
- Labels: Min, Max, Average, Std Dev, Points, Duration, Sample Rate
- Значения обновляются при каждом AddSample()

### Live Status улучшения:
- Label lblCurrentPressureLarge (большой шрифт, ~36pt)
- Label lblCurrentUnit
- Panel pnlWarnIndicator (желтый кружок)

### Header панель:
- COM порт и время
- Горячие клавиши
- Кнопки: Pause, Export, Reset, Fullscreen

### Footer панель:
- Auto-save статус
- Статистика (Min, Max, Avg, Points)
- Индикатор темы

### Target Control:
- TextBox для Target Value
- Button "GO TARGET"


