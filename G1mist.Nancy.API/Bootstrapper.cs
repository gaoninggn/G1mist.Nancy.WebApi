using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using G1mist.Nancy.API.Model;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using G1mist.Nancy.Repository;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Newtonsoft.Json;

namespace G1mist.Nancy.API
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        public static Dictionary<string, string> AllowApps { get; set; }

        public Bootstrapper()
        {
            AllowApps = new Dictionary<string, string>
            {
                {"4d53bce03ec34c0a911182d4c228ee6c1","A93reRTUJHsCuQSHR+L3GxqOJyDmQpCgps102ciuabc="}
            };
        }

        protected override byte[] FavIcon
        {
            get { return null; }
        }

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += CheckAuthrozation;

            container.Register<IBaseRepository<tb_gather>, BaseRepository<tb_gather>>().AsSingleton();
        }

        private static Response CheckAuthrozation(NancyContext ctx)
        {
            var url = ctx.Request.Url;
            var method = ctx.Request.Method;

            if (url.Path.Contains("/gather"))
            {
                if (method == "POST")
                {
                    var headers = ctx.Request.Headers.Authorization;

                    if (string.IsNullOrEmpty(headers))
                    {
                        return ErrorResponse(10003, "请输入验证信息");
                    }
                    else
                    {
                        var result = CheckHeaders(ctx, headers);

                        return result ? ctx.Response : ErrorResponse(10004, "验证信息不正确");
                    }
                }
                else
                {
                    return ctx.Response;
                }
            }
            else
            {
                return ctx.Response;
            }
        }

        private static bool CheckHeaders(NancyContext ctx, string headers)
        {
            var schame = GetSchame(headers);
            var content = GetContent(ctx);

            if (CheckSchame(schame))
            {
                var header = GetAuthHeaders(headers);

                if (header == null)
                {
                    return false;
                }
                else
                {
                    var result = ValidateHeaders(header[0], content, header[1]);

                    return result;
                }
            }
            else
            {
                return false;
            }
        }

        private static string GetContent(NancyContext ctx)
        {
            var bytes = new byte[ctx.Request.Body.Length];

            if (ctx.Request.Body.CanRead)
            {
                ctx.Request.Body.Read(bytes, 0, bytes.Length);

                var stringOfConteng = Encoding.UTF8.GetString(bytes);

                return string.IsNullOrEmpty(stringOfConteng) ? "" : stringOfConteng;
            }
            else
            {
                return "";
            }
        }

        private static string[] GetAuthHeaders(string headers)
        {
            var header = headers.Substring(headers.IndexOf(' ') + 1);
            var auths = header.Split(':');

            return auths.Length != 2 ? null : auths;
        }

        private static bool CheckSchame(string schame)
        {
            if (string.IsNullOrEmpty(schame))
            {
                return false;
            }
            else
            {
                return schame == "abc";
            }
        }

        private static string GetSchame(string headers)
        {
            var s = headers.Split(' ');

            return s.Length <= 0 ? "" : s[0];
        }

        private static Response ErrorResponse(int errorCode, string message)
        {
            var errorMsg = new ErrorResponseMessage { ErrorCode = errorCode, Message = message };
            var content = JsonConvert.SerializeObject(errorMsg);

            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                ContentType = "aplication/json",
                Contents =
                    stream => { stream.Write(Encoding.UTF8.GetBytes(content), 0, Encoding.UTF8.GetBytes(content).Length); }
            };
        }

        private static bool ValidateHeaders(string appid, string content, string clientSignature)
        {
            var requestContentString = "";

            if (content.Length != 0)
            {
                //计算Content的MD5Hash值
                var hashPasswordForStoringInConfigFile = FormsAuthentication.HashPasswordForStoringInConfigFile(content, "MD5");

                if (hashPasswordForStoringInConfigFile != null)
                {
                    requestContentString = hashPasswordForStoringInConfigFile.ToLower();
                }
                else
                {
                    return false;
                }
            }

            var signatureRawData = string.Format("{0}{1}", appid, requestContentString.ToLower());

            //将APPKEY转成UTF8编码
            var secretKeyByteArray = Encoding.UTF8.GetBytes(AllowApps[appid]);
            //将内容字符串转成Byte[],待Hash
            var signature = Encoding.UTF8.GetBytes(signatureRawData);

            //用APPKEY作为Key,计算HMAC哈希值
            using (var hmac = new HMACSHA256(secretKeyByteArray))
            {
                var signatureBytes = hmac.ComputeHash(signature);
                var serverSignature = ToHexString(signatureBytes).ToLower();
                return serverSignature.Equals(clientSignature);
            }
        }

        public static string ToHexString(byte[] bytes)
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