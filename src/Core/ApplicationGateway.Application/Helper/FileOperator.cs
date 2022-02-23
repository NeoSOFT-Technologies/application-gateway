namespace ApplicationGateway.Application.Helper
{
    public class FileOperator
    {
        public FileOperator()
        {

        }

        public async Task<string> ReadPolicies(string policiesFolderPath)
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

        public async Task WritePolicies(string policiesFolderPath, string content)
        {
            await File.WriteAllTextAsync($@"{policiesFolderPath}\policies.json", content);
        }
    }
}
