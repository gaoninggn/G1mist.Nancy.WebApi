using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
using G1mist.Nancy.Model;
using Newtonsoft.Json;

namespace G1mist.Nancy.API.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var appid = "4d53bce03ec34c0a911182d4c228ee6c1";
            var appkey = "A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc=";

            var model = new tb_gather
            {
                id = 0,
                angle = 123,
                electrical = 15,
                lightstate = 1,
                lumen = 20,
                temperature = 25,
                voltage = 5,
                time = "1448537964"
            };

            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(HttpMethod.Post, "http://115.159.18.147/gather");//localhost:12008

            var md5 = CalcMD5(JsonConvert.SerializeObject(model));
            var sign = CalcSign(md5, appid, appkey);

            var httpcontent = new StringContent(JsonConvert.SerializeObject(model));
            httpcontent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            request.Headers.Authorization = new AuthenticationHeaderValue("abc", string.Format("{0}:{1}", appid, sign));
            request.Content = httpcontent;

            var result =Result(httpClient, request).Result;
            var c = result.StatusCode;

            Console.WriteLine(result);
            Console.ReadKey();
        }

        private static async Task<HttpResponseMessage> Result(HttpClient httpClient, HttpRequestMessage request)
        {
            var result = await httpClient.SendAsync(request);
            return result;
        }

        public static string CalcMD5(string content)
        {
            var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(content, "MD5");

            return hashPasswordForStoringInConfigFile.ToLower();
        }

        public static string CalcSign(string contentMD5, string appid, string appkey)
        {
            var signatureRawData = string.Format("{0}{1}", appid, contentMD5.ToLower());

            //将APPKEY转成UTF8编码
            var secretKeyByteArray = Encoding.UTF8.GetBytes(appkey);
            //将内容字符串转成Byte[],待Hash
            var signature = Encoding.UTF8.GetBytes(signatureRawData);

            //用APPKEY作为Key,计算HMAC哈希值
            using (var hmac = new HMACSHA256(secretKeyByteArray))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var serverSignature = ToHexString(signatureBytes).ToLower();
                return serverSignature;
            }
        }

        private static string ToHexString(byte[] bytes)
        {
            var hexString = string.Empty;

            if (bytes != null)
            {
                var strB = new StringBuilder();

                foreach (var t in bytes)
                {
                    strB.Append(t.ToString("X2"));
                }

                hexString = strB.ToString();
            }
            return hexString;
        }
    }
}
