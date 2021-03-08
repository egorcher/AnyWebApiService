using System;

namespace Mag.VisualizationLocation.Adapter.Contract
{
    public class RegistrationInfoDto
    {
        public DateTimeOffset RegistrationDate { get; set; }
        public BaseStationInfoDto BaseStation { get; set; }
    }

}
