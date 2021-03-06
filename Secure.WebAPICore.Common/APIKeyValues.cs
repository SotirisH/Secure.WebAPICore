﻿using System;

namespace Secure.WebAPICore.Common
{
    /// <summary>
    /// This class contains all the key values that are used in the project
    /// </summary>
    public static class APIKeyValues
    {
        /// <summary>
        /// Application key for the example SimpleAppKey (Public Key)
        /// </summary>
        public static readonly Guid ApplicationKeyValue = new Guid("06673FAD-E3E9-43D7-9B59-EAC04E112D87");
        public static string ClientPrivateKeyKeyValue = "cE81fL+VU253rM3nebPB6jbBtKNV1NT2kcDBZcPmY+M=";


        /// <summary>
        /// The public shared key used as the user identifier
        /// </summary>
        public static readonly string ApplicationKeyAuthenticationScheme = "X-PSK";
        /// <summary>
        /// The nonce is used to help us to avoid the replay attacks in case the message has been sent within the acceptable
        /// time window
        /// </summary>
        public static readonly string NonceScheme = "X-Nonce";
        /// <summary>
        /// The value sent by the client and the UNIX time of the current time are compared. If the
        /// skew between these two are within the allowable tolerance limit, the request is not a replay.
        /// UNIX time is the number of seconds elapsed since midnight of January 1, 1970 Coordinated
        /// Universal Time(UTC)
        /// </summary>
        public static readonly string StampScheme = "X-Stamp";
        /// <summary>
        /// If the value sent by the client application in the X-Signature matches the HMAC-SHA256 of the
        /// values of X-PSK, X-Counter, X-Stamp, request URI, and the HTTP method, we can safely conclude
        /// that nothing was altered in transit
        /// </summary>
        public static readonly string SignatuerScheme = "X-Signature";
    }
}
