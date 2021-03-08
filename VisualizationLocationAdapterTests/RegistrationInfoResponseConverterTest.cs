using System;
using System.Collections.Generic;
using FluentAssertions;
using Mag.VisualizationLocation.Adapter;
using Mag.VisualizationLocation.Adapter.Contract;
using Xunit;

namespace VisualizationLocationAdapterTests
{
    public class RegistrationInfoResponseConverterTest
    {
        private const string FromDate = "2021-01-12 12:00:55.033";
        private const string ToDate = "2021-01-18 12:00:55.033";
        private const string RegDate1 = "2021-01-19 12:00:55.033";
        private const long Cid1 = 10292092;
        private const long Lac1 = 10909092;
        private const double Latitude1=10.2;
        private const double Longitude1 = 55.10;
        private const double Latitude2 = 10.2;
        private const double Longitude2 = 55.10;
        private const string RegDate2 = "2021-01-15 12:00:55.033";
        private const long Cid2 = 1022092;
        private const long Lac2 = 1090900;
        private const string MCC1 = "250";
        private const string MNC1 = "1234";
        private const string MCC2 = "250";
        private const string MNC2 = "114";
        private const string Cipher = "ПТП-100-1-2021";
        private const string IssValue = "79139130707";
        private const int IssType = 0;

        [Theory]
        [MemberData(nameof(TestData))]
        public void ShouldConvert(string name,Filter filter, Place[] places, RegistrationInfoResponse expect )
        {
            var converter = new RegistrationInfoResponseConverter();

            var actual = converter.Convert(filter, places);

            actual.Should().BeEquivalentTo(expect);
        }

        public static IEnumerable<object[]> TestData = new object[][]
        {
            new object[]
            {
                "Test01",
                new Filter
                {
                    From = DateTime.Parse(FromDate),
                    To = DateTime.Parse(ToDate),
                    OtmCipher = Cipher,
                    OtmGuid = Guid.NewGuid(),
                    IssValue = IssValue,
                    IssType = IssType
                },
                new Place[0],
                new RegistrationInfoResponse
                {
                    QueryParams = new QueryParamsDto
                    {
                        ControlObject = new ControlObjectDto
                        {
                            IssType = IssType,
                            IssValue = IssValue
                        },
                        OtmCipher = Cipher,
                        Period = new PeriodDto
                        {
                            Begin = DateTimeOffset.Parse(FromDate),
                            End =  DateTimeOffset.Parse(ToDate)
                        }
                    },
                    RegistrationInfos = new RegistrationInfoDto[0]
                }
            },
            new object[]
            {
                "Test02",
                new Filter
                {
                    From = DateTime.Parse(FromDate),
                    To = DateTime.Parse(ToDate),
                    OtmCipher = Cipher
                },
                new Place[0],
                new RegistrationInfoResponse
                {
                    QueryParams = new QueryParamsDto
                    {
                        OtmCipher = Cipher,
                        Period = new PeriodDto
                        {
                            Begin = DateTimeOffset.Parse(FromDate),
                            End =  DateTimeOffset.Parse(ToDate)
                        }
                    },
                    RegistrationInfos = new RegistrationInfoDto[0]
                }
            },
            new object[]
            {
                "Test03",
                new Filter
                {
                    From = DateTime.Parse(FromDate),
                    To = DateTime.Parse(ToDate),
                    OtmCipher = Cipher,
                    OtmGuid = Guid.NewGuid(),
                    IssValue = IssValue,
                    IssType = IssType
                },
                new []
                {
                    new Place
                    {
                        Cid = Cid1,
                        Lac = Lac1,
                        InDate = DateTime.Parse(RegDate1)
                    },
                    new Place
                    {
                        Cid = Cid2,
                        Lac = Lac2,
                        InDate = DateTime.Parse(RegDate2)
                    },
                },
                new RegistrationInfoResponse
                {
                    QueryParams = new QueryParamsDto
                    {
                        ControlObject = new ControlObjectDto
                        {
                            IssType = IssType,
                            IssValue = IssValue
                        },
                        OtmCipher = Cipher,
                        Period = new PeriodDto
                        {
                            Begin = DateTimeOffset.Parse(FromDate),
                            End =  DateTimeOffset.Parse(ToDate)
                        }
                    },
                    RegistrationInfos = new []
                    {
                        new RegistrationInfoDto
                        {
                            RegistrationDate = DateTimeOffset.Parse(RegDate1),
                            BaseStation = new BaseStationInfoDto
                            {
                                Identifier = new BaseStationIdentifierDto
                                {
                                    Cid = Cid1,
                                    Lac = Lac1
                                },
                                OperatorCode = new OperatorCodeDto()
                            }
                        },
                        new RegistrationInfoDto
                        {
                            RegistrationDate = DateTimeOffset.Parse(RegDate2),
                            BaseStation = new BaseStationInfoDto
                            {
                                Identifier = new BaseStationIdentifierDto
                                {
                                    Cid = Cid2,
                                    Lac = Lac2
                                },
                                OperatorCode = new OperatorCodeDto()
                            },
                        },
                    }
                }
            },
            new object[]
            {
                "Test04",
                new Filter
                {
                    From = DateTime.Parse(FromDate),
                    To = DateTime.Parse(ToDate),
                    OtmCipher = Cipher,
                    OtmGuid = Guid.NewGuid(),
                    IssValue = IssValue,
                    IssType = IssType
                },
                new []
                {
                    new Place
                    {
                        Cid = Cid1,
                        Lac = Lac1,
                        InDate = DateTime.Parse(RegDate1),
                        Latitude = Latitude1,
                        Longitude = Longitude1
                    },
                    new Place
                    {
                        Cid = Cid2,
                        Lac = Lac2,
                        InDate = DateTime.Parse(RegDate2),
                        Latitude = Latitude2,
                        Longitude = Longitude2
                    },
                },
                new RegistrationInfoResponse
                {
                    QueryParams = new QueryParamsDto
                    {
                        ControlObject = new ControlObjectDto
                        {
                            IssType = IssType,
                            IssValue = IssValue
                        },
                        OtmCipher = Cipher,
                        Period = new PeriodDto
                        {
                            Begin = DateTimeOffset.Parse(FromDate),
                            End =  DateTimeOffset.Parse(ToDate)
                        }
                    },
                    RegistrationInfos = new []
                    {
                        new RegistrationInfoDto
                        {
                            RegistrationDate = DateTimeOffset.Parse(RegDate1),
                            BaseStation = new BaseStationInfoDto
                            {
                                Identifier = new BaseStationIdentifierDto
                                {
                                    Cid = Cid1,
                                    Lac = Lac1
                                },
                                Latitude = Latitude1,
                                Longitude = Longitude1,
                                OperatorCode = new OperatorCodeDto()
                            }
                        },
                        new RegistrationInfoDto
                        {
                            RegistrationDate = DateTimeOffset.Parse(RegDate2),
                            BaseStation = new BaseStationInfoDto
                            {
                                Identifier = new BaseStationIdentifierDto
                                {
                                    Cid = Cid2,
                                    Lac = Lac2
                                },
                                Latitude = Latitude2,
                                Longitude = Longitude2,
                                OperatorCode = new OperatorCodeDto()
                            }
                        },
                    }
                }
            },
            new object[]
            {
                "Test05",
                new Filter
                {
                    From = DateTime.Parse(FromDate),
                    To = DateTime.Parse(ToDate),
                    OtmCipher = Cipher,
                    OtmGuid = Guid.NewGuid(),
                    IssValue = IssValue,
                    IssType = IssType
                },
                new []
                {
                    new Place
                    {
                        Cid = Cid1,
                        Lac = Lac1,
                        InDate = DateTime.Parse(RegDate1),
                        MCC = MCC1,
                        MNC = MNC1
                    },
                    new Place
                    {
                        Cid = Cid2,
                        Lac = Lac2,
                        InDate = DateTime.Parse(RegDate2),
                        MCC = MCC2,
                        MNC = MNC2
                    },
                },
                new RegistrationInfoResponse
                {
                    QueryParams = new QueryParamsDto
                    {
                        ControlObject = new ControlObjectDto
                        {
                            IssType = IssType,
                            IssValue = IssValue
                        },
                        OtmCipher = Cipher,
                        Period = new PeriodDto
                        {
                            Begin = DateTimeOffset.Parse(FromDate),
                            End =  DateTimeOffset.Parse(ToDate)
                        }
                    },
                    RegistrationInfos = new []
                    {
                        new RegistrationInfoDto
                        {
                            RegistrationDate = DateTimeOffset.Parse(RegDate1),
                            BaseStation = new BaseStationInfoDto
                            {
                                Identifier = new BaseStationIdentifierDto
                                {
                                    Cid = Cid1,
                                    Lac = Lac1
                                },
                                OperatorCode = new OperatorCodeDto
                                {
                                    MCC = MCC1,
                                    MNC = MNC1
                                }
                            }
                        },
                        new RegistrationInfoDto
                        {
                            RegistrationDate = DateTimeOffset.Parse(RegDate2),
                            BaseStation = new BaseStationInfoDto
                            {
                                Identifier = new BaseStationIdentifierDto
                                {
                                    Cid = Cid2,
                                    Lac = Lac2
                                },
                                OperatorCode = new OperatorCodeDto
                                {
                                    MCC = MCC2,
                                    MNC = MNC2
                                }
                            }
                        },
                    }
                }
            },
        };
    }
}
