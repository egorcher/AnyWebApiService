using System;
using Mag.VisualizationLocation.Adapter.Contract;
using Mag.VisualizationLocation.Adapter.DataContext;
using Serilog;

namespace Mag.VisualizationLocation.Adapter
{
    public interface IScenario
    {
        RegistrationInfoResponse GetRegistrationInfo(RegistrationInfoRequest request);
    }
    public class Scenario:IScenario
    {
        private readonly IDecoder _decoder;
        private readonly IMtbContext _mtbContext;
        private readonly IJsonSerealizer _serealizer;
        private readonly IRegistrationInfoResponseConverter _registrationInfoResponseConverter;
        private readonly ILogger _logger;

        public Scenario(IDecoder decoder, IMtbContext mtbContext, IJsonSerealizer serealizer,
            IRegistrationInfoResponseConverter registrationInfoResponseConverter, ILogger logger)
        {
            _decoder = decoder ?? throw new ArgumentNullException(nameof(decoder));
            _mtbContext = mtbContext ?? throw new ArgumentNullException(nameof(mtbContext));
            _serealizer = serealizer ?? throw new ArgumentNullException(nameof(serealizer));
            _registrationInfoResponseConverter = registrationInfoResponseConverter;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        public RegistrationInfoResponse GetRegistrationInfo(RegistrationInfoRequest request)
        {
            if (request == null || string.IsNullOrWhiteSpace(request.QueryParamsHashCode))
            {
                _logger.Error("Неверный фильтр");
                throw new NotFoundParmsException();
            }
            
            Filter filter;
            try
            {
                var decodeRequest = _decoder.EncryptDecrypt(request.QueryParamsHashCode);
                filter = _serealizer.Serealaze<Filter>(decodeRequest);
            }
            catch (Exception exception)
            {
                _logger.Error("Ошибка получения фильра", exception);
                throw new NotFoundParmsException();
            }

            if (!filter.IsCorrectFilter())
            {
                _logger.Error("Не корректный фильр");
                throw new NotFoundParmsException();
            }

            try
            {
                var places = _mtbContext.GetPlaces(filter);
                return _registrationInfoResponseConverter.Convert(filter, places);
            }
            catch (Exception e)
            {
                _logger.Error("Ошибка получения данных",e);
                throw e;
            }

            
        }
    }
}
