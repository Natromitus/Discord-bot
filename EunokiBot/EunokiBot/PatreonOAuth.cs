using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace EunokiBot
{
    public class PatreonOAuth
    {
        //private static final Gson gson = new GsonBuilder().serializeNulls().enableComplexMapKeySerialization().create();
        //private static const Logger LOG = LoggerFactory.getLogger(PatreonOAuth.class);
        private static readonly string GRANT_TYPE_AUTHORIZATION = "authorization_code";
        private static readonly string GRANT_TYPE_TOKEN_REFRESH = "refresh_token";
        private string sClientID;
        private string sClientSecret;
        private string sRedirectUri;

        private static readonly string BASE_URI = "www.patreon.com";

        public PatreonOAuth(string clientID, String clientSecret, String redirectUri)
        {
            sClientID = clientID;
            sClientSecret = clientSecret;
            sRedirectUri = redirectUri;
        }

        public string GetAuthorizationURL()
        {
            UriBuilder uriBuilder = new UriBuilder();
            uriBuilder.Scheme = "https";
            uriBuilder.Host = BASE_URI;
            uriBuilder.Path = "oauth2/authorize";
            uriBuilder.Query = "response_type=code" +
                "&client_id=" + sClientID +
                "&redirect_uri=" + sRedirectUri;

            Uri uri = uriBuilder.Uri;
            return uri.ToString();
        }

        /*public TokensResponse GetTokens(string sCode) // throws IOException?
        { 
            Connection requestInfo = Jsoup.connect(PatreonAPI.BASE_URI + "/api/oauth2/token")
                .data("grant_type", GRANT_TYPE_AUTHORIZATION)
                .data("code", code)
                .data("client_id", clientID)
                .data("client_secret", clientSecret)
                .data("redirect_uri", redirectUri)
                               .ignoreContentType(true);
            string response = requestInfo.post().body().text();

            return toObject(response, TokensResponse.class);
        }*/

        public class TokensResponse
        {
            // Fields
            private string access_token;
            private string refresh_token;
            private int expires_in;
            private string scope;
            private string token_type;
            private DateTime expiresAt;

            // Properties
            public string AccessToken => access_token;
            public string RefreshToken => refresh_token;
            public int ExpiresIn => expires_in;
            public string Scope => scope;
            public string TokenType => token_type;

            // Constructor
            public TokensResponse(String access_token, string refresh_token, int expires_in, string scope, string token_type)
            {
                this.access_token = access_token;
                this.refresh_token = refresh_token;
                this.expires_in = expires_in;
                this.scope = scope;
                this.token_type = token_type;
                DateTime dateTime = DateTime.Now;
                dateTime.AddSeconds(expires_in);
                this.expiresAt = dateTime;
            }
        }
    }
}
