namespace WebServer.Server.Http
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using Common;
    using Enums;
    using Exceptions;
    using Contracts;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            this.FormData = new Dictionary<string, string>();
            this.UrlParameters = new Dictionary<string, string>();
            this.Headers = new HttpHeaderCollection();
            this.Cookies = new HttpCookieCollection();

            this.ParseRequest(requestString);
        }

        public IDictionary<string, string> FormData { get; private set; }

        public IHttpHeaderCollection Headers { get; private set; }

        public IHttpCookieCollection Cookies { get; private set; }

        public string Path { get; private set; }

        public HttpRequestMethod RequestMethod { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public IHttpSession Session { get; set; }

        public void AddUrlParameter(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key,nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value,nameof(value));

            this.UrlParameters[key] = value;
        }

        private void ParseRequest(string requestString)
        {
            var requestLines = requestString.Split(Environment.NewLine);

            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            var firstLine = requestLines[0].Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);

            if (firstLine.Length != 3 || firstLine[2].ToLower() != "http/1.1")
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            this.RequestMethod = this.ParseMethod(firstLine[0]);
            this.Url = firstLine[1];
            this.Path = this.ParsePath(this.Url);
            this.ParseHeaders(requestLines);
            this.ParseCookies();
            this.ParseParameters();
            this.ParseFormData(requestLines.Last());
            this.SetSession();
        }

        private HttpRequestMethod ParseMethod(string methodString)
        {
            HttpRequestMethod requestMethod;

            if (!Enum.TryParse<HttpRequestMethod>(methodString, true, out requestMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            return requestMethod;
        }

        private string ParsePath(string url)
        {
            return url.Split(new []{'?','#'},StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void ParseHeaders(string[] requestLines)
        {
            var emptyLineIndex = Array.IndexOf(requestLines, string.Empty);

            for (int i = 1; i < emptyLineIndex; i++)
            {
                var currentLine = requestLines[i];
                var headerParts = currentLine.Split(new[] {": "}, StringSplitOptions.RemoveEmptyEntries);

                if (headerParts.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }

                var headerKey = headerParts[0];
                var headerValue = headerParts[1];

                var header = new HttpHeader(headerKey,headerValue);
                this.Headers.Add(header);
            }
        }

        private void ParseCookies()
        {
            if (this.Headers.ContainsKey(HttpHeader.Cookie))
            {
                var allCookies = this.Headers.Get(HttpHeader.Cookie);

                foreach (var cookie in allCookies)
                {
                    if (!cookie.Value.Contains('='))
                    {
                        return;
                    }

                    var cookieParts = cookie.Value.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                        .ToList();

                    if (!cookieParts.Any())
                    {
                        continue;
                    }

                    foreach (var cookiePart in cookieParts)
                    {
                        var cookieKeyValuePair = cookiePart.Split(new[] { '=' }, StringSplitOptions.RemoveEmptyEntries);

                        if (cookieKeyValuePair.Length == 2)
                        {
                            var cookieKey = cookieKeyValuePair[0].Trim();
                            var cookieValue = cookieKeyValuePair[1].Trim();

                            var httpCookie = new HttpCookie(cookieKey, cookieValue, false);
                            this.Cookies.Add(httpCookie);
                        }
                    }                 
                }
            }
        }

        private void ParseParameters()
        {
            if (!this.Url.Contains("?"))
            {
                return;
            }

            var query = this.Url.Split(new[] { '?' }, StringSplitOptions.RemoveEmptyEntries).Last();

            this.ParseQuery(query,this.UrlParameters);
        }

        private void ParseFormData(string formDataLine)
        {
            if (this.RequestMethod == HttpRequestMethod.Get)
            {
                return;
            }

            this.ParseQuery(formDataLine,this.FormData);
        }

        private void ParseQuery(string query, IDictionary<string,string> dict)
        {
            if (!query.Contains("="))
            {
                return;
            }

            var pairs = query.Split(new[] { '&' });

            foreach (var pair in pairs)
            {
                var pairElements = pair.Split(new[] { '=' });

                if (pairElements.Length != 2)
                {
                    return;
                }

                var key = WebUtility.UrlDecode(pairElements[0]);
                var value = WebUtility.UrlDecode(pairElements[1]);

                dict[key] = value;
            }
        }

        private void SetSession()
        {
            if (this.Cookies.ContainsKey(SessionStore.SessionCookieKey))
            {
                var cookie = this.Cookies.Get(SessionStore.SessionCookieKey);
                var sessionId = cookie.Value;

                this.Session = SessionStore.Get(sessionId);
            }
        }
    }
}
