using System;
namespace DT.Samples.Agora.Shared
{
    public static class EnumExtensions
    {
        public static string GetModeString(this EncryptionType value)
        {
            switch (value)
            {
                case EncryptionType.xts128:
                    return "aes-128-xts";
                case EncryptionType.xts256:
                    return "aes-256-xts";
            }
            return string.Empty;
        }

        public static string GetDescriptionString(this EncryptionType value)
        {
            switch (value)
            {
                case EncryptionType.xts128:
                    return "AES 128";
                case EncryptionType.xts256:
                    return "AES 256";
            }
            return string.Empty;
        }
    }
}
