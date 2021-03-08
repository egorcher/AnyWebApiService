using System;
using System.Collections.Generic;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.TestContext
{
    public class AutoTaskDto
    {
        public int Id { get; internal set; }
        public List<PlaceDto> places { get; set; }
        public string RegNumber { get; set; }
        public Guid TaskAutoGuid { get; internal set; }
    }
}