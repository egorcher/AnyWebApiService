namespace Mag.VisualizationLocation.Adapter.Contract
{
    public class RegistrationsByBaseStationsRequest
    {
        public string QueryParamsHashCode { get; set; }
        public BaseStationIdentifierDto[] BaseStationIds { get; set; }
    }

}
