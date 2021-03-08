using Mag.VisualizationLocation.Adapter.Client.Tests.TestContext;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.Builders
{
    public interface ITaskBuilder
    {
        void Create();
        OtmTaskDto Build();
    }

    public class TaskBuilder : ITaskBuilder
    {
        private ITestMtbContext _mtbContext;
        private OtmTaskDto _otm;

        public TaskBuilder(ITestMtbContext mtbContext)
        {
            _mtbContext = mtbContext;
        }

        public void Create()
        {
            var model = new OtmTaskDto();
            _otm = _mtbContext.AddModel(model);
        }

        public OtmTaskDto Build()
        {
            return _otm;
        }
    }
}