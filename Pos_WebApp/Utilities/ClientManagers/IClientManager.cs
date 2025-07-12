using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Threading.Tasks;



namespace Pos_WebApp.Utilities.ClientManagers
{
    public interface IClientManager
    {
        Task<T> Get<T>(string url, string token = null);
        //Task<T> Get<T>(string url, string baseAddress, string token = null);
        //Task<T> Get<T>(string url);
        Task<T> Post<T>(string url, object obj, string token = null);
        Task<T> Post<T>(string url, object obj, IFormFile file, string token = null);

        //Task<T> Post<T>(string url, string baseAddress, object obj, string token = null);
        Task<T> ExecuteRequestAsync<T>(HttpResponseMessage response);
        //HttpClient CreateClient(string baseAddress);
    }
}
