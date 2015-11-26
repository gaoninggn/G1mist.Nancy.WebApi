using System.Runtime.InteropServices;
using CsQuery.ExtensionMethods;
using G1mist.Nancy.IRepository;
using G1mist.Nancy.Model;
using G1mist.Nancy.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nancy;
using Nancy.Testing;

namespace G1mist.Nancy.API.Test
{
    [TestClass]
    public class TestApi
    {
        [TestMethod]
        public void Should_be_return_200()
        {
            var bowser = new Browser(with =>
            {
                with.Module<IndexModule>();
                with.Dependencies<IBaseRepository<t_user>>(new BaseRepository<t_user>());
            });
            var response = bowser.Get("api/", with => with.Header("Accept", "Application/json"));

            Assert.AreEqual(response.StatusCode, HttpStatusCode.OK);
        }

        [TestMethod]
        public void Should_be_return_HelloWorld()
        {
            var bowser = new Browser(with =>
            {
                with.Module<IndexModule>();
                with.Dependencies<IBaseRepository<t_user>>(new BaseRepository<t_user>());
            });
            var response = bowser.Get("api/", with => with.Header("Accept", "Application/json"));

            Assert.IsTrue(response.Body.AsString().Contains("Hello"));
        }

        [TestMethod]
        public void Should_be_return_401()
        {
            var bowser = new Browser(with =>
            {
                with.Module<IndexModule>();
                with.Dependencies<IBaseRepository<t_user>>(new BaseRepository<t_user>());
            });
            var response = bowser.Post("api/values", with =>
            {
                with.Header("Accept", "Application/json");
                with.Header("Content-Type", "Application/json");
            });

            Assert.AreEqual(response.StatusCode, HttpStatusCode.Unauthorized);
        }
    }
}
