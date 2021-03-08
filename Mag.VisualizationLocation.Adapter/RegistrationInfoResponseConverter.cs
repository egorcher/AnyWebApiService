using System.Linq;
using Mag.VisualizationLocation.Adapter.Contract;

namespace Mag.VisualizationLocation.Adapter
{
    public interface IRegistrationInfoResponseConverter
    {
        RegistrationInfoResponse Convert(Filter filter, Place[] places);
    }
    public class RegistrationInfoResponseConverter : IRegistrationInfoResponseConverter
    {
        public RegistrationInfoResponse Convert(Filter filter, Place[] places)
        {
            return new RegistrationInfoResponse
            {
                QueryParams = new QueryParamsDto
                {
                    ControlObject = ToControlObject(filter.IssValue, filter.IssType),
                    OtmCipher = filter.OtmCipher,
                    Period = new PeriodDto
                    {
                        Begin = filter.From,
                        End = filter.To
                    }
                },
                RegistrationInfos = ToRegistrationInfos(places)
            }; ;
        }

        private ControlObjectDto ToControlObject(string issValue, IssType issType)
        {
            if (issValue == null)
                return null;

            return new ControlObjectDto
            {
                IssType = (Mag.VisualizationLocation.Adapter.Contract.IssType)issType,
                IssValue = issValue
            };
        }

        private RegistrationInfoDto[] ToRegistrationInfos(Place[] places)
        {
            return places?.Select(pl =>
            {
                return new RegistrationInfoDto
                {
                    RegistrationDate = pl.InDate,
                    BaseStation = new BaseStationInfoDto
                    {
                        Identifier = new BaseStationIdentifierDto
                        {
                            Cid = pl.Cid,
                            Lac = pl.Lac
                        },
                        Longitude = pl.Longitude,
                        Latitude = pl.Latitude,
                        OperatorCode = new OperatorCodeDto
                        {
                            MNC = pl.MNC,
                            MCC = pl.MCC
                        }
                    }
                };
            }).ToArray();
        }
    }
}
