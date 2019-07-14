using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PangeaTemplate.Controllers
{
    [Authorize]
    public class TestController : ApiController
    {
        //https://www.c-sharpcorner.com/article/asp-net-mvc-oauth-2-0-rest-web-api-authorization-using-database-first-approach/
        public IEnumerable<string> Get()
        {
            return new string[] { "asd", "qwe", "zxc" };
        }

        public string Get(int id)
        {
            return "El id enviado es: " + id.ToString();
        }

        public void Post([FromBody]string value)
        {
            throw new NotImplementedException();
        }

        public void Put(int id, [FromBody]string value)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }
}
