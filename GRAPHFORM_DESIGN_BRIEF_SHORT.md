# GraphForm - ТЗ для дизайна

## Проект
Alicat Controller - Windows-приложение управления контроллером давления. .NET 8.0, Windows Forms, LiveCharts2, SkiaSharp. Темная/светлая тема.

## Структура
```
┌─────────────────────────────────────┐
│ HEADER (50px) - Заголовок, кнопки   │
├──────┬──────────────────┬───────────┤
│ LEFT │ CENTER (График)  │ RIGHT     │
│ 200px│ chartPressure    │ 180px     │
├──────┴──────────────────┴───────────┤
│ FOOTER - Статистика, кнопки         │
└─────────────────────────────────────┘
```

## Элементы

### HEADER (50px)
Заголовок, кнопки Reset/Fullscreen, легенда (Current синий, Target желтый), COM-порт, время сессии.

### LEFT PANEL (200px)
**Live Status (46%):** Текущее давление (большой текст), Target, Delta, Rate, ETA, Trend.
**Statistics (40%):** Min, Max, Avg, StdDev, Points, Duration, Sample Rate.
**Emergency (10%):** Красная кнопка "EMERGENCY VENT".

### CENTER - График
**Серии:** Current (синий #00C8F0), Target (желтый #F0C800), Min/Max пороги (красный).
**Интерактивность:** Курсор с панелью 160x95px (время, давление, цель, скорость). Двойной клик - сброс.
**Временные окна:** 1min, 5min, 15min, 30min, 1h, 2h, 4h, 6h, 8h, 10h.
**Сетка:** Вертикальные/горизонтальные линии, настраиваемый шаг.

### RIGHT PANEL (180px)
**Time Window:** Выпадающий список (1min-10h).
**Grid:** X Step (AUTO/ручной), Y Step (5/10/20 PSIG), Show Grid.
**Thresholds:** Minimum/Maximum (числовые поля), предупреждения при превышении.
**Display:** Show Grid, Smoothing.
**Alerts:** Flash, Sound.
**Target Control:** Поле ввода + кнопка "Go".

### FOOTER
Auto-save status, Min/Avg/Max, Points, кнопки: Pause, Export, Reset, Fullscreen.

## Цвета (Темная тема)
Фон окна: #15171C, панелей: #20232C, полей: #282B34.
Текст: #D2D7E1 (основной), #787D8C (вторичный).
Акценты: Current #00C8F0, Target #F0C800, Пороги #F44336.
Сетка: #2A2D35.

## Цвета (Светлая тема)
Фон: #F5F5F5, панели: #FFFFFF.
Текст: #212121.
Акценты: #2196F3, #FFC107.

## Размеры
Мин: 1000x700px, рекоменд: 1280x800px.
Left: 200px, Right: 180px, Header: 50px, Footer: 40-50px.
Отступы: 8-16px.

## Шрифты
Заголовки: Segoe UI 8-9pt Bold.
Основной: Segoe UI 9pt.
Большие числа: Segoe UI 24-32pt Bold.
Статистика: Consolas 9pt.

## Интерактивность
Курсор: вертикальная линия + панель 160x95px.
Обновление: 10 FPS (100ms).
Анимации: плавные переходы, fade-эффекты.

## Особенности
Промышленный вид, высокая читаемость, контрастность, темная тема приоритетна, реальное время, точность критична.
