using G1mist.Nancy.API.Model;
using Nancy;

namespace G1mist.Nancy.API
{
    public class IndexModule : NancyModule
    {
        public IndexModule()
            : base("/api")
        {
            Get["/"] = parameters =>
            {
                var responseModel = new ResponseMessage { ErrorCode = 0, Message = "Hello world" };

                return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(responseModel);
            };


        }
    }
}