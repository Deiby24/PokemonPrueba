using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PokedexAPI.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace PokedexAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly PokedexContext _context;

        public PokemonController(PokedexContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PokemonDto>> GetPokemon(int id)
        {
            var pokemon = await _context.Pokemon
                .Include(p => p.PokemonAtaque)
                    .ThenInclude(pa => pa.Ataque)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pokemon == null)
            {
                return NotFound();
            }

            var pokemonDto = new PokemonDto
            {
                Id = pokemon.Id,
                Nombre = pokemon.Nombre,
                Tipo = pokemon.Tipo,
                Altura = pokemon.Altura,
                Peso = pokemon.Peso,
                Ataques = pokemon.PokemonAtaque.Select(pa => new AtaqueDto
                {
                    Id = pa.Ataque.Id,
                    Nombre = pa.Ataque.Nombre,
                    Poder = pa.Ataque.Poder,
                    Precision = pa.Ataque.Precisio,
                    Tipo = pa.Ataque.Tipo
                }).ToList()
            };
            Console.WriteLine("prueba");
            return Ok(pokemonDto);
        }
    }

    public class PokemonDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public List<AtaqueDto> Ataques { get; set; }
    }

    public class AtaqueDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Poder { get; set; }
        public decimal Precision { get; set; }
        public string Tipo { get; set; }
    }
}
