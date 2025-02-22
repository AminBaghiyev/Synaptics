using Newtonsoft.Json;
using Synaptics.Application.Common.Results;
using Synaptics.Application.Interfaces.Services;
using System.Text;

namespace Synaptics.Infrastructure.Services;

public class PyBridgeService : IPyBridgeService
{
    readonly HttpClient _client;

    public PyBridgeService(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
    }

    public async Task<(PyBridgeResult, float[])> EmbeddingAsync(string sentence)
    {
        try
        {
            string url = "http://127.0.0.1:8001/api/embed";
            StringContent content = new($"{{\"sentence\": \"{sentence}\"}}", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(url, content);

            if (!response.IsSuccessStatusCode)
                return (PyBridgeResult.Failure(response.ReasonPhrase), []);

            string result = await response.Content.ReadAsStringAsync();
            float[]? embedding = JsonConvert.DeserializeObject<float[]>(result);

            if (embedding is null)
                return (PyBridgeResult.Failure("Invalid response format"), []);

            return (PyBridgeResult.Success(), embedding);
        }
        catch (Exception ex)
        {
            return (PyBridgeResult.Failure(ex.Message), []);
        }
    }
}