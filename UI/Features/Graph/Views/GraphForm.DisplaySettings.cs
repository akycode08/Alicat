using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Alicat.UI.Features.Graph.Views
{
    public partial class GraphForm
    {
        // GetSettingsFilePath уже определен в GraphForm.GoToTarget.cs как static, используем его

        /// <summary>
        /// Сохраняет секцию Display в settings.json
        /// </summary>
        private void SaveDisplaySettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (string.IsNullOrEmpty(settingsPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[SaveDisplaySettings] Settings path is empty");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[SaveDisplaySettings] Saving Display section to: {settingsPath}");

                // Читаем существующий файл
                JsonObject? settingsData = null;
                if (File.Exists(settingsPath))
                {
                    try
                    {
                        string json = File.ReadAllText(settingsPath);
                        settingsData = JsonSerializer.Deserialize<JsonObject>(json);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[SaveDisplaySettings] Error reading existing settings: {ex.Message}");
                        settingsData = new JsonObject();
                    }
                }
                else
                {
                    settingsData = new JsonObject();
                }

                if (settingsData == null)
                {
                    settingsData = new JsonObject();
                }

                // Создаем секцию GraphForm если её нет
                if (!settingsData.ContainsKey("GraphForm"))
                {
                    settingsData["GraphForm"] = new JsonObject();
                }

                var graphFormObj = settingsData["GraphForm"] as JsonObject;
                if (graphFormObj == null)
                {
                    graphFormObj = new JsonObject();
                    settingsData["GraphForm"] = graphFormObj;
                }

                // Создаем секцию Display если её нет
                if (!graphFormObj.ContainsKey("Display"))
                {
                    graphFormObj["Display"] = new JsonObject();
                }

                var displayObj = graphFormObj["Display"] as JsonObject;
                if (displayObj == null)
                {
                    displayObj = new JsonObject();
                    graphFormObj["Display"] = displayObj;
                }

                // Сохраняем значения checkbox'ов
                displayObj["ShowGrid"] = chkShowGrid?.Checked ?? true;
                displayObj["ShowTarget"] = chkShowTarget?.Checked ?? true;
                displayObj["ShowMax"] = chkShowMax?.Checked ?? true;
                displayObj["ShowMin"] = chkShowMin?.Checked ?? true;

                System.Diagnostics.Debug.WriteLine($"[SaveDisplaySettings] Values: ShowGrid={displayObj["ShowGrid"]}, ShowTarget={displayObj["ShowTarget"]}, ShowMax={displayObj["ShowMax"]}, ShowMin={displayObj["ShowMin"]}");

                // Создаем директорию если её нет
                string? directory = Path.GetDirectoryName(settingsPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Сохраняем файл
                string jsonOutput = JsonSerializer.Serialize(settingsData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(settingsPath, jsonOutput);
                System.Diagnostics.Debug.WriteLine($"[SaveDisplaySettings] Display settings saved successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SaveDisplaySettings] Error saving Display settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает секцию Display из settings.json
        /// </summary>
        private void LoadDisplaySettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (string.IsNullOrEmpty(settingsPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Settings path is empty");
                    return;
                }

                if (!File.Exists(settingsPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Settings file doesn't exist: {settingsPath}");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Loading Display section from: {settingsPath}");

                string json = File.ReadAllText(settingsPath);
                var settingsData = JsonSerializer.Deserialize<JsonObject>(json);

                if (settingsData == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Failed to parse settings file");
                    return;
                }

                // Проверяем наличие секции GraphForm
                if (!settingsData.TryGetPropertyValue("GraphForm", out var graphFormNode) || graphFormNode is not JsonObject graphFormObj)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] GraphForm section not found");
                    return;
                }

                // Проверяем наличие секции Display
                if (!graphFormObj.TryGetPropertyValue("Display", out var displayNode) || displayNode is not JsonObject displayObj)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Display section not found");
                    return;
                }

                // Загружаем значения checkbox'ов
                if (displayObj.TryGetPropertyValue("ShowGrid", out var showGrid) && showGrid != null)
                {
                    bool value = showGrid.GetValue<bool>();
                    if (chkShowGrid != null)
                    {
                        chkShowGrid.Checked = value;
                        System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] ShowGrid loaded: {value}");
                    }
                }

                if (displayObj.TryGetPropertyValue("ShowTarget", out var showTarget) && showTarget != null)
                {
                    bool value = showTarget.GetValue<bool>();
                    if (chkShowTarget != null)
                    {
                        chkShowTarget.Checked = value;
                        System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] ShowTarget loaded: {value}");
                    }
                }

                if (displayObj.TryGetPropertyValue("ShowMax", out var showMax) && showMax != null)
                {
                    bool value = showMax.GetValue<bool>();
                    if (chkShowMax != null)
                    {
                        chkShowMax.Checked = value;
                        System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] ShowMax loaded: {value}");
                    }
                }

                if (displayObj.TryGetPropertyValue("ShowMin", out var showMin) && showMin != null)
                {
                    bool value = showMin.GetValue<bool>();
                    if (chkShowMin != null)
                    {
                        chkShowMin.Checked = value;
                        System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] ShowMin loaded: {value}");
                    }
                }

                System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Display settings loaded successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LoadDisplaySettings] Error loading Display settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Сохраняет секцию Duration в settings.json
        /// </summary>
        private void SaveDurationSettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (string.IsNullOrEmpty(settingsPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[SaveDurationSettings] Settings path is empty");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[SaveDurationSettings] Saving Duration section to: {settingsPath}");

                // Читаем существующий файл
                JsonObject? settingsData = null;
                if (File.Exists(settingsPath))
                {
                    try
                    {
                        string json = File.ReadAllText(settingsPath);
                        settingsData = JsonSerializer.Deserialize<JsonObject>(json);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"[SaveDurationSettings] Error reading existing settings: {ex.Message}");
                        settingsData = new JsonObject();
                    }
                }
                else
                {
                    settingsData = new JsonObject();
                }

                if (settingsData == null)
                {
                    settingsData = new JsonObject();
                }

                // Создаем секцию GraphForm если её нет
                if (!settingsData.ContainsKey("GraphForm"))
                {
                    settingsData["GraphForm"] = new JsonObject();
                }

                var graphFormObj = settingsData["GraphForm"] as JsonObject;
                if (graphFormObj == null)
                {
                    graphFormObj = new JsonObject();
                    settingsData["GraphForm"] = graphFormObj;
                }

                // Создаем секцию Duration если её нет
                if (!graphFormObj.ContainsKey("Duration"))
                {
                    graphFormObj["Duration"] = new JsonObject();
                }

                var durationObj = graphFormObj["Duration"] as JsonObject;
                if (durationObj == null)
                {
                    durationObj = new JsonObject();
                    graphFormObj["Duration"] = durationObj;
                }

                // Сохраняем текущий индекс страницы
                durationObj["SelectedIndex"] = _currentPageIndex;

                System.Diagnostics.Debug.WriteLine($"[SaveDurationSettings] SelectedIndex: {_currentPageIndex}");

                // Создаем директорию если её нет
                string? directory = Path.GetDirectoryName(settingsPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // Сохраняем файл
                string jsonOutput = JsonSerializer.Serialize(settingsData, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                File.WriteAllText(settingsPath, jsonOutput);
                System.Diagnostics.Debug.WriteLine($"[SaveDurationSettings] Duration settings saved successfully");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[SaveDurationSettings] Error saving Duration settings: {ex.Message}");
            }
        }

        /// <summary>
        /// Загружает секцию Duration из settings.json
        /// </summary>
        private void LoadDurationSettings()
        {
            try
            {
                string settingsPath = GetSettingsFilePath();
                if (string.IsNullOrEmpty(settingsPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Settings path is empty");
                    return;
                }

                if (!File.Exists(settingsPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Settings file doesn't exist: {settingsPath}");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Loading Duration section from: {settingsPath}");

                string json = File.ReadAllText(settingsPath);
                var settingsData = JsonSerializer.Deserialize<JsonObject>(json);

                if (settingsData == null)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Failed to parse settings file");
                    return;
                }

                // Проверяем наличие секции GraphForm
                if (!settingsData.TryGetPropertyValue("GraphForm", out var graphFormNode) || graphFormNode is not JsonObject graphFormObj)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] GraphForm section not found");
                    return;
                }

                // Проверяем наличие секции Duration
                if (!graphFormObj.TryGetPropertyValue("Duration", out var durationNode) || durationNode is not JsonObject durationObj)
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Duration section not found");
                    return;
                }

                // Загружаем SelectedIndex
                if (durationObj.TryGetPropertyValue("SelectedIndex", out var selectedIndex) && selectedIndex != null)
                {
                    int index = selectedIndex.GetValue<int>();
                    // Проверяем, что индекс валидный (0-5)
                    // PaginationData определен в GraphForm.cs, используем константу 6 (количество страниц)
                    const int MaxPageIndex = 5; // 0-5 (6 страниц: 5M, 15M, 1H, 4H, 10H, ALL)
                    if (index >= 0 && index <= MaxPageIndex)
                    {
                        _currentPageIndex = index;
                        System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] SelectedIndex loaded: {index}");
                        
                        // Применяем загруженный индекс (без сохранения, чтобы избежать циклических вызовов)
                        _timeWindowSeconds = GetDurationSeconds(index);
                        _gridStepXSeconds = GetAutoGridStepX(_timeWindowSeconds);
                        UpdateGridStepXDisplay();
                        UpdatePaginationButtonStates(index);
                        RedrawFromStore();
                        ApplyGridSettings();
                        UpdateCustomLabelsX();
                        System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Duration applied successfully");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Invalid SelectedIndex: {index}, using default (0)");
                    }
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] SelectedIndex not found in Duration section");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[LoadDurationSettings] Error loading Duration settings: {ex.Message}");
            }
        }
    }
}
