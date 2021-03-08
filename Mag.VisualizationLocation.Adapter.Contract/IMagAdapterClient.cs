using System.Threading.Tasks;

namespace Mag.VisualizationLocation.Adapter.Contract
{
    public interface IMagAdapterClient
    {
        Task<RegistrationInfoResponse> GetAllRegistrationsByParamsAsync(RegistrationInfoRequest request);
        Task<RegistrationInfoDto[]> GetReistrationsByBaseStationsAsync(RegistrationsByBaseStationsRequest bsRequest);
    }

}
