using DocGPT.Module.BusinessObjects;

namespace DocGPT.Module.Services
{
    public class SettingsService
    {
        private readonly DocGPTEFCoreDbContext _dbContext;
        Settings _settings;
        public SettingsService(DocGPTEFCoreDbContext dbContext) 
        {
            _dbContext = dbContext;
            //_ = InitializeSettings();
        }
        //TODO: kan dit nog anders?

        private void  InitializeSettings()

        {
            _settings = _dbContext.Settings.OrderBy(i => i.SettingsID).FirstOrDefault();
        }
        /// <summary>
        /// Returns all settings
        /// </summary>
        /// <returns>An instance of the Settings Class</returns>
        public  Settings GetSettings()
        {
            if (_settings == null)
            {
                 InitializeSettings();
            }
            return _settings;
        }

        /// <summary>
        /// Get the value of a single settings property like ChatModel
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns>the current value of the setting</returns>
        /// <exception cref="Exception"></exception>
        public  object GetProperty(string propertyName)
        {
            if (_settings == null)
            {
                 InitializeSettings();
            }

            var property = typeof(Settings).GetProperty(propertyName);
            if (property == null)
            {
                throw new Exception($"Property '{propertyName}' not found in Settings.");
            }

            return property.GetValue(_settings);
        }

    }
}
