using DocGPT.Module.BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DocGPT.Module.Services
{
    public class SettingsService
    {
        DocGPTEFCoreDbContext _dbContext;
        Settings _settings;
        public SettingsService(DocGPTEFCoreDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        private async Task InitializeSettingsAsync()
        {
            _settings = await _dbContext.Settings.FirstOrDefaultAsync();
        }
        /// <summary>
        /// Returns all settings
        /// </summary>
        /// <returns>An instance of the Settings Class</returns>
        public async Task<Settings> GetSettingsAsync()
        {
            if (_settings == null)
            {
               await InitializeSettingsAsync();
            }
            return _settings;
        }

        /// <summary>
        /// Get the value of a single settings property like ChatModel
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>the current value of the setting</returns>
        /// <exception cref="Exception"></exception>
        public async Task<object?> GetPropertyAsync(string propertyName)
        {
            if (_settings == null)
            {
                await InitializeSettingsAsync();
            }

            var property = typeof(Settings).GetProperty(propertyName);
            if (property == null)
            {
                throw new Exception($"Property '{propertyName}' not found in Settings.");
            }

            return property.GetValue(_settings);
        }

        public void SaveSettings() { }

        public void DeleteSettings() { }

        public void UpdateSettings() { }

    }
}
