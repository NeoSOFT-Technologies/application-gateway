using JUST;

namespace ApplicationGateway.Application.Helper
{
    public class FileOperator
    {
        private readonly string _basePath;

        public FileOperator()
        {
            _basePath = Directory.GetCurrentDirectory();
        }

        public async Task<string> Transform(string requestJson, string fileName)
        {
            string transformer = await File.ReadAllTextAsync($@"{_basePath}\JsonTransformers\Tyk\{fileName}.json");
            return new JsonTransformer().Transform(transformer, requestJson);
        }

        public static async Task<string> ReadPolicies(string policiesFolderPath)
        {
            if (!Directory.Exists(policiesFolderPath))
            {
                Directory.CreateDirectory(policiesFolderPath);
            }
            if (!File.Exists($@"{policiesFolderPath}\policies.json"))
            {
                StreamWriter sw = File.CreateText($@"{policiesFolderPath}\policies.json");
                await sw.WriteLineAsync("{}");
                sw.Dispose();
            }
            return await File.ReadAllTextAsync($@"{policiesFolderPath}\policies.json");
        }

        public static async Task WritePolicies(string policiesFolderPath, string content)
        {
            await File.WriteAllTextAsync($@"{policiesFolderPath}\policies.json", content);
        }
    }
}
