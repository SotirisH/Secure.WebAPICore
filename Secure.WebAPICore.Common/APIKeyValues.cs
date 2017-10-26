using System;

namespace Secure.WebAPICore.Common
{
    /// <summary>
    /// This class contains all the key values that are used in the project
    /// </summary>
    public static class APIKeyValues
    {
        /// <summary>
        /// Application key for the example SimpleAppKey
        /// </summary>
        public static readonly Guid ApplicationKeyValue = new Guid("06673FAD-E3E9-43D7-9B59-EAC04E112D87");
        public static readonly string ApplicationKeyAuthenticationScheme = "app-key";
    }
}
