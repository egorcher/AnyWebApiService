using System;
using System.Collections.Generic;
using System.Linq;
using Mag.VisualizationLocation.Adapter.Client.Tests.TestContext;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.Builders
{
    public interface IAutoTaskBuilder
    {
        AutoTaskBuilder Create();
        AutoTaskBuilder WithPlace(params DateTime[] inDates);
        AutoTaskDto Build();
    }
    public class AutoTaskBuilder : IAutoTaskBuilder
    {
        private ITestMtbContext _testMtbContext;
        private AutoTaskDto _autoTask;
        private List<PlaceDto> _places = new List<PlaceDto>();

        public AutoTaskBuilder(ITestMtbContext testMtbContext)
        {
            _testMtbContext = testMtbContext;
        }

        public AutoTaskBuilder Create()
        {
            _autoTask = _testMtbContext.AddModel();
            return this;
        }

        public AutoTaskBuilder WithPlace(params DateTime[] inDates)
        {
            var rendomize = new Random(DateTime.Now.Millisecond);
            var models = inDates.Select(x => new PlaceDto
            {
                InDate = x, 
                TaskAutoId = _autoTask.Id,
                MNC = $"{rendomize.Next()}",
                MCC = $"{rendomize.Next()}",
                Cid = rendomize.Next(),
                Lac = rendomize.Next()
            }).ToArray();
            _places.AddRange(_testMtbContext.AddModels(models));
            return this;
        }

        public AutoTaskDto Build()
        {
            _autoTask.places = _places;
            return _autoTask;
        }
    }
}
