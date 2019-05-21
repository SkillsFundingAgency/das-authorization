namespace SFA.DAS.Authorization.Features
{
    public class FeatureToggle
    {
        public string Feature { get; }
        public bool IsEnabled { get; }

        public FeatureToggle(string feature, bool isEnabled)
        {
            Feature = feature;
            IsEnabled = isEnabled;
        }
    }
}