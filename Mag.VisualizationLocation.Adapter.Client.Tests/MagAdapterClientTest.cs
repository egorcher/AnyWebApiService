using System;
using System.Linq;
using FluentAssertions;
using Mag.VisualizationLocation.Adapter.Client.Tests.Builders;
using Mag.VisualizationLocation.Adapter.Contract;
using Newtonsoft.Json;
using Xunit;

namespace Mag.VisualizationLocation.Adapter.Client.Tests
{
    public class MagAdapterClientTest: ServiceBaseTest<Startup>
    {
        private readonly ITaskGrantBuilder _tgBuilder;
        private readonly IRegistrationInfoResponseConverter _converter;
        private readonly IDecoder _decoder;
        private readonly IMagAdapterClient _magAdapterClient;
        private const string _filterFrom = "2019-02-12 00:00:00.000";
        private const string _filterTo = "2019-02-26 00:00:00.000";
        public MagAdapterClientTest()
        {
            _tgBuilder = Resolve<ITaskGrantBuilder>();
            _converter = Resolve<IRegistrationInfoResponseConverter>();
            _decoder = Resolve<IDecoder>();
            _magAdapterClient = Resolve<IMagAdapterClient>();
        }

        [Theory]
        [MemberData(nameof(TestData))]
        public async void ShouldGetRegistrationByFilter(
            string testName, string fromAsString, string toAsString, string[] inDateAStrings, DateTime[] expectInDates
            )
        { 
            //given
            var indates = inDateAStrings.Select(DateTime.Parse).ToArray();

            var taskGrant = _tgBuilder
                .Create(DateTime.Parse(fromAsString), DateTime.Parse(toAsString))
                .WithAutoTaskAndTask()
                .WithPlaces(indates)
                .Build();

            var filter = new Filter
            {
                From = DateTime.Parse(fromAsString),
                To = DateTime.Parse(toAsString),
                OtmGuid = taskGrant.Otm.TaskGuid,
                OtmCipher = "ПТП-101-2000-100",
                IssType = IssType.Phone,
                IssValue = "334-22-33",
                User = "admin",
                Password = "mag"
            };

            var expect = _converter.Convert(filter, taskGrant.AutoTask.places.Where(x=> expectInDates.Contains(x.InDate)).Select(x=>new Place
            {
                Cid = x.Cid,
                InDate = x.InDate,
                Lac = x.Lac,
                MCC = x.MCC,
                MNC = x.MNC
            }).ToArray());
            
            
            var request = new RegistrationInfoRequest
            {
                QueryParamsHashCode = _decoder.EncryptDecrypt(JsonConvert.SerializeObject(filter))
            };

            //when
            var actual = await _magAdapterClient.GetAllRegistrationsByParamsAsync(request);

            //then
            actual.Should().BeEquivalentTo(expect);
        }

        public static object[][] TestData => new[]
        {
            new object[]
            {
                "Test1",
                _filterFrom,
                _filterTo,
                new[]
                {
                    "2019-02-11 00:00:00.000",
                    _filterFrom,
                    "2019-02-14 00:00:00.000",
                    _filterTo,
                    "2019-02-27 00:00:00.000"
                },
                new[]
                {
                    DateTime.Parse(_filterFrom),
                    DateTime.Parse("2019-02-14 00:00:00.000"),
                    DateTime.Parse(_filterTo)
                }
            },
            new object[]
            {
                "Test2",
                _filterFrom,
                _filterTo,
                new[]
                {
                    "2019-02-10 00:00:00.000",
                    "2019-02-11 00:00:00.000",
                },
                new DateTime[0]
            },
            new object[]
            {
                "Test3",
                _filterFrom,
                _filterTo,
                new[]
                {
                    "2019-02-11 00:00:00.000",
                    "2019-02-28 00:00:00.000"
                },
                new DateTime[0]
            }
            ,
            new object[]
            {
                "Test4",
                _filterFrom,
                _filterTo,
                new[]
                {
                    _filterFrom,
                    _filterTo
                },
                new[]
                {
                    DateTime.Parse(_filterFrom),
                    DateTime.Parse(_filterTo)
                }
            }
        };

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("ἷὮἊἾἣἡὮὶὮ\u1f7eὼ\u1f7eώὡὼώὡώ\u1f7eἘώ\u1f7eὶὼὼὶόόὢὼ\u1f7f\u1f7fὮὠὮἘἣὮὶὮ\u1f7eὼ\u1f7eώὡὼώὡώὺἘώ\u1f7eὶ")]
        public async void SholdThrowNotFoundParmsExceptionWhenIncorretQueryParamsHashCode(string queryParamsHashCode)
        {

            var request = new RegistrationInfoRequest
            {
                QueryParamsHashCode = queryParamsHashCode
            };

            //when
            Action act = () =>
            {
                var task = _magAdapterClient.GetAllRegistrationsByParamsAsync(request);
                _ = task.Result;
            };

            //then
            act.Should().Throw<NotFoundParmsException>();
        }
    }
}
