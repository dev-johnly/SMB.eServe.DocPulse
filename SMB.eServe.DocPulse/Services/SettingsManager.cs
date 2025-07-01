using System.Text.Json;
using SMB.eServe.DocPulse.Models;

namespace SMB.eServe.DocPulse.Services
{
    public static class SettingsManager
    {
        private static readonly string SettingsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.json");

        public static void Save(ConversionSettings settings)
        {
            var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SettingsPath, json);
        }

        public static ConversionSettings Load()
        {
            try
            {
                if (!File.Exists(SettingsPath)) return new ConversionSettings();
                var json = File.ReadAllText(SettingsPath);
                if (string.IsNullOrWhiteSpace(json)) return new ConversionSettings(); // fallback

                return JsonSerializer.Deserialize<ConversionSettings>(json) ?? new ConversionSettings();
            }
            catch
            {
                return new ConversionSettings(); // fallback on invalid JSON
            }
        }

    }

}
