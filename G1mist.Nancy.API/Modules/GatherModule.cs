using System;
using G1mist.Nancy.API.Model;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using Nancy;
using Nancy.ModelBinding;
using Nancy.Security;
using Nancy.Validation;

namespace G1mist.Nancy.API.Modules
{
    public class GatherModule : NancyModule
    {
        public GatherModule(IBaseRepository<tb_gather> gatheRepository)
            : base("/gather")
        {
            this.RequiresAuthentication();

            Post["/"] = light =>
            {
                tb_gather gather;

                try
                {
                    gather = this.Bind<tb_gather>();
                }
                catch (Exception ex)
                {
                    var responseModel = new ResponseMessage { ErrorCode = 15000, Message = ex.Message };
                    return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(responseModel);
                }

                var validateResult = this.Validate(gather);

                if (gather == null)
                {
                    var responseModel = new ResponseMessage { ErrorCode = 10006, Message = "Parameters empty" };

                    return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(responseModel);
                }
                else if (!validateResult.IsValid)
                {
                    var responseModel = new ResponseMessage { ErrorCode = 10008, Message = "Parameters is error" };

                    return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(responseModel);
                }
                else
                {
                    var result = gatheRepository.Insert(gather);

                    if (result)
                    {
                        var responseModel = new ResponseMessage { ErrorCode = 0, Message = "Success" };

                        return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(responseModel);
                    }
                    else
                    {
                        var responseModel = new ResponseMessage { ErrorCode = 10007, Message = "Database too busy" };

                        return Negotiate.WithStatusCode(HttpStatusCode.OK).WithHeader("content-type", "application/json").WithModel(responseModel);
                    }
                }
            };
        }
    }
}