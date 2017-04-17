// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace DT.Samples.Agora.Shared.Helpers
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public class AgoraSettings : BasePropertyChanged
    {
        public const int MaxPossibleBitrate = 300;
        public const int MinPossibleBitrate = 100;

        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        static AgoraSettings _settings;
        public static AgoraSettings Current
        {
            get { return _settings ?? (_settings = new AgoraSettings()); }
        }

        #region Setting Constants

        private const string ProfileKey = "profile_key";
        private static readonly int ProfileDefault = 30;

        private const string UseMySettingsKey = "useMySettings_key";
        private static readonly bool UseMySettingsDefault = false;

        private const string RoomNameKey = "roomName_key";
        private static readonly string RoomNameDefault = "drmtm.us";

        private const string EncryptionPhraseKey = "encryptionPhrase_key";
        private static readonly string EncryptionPhraseDefault = "";

        private const string EncryptionTypeKey = "encryptionTypeKey_key";
        private static readonly int EncryptionTypeDefault = (int)EncryptionType.xts128;

        #endregion

        public int Profile
        {
            get
            {
                return AppSettings.GetValueOrDefault<int>(ProfileKey, ProfileDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(ProfileKey, value);
                OnPropertyChanged();
            }
        }

        public bool UseMySettings
        {
            get
            {
                return AppSettings.GetValueOrDefault<bool>(UseMySettingsKey, UseMySettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<bool>(UseMySettingsKey, value);
                OnPropertyChanged();
            }
        }

        public string RoomName
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(RoomNameKey, RoomNameDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(RoomNameKey, value);
                OnPropertyChanged();
            }
        }

        public string EncryptionPhrase
        {
            get
            {
                return AppSettings.GetValueOrDefault<string>(EncryptionPhraseKey, EncryptionPhraseDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<string>(EncryptionPhraseKey, value);
                OnPropertyChanged();
            }
        }

        public EncryptionType EncryptionType
        {
            get
            {
                return (EncryptionType)AppSettings.GetValueOrDefault<int>(EncryptionTypeKey, EncryptionTypeDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue<int>(EncryptionTypeKey, (int)value);
                OnPropertyChanged();
            }
        }
    }
}