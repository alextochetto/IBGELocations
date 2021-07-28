using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace LocationWebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    public class CityController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IbgeLocationOptions _ibgeLocationOptions;

        public CityController(IHttpClientFactory httpClientFactory, IOptionsMonitor<IbgeLocationOptions> ibgeLocationOptions)
        {
            this.httpClientFactory = httpClientFactory;
            _ibgeLocationOptions = ibgeLocationOptions.CurrentValue;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<City>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
        public async Task<ActionResult> GetCity(int? ufCode)
        {
            if (ufCode is null)
                return BadRequest(ufCode);

            string requestUri = string.Format(_ibgeLocationOptions.RequestUriCity, ufCode);
            HttpClient httpClient = httpClientFactory.CreateClient(IbgeLocationOptions.Instance);
            httpClient.BaseAddress = new Uri(_ibgeLocationOptions.BaseAddress);
            HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(requestUri);
            string jsonContent = await httpResponseMessage.Content.ReadAsStringAsync();
            List<City> cities = JsonConvert.DeserializeObject<List<City>>(jsonContent);

            return Ok(cities);
        }
    }

    public class IbgeLocationOptions
    {
        public static string Instance { get; } = "IBGE";
        public string BaseAddress { get; set; }
        public string RequestUriCity { get; set; }
    }

    public class City
    {
        public int id { get; set; }
        public string nome { get; set; }
        public Microrregiao microrregiao { get; set; }
        public RegiaoImediata regiaoimediata { get; set; }
    }

    public class Microrregiao
    {
        public int id { get; set; }
        public string nome { get; set; }
        public Mesorregiao mesorregiao { get; set; }
    }

    public class Mesorregiao
    {
        public int id { get; set; }
        public string nome { get; set; }
        public UF UF { get; set; }
    }

    public class UF
    {
        public int id { get; set; }
        public string sigla { get; set; }
        public string nome { get; set; }
        public Regiao regiao { get; set; }
    }

    public class Regiao
    {
        public int id { get; set; }
        public string sigla { get; set; }
        public string nome { get; set; }
    }

    public class RegiaoImediata
    {
        public int id { get; set; }
        public string nome { get; set; }
        public RegiaoIntermediaria regiaointermediaria { get; set; }
    }

    public class RegiaoIntermediaria
    {
        public int id { get; set; }
        public string nome { get; set; }
        public UF1 UF { get; set; }
    }

    public class UF1
    {
        public int id { get; set; }
        public string sigla { get; set; }
        public string nome { get; set; }
        public Regiao1 regiao { get; set; }
    }

    public class Regiao1
    {
        public int id { get; set; }
        public string sigla { get; set; }
        public string nome { get; set; }
    }
}

