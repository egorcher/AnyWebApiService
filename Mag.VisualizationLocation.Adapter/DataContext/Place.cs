using System;

namespace Mag.VisualizationLocation.Adapter
{
    public class Place
    {
        public long Cid { get; set; }
        public long Lac { get; set; }
        public DateTime InDate { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string MNC { get; set; }
        public string MCC { get; set; }
    }
}
