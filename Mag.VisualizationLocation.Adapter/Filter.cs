using System;

namespace Mag.VisualizationLocation.Adapter
{
    public class Filter
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public Guid OtmGuid { get; set; }
        public string IssValue { get; set; }
        public IssType IssType { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string OtmCipher { get; set; }

        public bool IsCorrectFilter()
        {
            return
                From != default
                && To != default
                && OtmGuid != default
                && !string.IsNullOrWhiteSpace(User)
                && !string.IsNullOrWhiteSpace(Password)
                && !string.IsNullOrWhiteSpace(OtmCipher);
        }
    }
}
