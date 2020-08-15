using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Client
{
    public static class Utility
    {
        public static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        };

        public static ClientOptions ClientOptions { get; set; }

        public static async Task<TResult> GetResultAsync<TResult>(this HttpClient httpClient, string uri) where TResult : class
        {
            var response = await httpClient.GetAsync(uri);
            var json = await response.Content?.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                return string.IsNullOrWhiteSpace(json) ? null : JsonSerializer.Deserialize<TResult>(json, _serializerOptions);
            }
            else { return null; }
        }
    }
}
