using Models;
using Newtonsoft.Json;
using Pos_WebApp.Utilities.ClientManagers;

namespace Pos_WebApp.Services
{
    public class ServiceBase
    {
        protected string Route {get;set;}
        protected readonly IClientManager Client;


        public ServiceBase(string route, IClientManager clientManager)
        {
            Route = route;
            Client = clientManager;
        }
        protected Response DeserializeResponseModel<T>(Response response)
        {
            if (response.Model != null)
            {
                response.Model = JsonConvert.DeserializeObject<T>(response.Model.String());
            }
            return response;
        }
    }
}
