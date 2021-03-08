using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Mag.VisualizationLocation.Adapter.Contract;
using Newtonsoft.Json;

namespace Mag.VisualizationLocation.Adapter.Client
{
    public class MagAdapterClient:IMagAdapterClient
    {
        private readonly HttpClient _httpClient;

        public MagAdapterClient(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            if(httpClient.BaseAddress==null)
                throw new Exception("Базовый адрес у http-клиента не может быть пустым");
            

        }

        public async Task<RegistrationInfoResponse> GetAllRegistrationsByParamsAsync(RegistrationInfoRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/Place/RegistrationInfo", content);
            if (!response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.UnprocessableEntity) //422 Unprocessable Entity («необрабатываемый экземпляр»);
                {
                    throw new NotFoundParmsException();
                }
                else
                {
                    throw new Exception();
                }
            }

            var result = await ConvertResponse<RegistrationInfoResponse>(response);
            return result;
        }

        public async Task<RegistrationInfoDto[]> GetReistrationsByBaseStationsAsync(RegistrationsByBaseStationsRequest bsRequest)
        {
            var content = new StringContent(JsonConvert.SerializeObject(bsRequest), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/Place/RegistrationInfoDto", content);
            var result = await ConvertResponse<RegistrationInfoDto[]>(response);
            return result;
        }

        private static async Task<T> ConvertResponse<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<T>(content);
            return result;
        }
    }
}
