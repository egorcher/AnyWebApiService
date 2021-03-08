using System.Linq;

namespace Mag.VisualizationLocation.Adapter
{
    public interface IDecoder
    {
        string EncryptDecrypt(string str);
    }

    public class Decoder : IDecoder
    {
        private const ushort Key = 8012;
        public string EncryptDecrypt(string str) => string.Join("", str.Select(ch => (char) (ch ^ Key)));
    }
}
