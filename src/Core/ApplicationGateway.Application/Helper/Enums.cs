namespace ApplicationGateway.Application.Helper
{
    public static class Enums
    {
        public enum Type
        {
            API,
            Key,
            Policy
        }
        public enum Operation
        {
            Created,
            Updated,
            Deleted
        }
        public enum Gateway
        {
            Tyk,
            Envoy,
            Kong
        }
    }
}
