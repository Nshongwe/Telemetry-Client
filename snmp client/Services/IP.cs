using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Pocos;

namespace snmp_client.Services
{
    public interface IIP
    {
        void ReadIPAddress();
        IpModel IpModel { get; set; }
    }

    public class IP : IIP
    {
        public IpModel IpModel { get; set; }

        public void ReadIPAddress()
        {
            HttpClient cons = new HttpClient {BaseAddress = new Uri("https://api.ipify.org?format=json")};
            cons.DefaultRequestHeaders.Accept.Clear();
            cons.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            GetIpapi(cons).Wait();
        }

        private async Task GetIpapi(HttpClient cons)
        {
            using (cons)
            {
                HttpResponseMessage res = await cons.GetAsync("https://api.ipify.org?format=json");
                res.EnsureSuccessStatusCode();
                if (res.IsSuccessStatusCode)
                {
                    IpModel = await res.Content.ReadAsAsync<IpModel>();
                }
            }
        }
    }

}
