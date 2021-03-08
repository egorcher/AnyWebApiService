using System;
using System.Collections.Generic;
using FluentAssertions;
using Mag.VisualizationLocation.Adapter;
using Mag.VisualizationLocation.Adapter.Contract;
using Newtonsoft.Json;
using Xunit;
using IssType = Mag.VisualizationLocation.Adapter.Contract.IssType;

namespace VisualizationLocationAdapterTests
{
    
    public class DecoderTest
    {
        [Theory]
        [InlineData("{\"login\":\"Admin\",\"psw\":\"mag\",\"taskGuid\":\"\00000000-0000-0000-0000-000000000001\",\"begin\":\"2021-02-18 16:50:22.517\",\"end\":\"2021-02-18 16:50:22.517\"}")]
        public void ShouldDecodeString(string value)
        {
            var decoder = new Decoder();
            var encrypt = decoder.EncryptDecrypt(value);
            
            value.Should().NotBe(encrypt);

            var decrypt = decoder.EncryptDecrypt(encrypt);

            value.Should().Be(decrypt);
        }

        [Theory]
        [MemberData(nameof(RequestResponceData))]
        public void ShouldResponceDecrypt(RegistrationInfoRequest request, RegistrationInfoResponse response)
        {
            var decoder = new Decoder();
            var encrypt = decoder.EncryptDecrypt(request.QueryParamsHashCode);
        }

        public static IEnumerable<object[]> RequestResponceData = new object[][]
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
                            IssType = IssType.Phone,
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

        [Fact]
        public void TempTest()
        {
            var filter = new Filter
            {
                From = DateTime.Parse("2021-01-12 12:00:55.033"),
                To = DateTime.Parse("2021-01-16 12:00:55.033"),
                IssType = Mag.VisualizationLocation.Adapter.IssType.Phone,
                IssValue = "79139130707",
                OtmCipher = "ПТП-100-1-2021",
            };
            var decoder = new Decoder();
            var encrypt = decoder.EncryptDecrypt(JsonConvert.SerializeObject(filter));

        }
    }
}
