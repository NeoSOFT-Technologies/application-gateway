namespace ApplicationGateway.API.IntegrationTests.Helper
{
    public static class ApplicationConstants
    {
        public const string TYK_BASE_URL = "http://localhost:3004/";
        public const string BASE_PATH = "../../../JsonData";
        public const string ORIGIN_IP_URL = "http://httpbin.org/get";
        public const string TARGET_URL = "http://host.docker.internal:5000";
    }
}
