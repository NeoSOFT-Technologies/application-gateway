namespace ApplicationGateway.Application.Helper
{
	public class RestClient<TIdentifier> : IDisposable
	{
		private HttpClient httpClient;
		protected readonly string _baseAddress;
		private readonly string _addressSuffix;
		private bool disposed = false;

		public RestClient(string baseAddress, string addressSuffix, Dictionary<string, string>? headers)
		{
			_baseAddress = baseAddress;
			_addressSuffix = addressSuffix;
			httpClient = CreateHttpClient(_baseAddress, headers);
		}
		protected virtual HttpClient CreateHttpClient(string serviceBaseAddress, Dictionary<string, string>? headers)
		{
			httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri(serviceBaseAddress);
            foreach (KeyValuePair<string, string> header in headers)
            {
				httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
			return httpClient;
		}
		public async Task<string> GetAsync(TIdentifier? identifier)
		{
			string address = identifier is not null ? _addressSuffix + identifier.ToString() : _addressSuffix;
			HttpResponseMessage responseMessage = await httpClient.GetAsync(address);
			responseMessage.EnsureSuccessStatusCode();
			return await responseMessage.Content.ReadAsStringAsync();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed && disposing)
			{
				if (httpClient != null)
				{
					httpClient.Dispose();
				}
				disposed = true;
			}
		}
	}
}
