namespace Mag.VisualizationLocation.Adapter.Contract
{
    public class OperatorCodeDto
    {
        /// <summary>
        ///     идентификатор страны (Всегда 250)
        /// </summary>
        public string MCC { get; set; }

        /// <summary>
        ///     Идентификатор оператора (От 1 до 100)
        /// </summary>
        public string MNC { get; set; }
    }

}
