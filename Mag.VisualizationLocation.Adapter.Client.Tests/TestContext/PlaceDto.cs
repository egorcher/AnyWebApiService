using System;

namespace Mag.VisualizationLocation.Adapter.Client.Tests.TestContext
{
    public class PlaceDto
    {
        public int Id { get; set; }
        public DateTime InDate { get; internal set; }
        public long Cid { get; internal set; }
        public long Lac { get; internal set; }
        public string MCC { get; internal set; }
        public string MNC { get; internal set; }
        public int TaskAutoId { get; set; }
    }
}