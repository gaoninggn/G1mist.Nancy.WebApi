using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using G1mist.Nancy.API.Model;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using G1mist.Nancy.Repository;
using Nancy;
using Nancy.Authentication.Stateless;
using Nancy.Bootstrapper;
using Nancy.Extensions;
using Nancy.Responses;
using Nancy.TinyIoc;

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

        /// <summary>
        /// 应用级别
        /// </summary>
        /// <param name="container"></param>
        /// <param name="pipelines"></param>
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.BeforeRequest += ctx =>
            {
                var url = ctx.Request.Url;

                return url.Path == "/" ? ctx.GetRedirect("~/show/index") : ctx.Response;
            };

            container.Register<IBaseRepository<tb_gather>, BaseRepository<tb_gather>>().AsSingleton();
        }

        /// <summary>
        /// 每一次Request将执行此函数，在此函数中校验用户的身份
        /// </summary>
        /// <param name="container"></param>
        /// <param name="pipelines"></param>
        /// <param name="context"></param>
        protected override void RequestStartup(TinyIoCContainer container, IPipelines pipelines, NancyContext context)
        {
            pipelines.AfterRequest += AfterRequest;

            var configuration =
                new StatelessAuthenticationConfiguration(nancyContext =>
                {
                    var authorization = nancyContext.Request.Headers.Authorization;

                    if (string.IsNullOrEmpty(authorization))
                    {
                        return null;
                    }
                    else
                    {
                        var result = CheckHeaders(context, authorization);

                        if (result)
                        {
                            return new UserIdentify
                            {
                                UserName = authorization.Split(':')[1],
                                Claims = new[]
                                {
                                     authorization.Split(':')[1]
                                }
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                });

            StatelessAuthentication.Enable(pipelines, configuration);
        }

        /// <summary>
        /// 每一个Request执行完之后，将执行此函数
        /// </summary>
        /// <param name="ctx">NancyContext</param>
        private static void AfterRequest(NancyContext ctx)
        {
            if (ctx.Response.StatusCode == HttpStatusCode.Unauthorized)
            {
                //ctx.Response =new .WithStatusCode(HttpStatusCode.Unauthorized).WithContentType("Application/json");

                ctx.Response = new JsonResponse(new ErrorResponseMessage { ErrorCode = 401, Message = "权限不足" }, new DefaultJsonSerializer()).WithStatusCode(HttpStatusCode.Unauthorized).WithContentType("Application/json");
            }
        }

        #region Validate Authorization Headers

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
        #endregion
    }
}