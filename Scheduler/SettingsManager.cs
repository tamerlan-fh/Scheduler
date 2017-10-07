using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Scheduler
{
    class SettingsManager
    {
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SettingsManager();
                return instance;
            }
        }
        private static SettingsManager instance;

        private MySettings settings;
        public TimeSpan Interval
        {
            get { return settings.Interval; }
            set { settings.Interval = value; }
        }
        public bool IsEnabled
        {
            get { return settings.IsEnabled; }
            set { settings.IsEnabled = value; }
        }
        public string Message
        {
            get { return settings.Message; }
            set { settings.Message = value; }
        }
        public bool RunTogetherWithWindows
        {
            get { return settings.RunTogetherWithWindows; }
            set { settings.RunTogetherWithWindows = value; }
        }

        public bool StateIsMinimized
        {
            get { return settings.StateIsMinimized; }
            set { settings.StateIsMinimized = value; }
        }

        public string Password
        {
            get { return settings.Password; }
            set { settings.Password = value; }
        }

        private const string filename = "scheduler.config";
        private void Load()
        {
            if (!File.Exists(filename))
            {
                settings = new MySettings(false, TimeSpan.FromHours(5), "Закройте это окно и продолжайте свои дела!", false);
            }
            else
            {
                var formatter = new BinaryFormatter();
                using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    settings = (MySettings)formatter.Deserialize(fs);
                }
            }

            settings.Password = "1";
        }
        public void Save()
        {
            var formatter = new BinaryFormatter();
            using (var fs = new FileStream(filename, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, settings);
            }
        }
        private SettingsManager() { Load(); }
    }

    [Serializable]
    class MySettings
    {
        public MySettings() { }
        public MySettings(bool isEnables, TimeSpan interval, string message, bool runTogetherWithWindows)
        {
            this.Message = message;
            this.Interval = interval;
            this.IsEnabled = isEnables;
            this.RunTogetherWithWindows = RunTogetherWithWindows;
        }
        public TimeSpan Interval { get; set; }
        public bool IsEnabled { get; set; }
        public string Message { get; set; }
        public bool RunTogetherWithWindows { get; set; }
        public bool StateIsMinimized { get; set; }
        public string Password { get; set; }
    }
}
