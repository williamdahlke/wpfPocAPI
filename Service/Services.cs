﻿using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using wpfPocAPI.Interceptors;

namespace wpfPocAPI.Service
{
    internal class Services
    {
        internal Services()
        {
        }

        private static Services _instance;
        internal static Services Instance 
        { 
            get
            {
                if (_instance == null)
                {
                    _instance = new Services();
                }                
                return _instance;
            }
        }

        [MetricInterceptor]
        internal void SaveProject()
        {
            MessageBox.Show("Entrou no método SaveProject da classe Services");
        }

        public async Task PostJsonAsync(string url, object data)
        {
            using (var client = new HttpClient())
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(url, content);

                if (!response.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Request failed with status code: {response.StatusCode}");
                }
            }
        }
    }
}
