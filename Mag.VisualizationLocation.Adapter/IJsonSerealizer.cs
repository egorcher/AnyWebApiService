using Newtonsoft.Json;

namespace Mag.VisualizationLocation.Adapter
{
    public interface IJsonSerealizer
    {
        T Serealaze<T>(string value);
    }

    public class JsonSerealizer : IJsonSerealizer
    {
        public T Serealaze<T>(string value)
        {
            return JsonConvert.DeserializeObject<T>(value);
        }

    }
}
