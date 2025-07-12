using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using StatusCodesEnums = Models.Enums.StatusCodes;

namespace Pos_WebApp.Utilities.ClientManagers
{
    public class ClientManager: IClientManager
    {
        private HttpClient _client;
        private IConfiguration Configuration { get; }
        private const string ApiSettingsName = "AppSettings:ApiUrl";
        private const string HttpVersionCheck = "AppSettings:UseHttp2";

        public ClientManager(IConfiguration config)
        {
            _client = new HttpClient();
            Configuration = config;
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //public async Task<T> Get<T>(string url, string baseAddress, string token = null)
        //{
        //    try
        //    {
        //        var _client = CreateClient(baseAddress);
        //        if (token != null)
        //        {
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        }
        //        return await ExecuteRequestAsync<T>(await _client.GetAsync(url).ConfigureAwait(false)).ConfigureAwait(false);
        //    }
        //    catch (Exception)
        //    {
        //        throw new Exception();
        //    }
        //}
        public async Task<T> Get<T>(string url, string token = null)
        {
            try
            {
                var httpClient = CreateClient(GetApiUrl());
                if (token!=null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                return await ExecuteRequestAsync<T>(await httpClient.GetAsync(url).ConfigureAwait(false)).ConfigureAwait(false);
            }
            catch (Exception  ex)
            {
                throw ex;
            }
        }
        //public async Task<T> Post<T>(string url, string baseAddress, object obj, string token = null)
        //{
        //    try
        //    {
        //        var _client = CreateClient(baseAddress);
        //        if (token != null)
        //        {
        //            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        //        }
        //        return await ExecuteRequestAsync<T>(await _client.PostAsync(url, new StringContent(
        //            JsonConvert.SerializeObject(obj),
        //            Encoding.UTF8,
        //            "application/json"))
        //            .ConfigureAwait(false));
        //    }
        //    catch (Exception)
        //    {
        //        return await ExecuteRequestAsync<T>(await _client.PostAsync(url, new StringContent(
        //            JsonConvert.SerializeObject(obj),
        //            Encoding.UTF8,
        //            "application/json"))
        //            .ConfigureAwait(false));
        //    }

        //}
        public async Task<T> Post<T>(string url, object obj, string token = null)
        {
            try
            {
                var httpClient = CreateClient(GetApiUrl());
                if (token != null)
                {
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var response = await ExecuteRequestAsync<T>(await httpClient.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/json"))
                    .ConfigureAwait(false));
                return response;
            }
            catch (Exception )
            {
                return await ExecuteRequestAsync<T>(await _client.PostAsync(url, new StringContent(
                    JsonConvert.SerializeObject(obj),
                    Encoding.UTF8,
                    "application/json"))
                    .ConfigureAwait(false));
            }

        }
        public async Task<T> Post<T>(string url, object obj = null, IFormFile file = null, string token = null)
        {
            try
            {
                var client = CreateClient(GetApiUrl());
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                if (token != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                //var multiContent = new MultipartFormDataContent();
                var requestContent = new MultipartFormDataContent();
                
                if (file!=null)
                {
                    requestContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                    requestContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = file.FileName };
                }
                if (obj!=null)
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        object propVal = prop.GetValue(obj, null);
                        requestContent.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(Convert.ToString(propVal))), prop.Name);
                    }
                }
                var response = await client.PostAsync(url, requestContent).ConfigureAwait(false);
                return await ExecuteRequestAsync<T>(response);
            }
            catch (Exception)
            {
                var client = CreateClient(GetApiUrl());
                _client.DefaultRequestHeaders.Clear();
                _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                if (token != null)
                {
                    _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                var requestContent = new MultipartFormDataContent();

                if (file != null)
                {

                    requestContent.Add(new StreamContent(file.OpenReadStream()), "file", file.FileName);
                    requestContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data") { Name = "file", FileName = file.FileName };
                }
                if (obj != null)
                {
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        object propVal = prop.GetValue(obj, null);
                        requestContent.Add(new ByteArrayContent(Encoding.UTF8.GetBytes(Convert.ToString(propVal))), prop.Name);
                    }
                }
                var response = await client.PostAsync(url, requestContent).ConfigureAwait(false);
                return await ExecuteRequestAsync<T>(response);
            }
        }
        public async Task<T> ExecuteRequestAsync<T>(HttpResponseMessage response)
        {
            var resultContent = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode || 
                Convert.ToInt32(response.StatusCode) == Convert.ToInt32(HttpStatusCode.Unauthorized) ||
                Convert.ToInt32(response.StatusCode) == Convert.ToInt32(HttpStatusCode.NotFound) ||
                Convert.ToInt32(response.StatusCode) == Convert.ToInt32(HttpStatusCode.Conflict) ||
                Convert.ToInt32(response.StatusCode) == Convert.ToInt32(HttpStatusCode.BadRequest)||
                Convert.ToInt32(response.StatusCode) == Convert.ToInt32(StatusCodesEnums.Error_Occured)
                )
            {
                return JsonConvert.DeserializeObject<T>(resultContent, CreateDefaultJsonDeserializerSettings());
            }
            else
            {
                //switch (response.StatusCode)
                //{
                //    case HttpStatusCode.Ambiguous:
                //    case HttpStatusCode.Forbidden:
                //    case HttpStatusCode.Unauthorized:
                //    case HttpStatusCode.MethodNotAllowed:
                //    case HttpStatusCode.UnsupportedMediaType:
                //    case (HttpStatusCode)422://UnProcessable entity
                //                             //throw new HttpException();
                //    default:
                //        throw new HttpRequestException($"Unknown http exception occurred with status code: {(int)response.StatusCode}.");
                //}
                throw response.StatusCode switch
                {
                    _ => new HttpRequestException($"Unknown http exception occurred with status code: {(int)response.StatusCode}."),
                };
            }
        }
        private JsonSerializerSettings CreateDefaultJsonDeserializerSettings()
        {
            return new JsonSerializerSettings
            {
                DateFormatString = "MM-dd-yyyy",
                NullValueHandling = NullValueHandling.Ignore,
                Converters = new List<JsonConverter>()
            };
        }

        public HttpClient CreateClient(string baseAddress)
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri(baseAddress)
            };
            var check = Convert.ToBoolean(Configuration.GetSection(HttpVersionCheck).Value);
            if (check)
            {
                _client.DefaultRequestVersion = new Version(2, 0);
            }
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return _client;
        }
        private string GetApiUrl()
        {
            return Configuration.GetSection(ApiSettingsName).Value;
        }

        
    }
}
