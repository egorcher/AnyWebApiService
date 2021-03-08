namespace Mag.VisualizationLocation.Adapter.Contract
{
    public class BaseStationInfoDto
    {
        public OperatorCodeDto OperatorCode { get; set; }

        public BaseStationIdentifierDto Identifier { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }
    }

}
