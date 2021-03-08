using System;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.TestContext
{
    public class TaskGrantDto
    {
        public int Id { get; set; }
        public int OtmId { get; set; }
        public int AutoTaskId { get; set; }
        public DateTime AutoStart { get; set; }
        public DateTime AutoEnd { get; set; }

        public AutoTaskDto AutoTask { get; set; }
        public OtmTaskDto Otm { get; set; }
    }
}