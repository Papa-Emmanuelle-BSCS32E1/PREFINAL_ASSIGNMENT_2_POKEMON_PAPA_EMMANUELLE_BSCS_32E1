using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using PREFINAL_ASSIGNMENT_TWO_POKEMON_PAPA_EMMANUELLE_BSCS_32E1.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PREFINAL_ASSIGNMENT_TWO_POKEMON_PAPA_EMMANUELLE_BSCS_32E1.Controllers
{
    public class PokemonController : Controller
    {
        private readonly HttpClient _httpClient;

        public PokemonController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            const int pageSize = 20;
            var offset = (page - 1) * pageSize;
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon?offset={offset}&limit={pageSize}");
            var data = JObject.Parse(response);
            var results = data["results"];

            var pokemons = new List<Pokemon>();

            foreach (var result in results)
            {
                pokemons.Add(new Pokemon { Name = result["name"].ToString() });
            }

            ViewBag.Page = page;
            ViewBag.HasPrevious = offset > 0;
            ViewBag.HasNext = data["next"] != null;

            return View(pokemons);
        }

        public async Task<IActionResult> Details(string name)
        {
            var response = await _httpClient.GetStringAsync($"https://pokeapi.co/api/v2/pokemon/{name}");
            var data = JObject.Parse(response);

            var pokemon = new Pokemon
            {
                Name = data["name"].ToString(),
                Moves = data["moves"].Select(m => m["move"]["name"].ToString()).ToList(),
                Abilities = data["abilities"].Select(a => a["ability"]["name"].ToString()).ToList()
            };

            return View(pokemon);
        }
    }
}
