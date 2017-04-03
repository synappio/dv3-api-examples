using Newtonsoft.Json;
using System;
using System.Net;

namespace csharp
{
    sealed class Dv3Api
    {
        public enum Grade { A_PLUS, A, B, D, F }

        private const string API_ROOT = "https://dv3.datavalidation.com/api/v2";

        private readonly string apiKey;

        public Dv3Api(string apiKey)
        {
            this.apiKey = apiKey;
        }

        public Grade Realtime(String email)
        {
            dynamic m = this.AuthenticatedJsonRequest("/realtime/?email=" + WebUtility.UrlEncode(email));
            if (m.status != "ok")
            {
                throw new WebException("Invalid response status: " + m.status);
            }
            return From((string)m.grade);
        }

        public string GetUploadUrl(string listName, int emailColumnIndex, bool firstLineIsHeader)
        {
            return this.AuthenticatedJsonRequest(
                "/user/me/list/create_upload_url/?name=" + WebUtility.UrlEncode(listName)
                + "&email_column_index=" + emailColumnIndex + "&has_header=" + firstLineIsHeader);
        }

        public string Upload(string uploadUrl, string filePath)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization: bearer " + this.apiKey);
                byte[] response = client.UploadFile(uploadUrl, filePath);
                return (string)JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(response));
            }
        }

        public dynamic ListInfo(string listId)
        {
            return this.AuthenticatedJsonRequest("/user/me/list/" + WebUtility.UrlEncode(listId) + "/");
        }

        private Grade From(String value)
        {
            switch (value)
            {
                case "A+": return Grade.A_PLUS;
                case "A": return Grade.A;
                case "B": return Grade.B;
                case "D": return Grade.D;
                case "F": return Grade.F;
                default:
                    throw new System.ArgumentException("Invalid grade: " + value);
            }
        }

        private dynamic AuthenticatedJsonRequest(string urlSuffix)
        {
            using (var client = new WebClient())
            {
                client.Headers.Add("Authorization: bearer " + this.apiKey);
                return JsonConvert.DeserializeObject(client.DownloadString(API_ROOT + urlSuffix));
            }
        }
    }
}
