using System;
using System.Collections.Generic;
using FluentAssertions;
using Mag.VisualizationLocation.Adapter;
using Mag.VisualizationLocation.Adapter.Contract;
using Mag.VisualizationLocation.Adapter.DataContext;
using Serilog;
using Moq;
using Xunit;

namespace VisualizationLocationAdapterTests
{
    public class ScenarioTest
    {
        private readonly Scenario _target;
        private readonly Mock<IMtbContext> _mockMtb;
        private readonly Mock<IDecoder> _mockDecoder;
        private readonly Mock<IJsonSerealizer> _mockJsonSerealizer;
        private readonly Mock<ILogger> _logger = new Mock<ILogger>();
        private readonly Mock<IRegistrationInfoResponseConverter> _mockRegistrationInfoResponse;

        public ScenarioTest()
        {
            _mockMtb = new Mock<IMtbContext>();
            _mockDecoder = new Mock<IDecoder>();
            _mockJsonSerealizer = new Mock<IJsonSerealizer>();
            _mockRegistrationInfoResponse = new Mock<IRegistrationInfoResponseConverter>();
            _target = new Scenario(_mockDecoder.Object,
                _mockMtb.Object, 
                _mockJsonSerealizer.Object,
                _mockRegistrationInfoResponse.Object, _logger.Object);
        }
        [Fact]
        public void ShouldReturnResponce()
        {
            //Given
            var jsonString = "{\"filter\":\"value\"}";

            var request = new RegistrationInfoRequest
            {
                QueryParamsHashCode = "anyString"
            };

            var filter = new Filter
            {
                From = DateTime.Parse("2021-01-12 12:00:55.033"),
                To = DateTime.Parse("2021-01-12 12:00:55.033"),
                IssValue = "333-22-44",
                IssType = Mag.VisualizationLocation.Adapter.IssType.Phone,
                OtmCipher = "ПТП-2020",
                OtmGuid = Guid.Parse("93f12ddf-ba74-46ea-b739-319622d331d5"),
                Password = "123",
                User = "mag"
            };
            var places = new Place[9];
            var expect = new RegistrationInfoResponse();

            _mockDecoder
                .Setup(x => x.EncryptDecrypt(request.QueryParamsHashCode))
                .Returns(jsonString);

            _mockJsonSerealizer
                .Setup(x => x.Serealaze<Filter>(jsonString))
                .Returns(filter);

            _mockMtb.Setup(x => x.GetPlaces(filter)).Returns(places);
            _mockRegistrationInfoResponse.Setup(x => x.Convert(filter, places)).Returns(expect);

            var actual = _target.GetRegistrationInfo(request);
            actual.Should().BeEquivalentTo(expect);
            
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ἷὮἊἾἣἡὮὶὮ\u1f7eὼ\u1f7eώὡὼώὡώ\u1f7eἘώ\u1f7eὶὼὼὶόόὢὼ\u1f7f\u1f7fὮὠὮἘἣὮὶὮ\u1f7eὼ\u1f7eώὡὼώὡώὺἘώ\u1f7eὶ")]
        public void SholdThrowNotFoundParmsExceptionWhenIncorretQueryParamsHashCode(string queryParamsHashCode)
        {
            var target = new Scenario(new Decoder(), _mockMtb.Object, new JsonSerealizer(), _mockRegistrationInfoResponse.Object, _logger.Object);
            Action action= ()=> target.GetRegistrationInfo(new RegistrationInfoRequest
            {
                QueryParamsHashCode = queryParamsHashCode
            });
            action.Should().Throw<NotFoundParmsException>();
        }

        public static IEnumerable<object[]> RequestResponceData =  new object[][]
        {
            new object[]
            {
                new RegistrationInfoRequest
                {
                    QueryParamsHashCode = "ἷὮἊἾἣἡὮὶὮ\u1f7eὼ\u1f7eώὡὼώὡώ\u1f7eἘώ\u1f7eὶὼὼὶόόὢὼ\u1f7f\u1f7fὮὠὮἘἣὮὶὮ\u1f7eὼ\u1f7eώὡὼώὡώὺἘώ\u1f7eὶὼὼὶόόὢὼ\u1f7f\u1f7fὮὠὮἃἸἡἋἹἥἨὮὶἢἹἠἠὠὮἅἿἿἚἭἠἹἩὮὶὮύήώ\u1f7fήώ\u1f7fὼύὼύὮὠὮἅἿἿἘἵἼἩὮὶὼὠὮἙἿἩἾὮὶἢἹἠἠὠὮἜἭἿἿἻἣἾἨὮὶἢἹἠἠὠὮἃἸἡἏἥἼἤἩἾὮὶὮ᭓᭮᭓ὡώὼὼὡώὡ\u1f7eὼ\u1f7eώὮἱ"
                },
                new RegistrationInfoResponse
                {
                    QueryParams = new QueryParamsDto
                    {
                        ControlObject = new ControlObjectDto
                        {
                            IssType = Mag.VisualizationLocation.Adapter.Contract.IssType.Phone,
                            IssValue = "79139130707"
                        },
                        OtmCipher = "ПТП-100-1-2021",
                        Period = new PeriodDto
                        {
                            Begin = DateTimeOffset.Parse("2021-01-12 12:00:55.033"),
                            End =  DateTimeOffset.Parse("2021-01-16 12:00:55.033")
                        }
                    }
                }
            }
        };
    }
}
