using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows;

namespace QTrack.Utils
{
    public sealed class AppSettings
    {
        public int InitialTabIndexForDebug { get; set; }

        public double? WindowTop { get; set; }

        public double? WindowLeft { get; set; }

        public double WindowWidth { get; set; } = 800;

        public double WindowHeight { get; set; } = 600;

        public WindowState WindowState { get; set; } = WindowState.Normal;

        [JsonIgnore]
        private static string ConfigPath =>
            Path.Combine(AppContext.BaseDirectory, "app_settings.json");

        public static AppSettings Load()
        {
            // 設定ファイルはサイズが小さいので、同期処理でロードでも問題ないはず。
            if (!File.Exists(ConfigPath))
            {
                var settings = new AppSettings();
                settings.Save();
                return settings;
            }

            var json = File.ReadAllText(ConfigPath);
            return JsonSerializer.Deserialize<AppSettings>(json)
                   ?? new AppSettings();
        }

        public void Save()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize(this, options);
            File.WriteAllText(ConfigPath, json);
        }

        public async Task SaveAsync()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
            };

            var json = JsonSerializer.Serialize(this, options);
            await File.WriteAllTextAsync(ConfigPath, json);
        }
    }
}