﻿namespace Snippets5.Encryption.Configuration
{
    using System.Collections.Generic;
    using NServiceBus;

    class FromCode
    {
        public FromCode()
        {
#pragma warning disable 618
            #region EncryptionFromCode

            BusConfiguration busConfiguration = new BusConfiguration();
            string encryptionKey = "gdDbqRpqdRbTs3mhdZh9qCaDaxJXl+e6";
            List<string> expiredKeys = new List<string>
            {
                "abDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6",
                "cdDbqRpQdRbTs3mhdZh9qCaDaxJXl+e6"
            };
            busConfiguration.RijndaelEncryptionService(encryptionKey, expiredKeys);

            #endregion
#pragma warning restore 618
        }
    }
}
