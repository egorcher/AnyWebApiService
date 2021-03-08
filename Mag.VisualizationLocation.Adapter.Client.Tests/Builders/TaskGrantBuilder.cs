using System;
using Mag.VisualizationLocation.Adapter.Client.Tests.TestContext;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.Builders
{
    public interface ITaskGrantBuilder
    {
        TaskGrantBuilder Create(DateTime autoStart, DateTime autoEnd);
        TaskGrantBuilder WithAutoTaskAndTask();
        TaskGrantBuilder WithPlaces(params DateTime[] indates);
        TaskGrantDto Build();
    }

    public class TaskGrantBuilder : ITaskGrantBuilder
    {
        private TaskGrantDto _taskGrant;
        private ITestMtbContext _testMtbContext;
        private IAutoTaskBuilder _autoTaskBuilder;
        private ITaskBuilder _taskBuilder;

        public TaskGrantBuilder(ITestMtbContext testMtbContext, IAutoTaskBuilder autoTaskBuilder, ITaskBuilder taskBuilder)
        {
            _testMtbContext = testMtbContext ?? throw new ArgumentNullException(nameof(testMtbContext));
            _autoTaskBuilder = autoTaskBuilder;
            _taskBuilder = taskBuilder;
        }

        public TaskGrantBuilder Create(DateTime autoStart, DateTime autoEnd)
        {
            _taskGrant = new TaskGrantDto
            {
                AutoStart = autoStart,
                AutoEnd = autoEnd

            };
            return this;
        }

        public TaskGrantBuilder WithAutoTaskAndTask()
        {
            _autoTaskBuilder.Create();
            _taskBuilder.Create();
            return this;
        }

        public TaskGrantBuilder WithPlaces(params DateTime[] indates)
        {
            _autoTaskBuilder.WithPlace(indates);
            return this;
        }

        public TaskGrantDto Build()
        {
            var autoTask = _autoTaskBuilder.Build();
            var otm = _taskBuilder.Build();
            _taskGrant.OtmId = otm.Id;
            _taskGrant.AutoTaskId = autoTask.Id;
            _testMtbContext.AddModel(_taskGrant);
            _taskGrant.Otm = otm;
            _taskGrant.AutoTask = autoTask;
            return _taskGrant;
        }

    }
}
