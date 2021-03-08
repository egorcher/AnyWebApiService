namespace Mag.VisualizationLocation.Adapter
{
    public class Subject
    {
        public long Id { get; set; }

        public string Address { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string OperatorName { get; set; }

        public string Source { get; set; }

        public bool LatLngUpdated { get; set; }
    }
}
