using Microsoft.EntityFrameworkCore;

namespace PokedexAPI.Data
{
    public class PokedexContext : DbContext
    {
        public PokedexContext(DbContextOptions<PokedexContext> options) : base(options) { }

        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<Ataque> Ataque { get; set; }

        // Aquí ajustamos el nombre de la tabla a 'PokemonAtaque' en singular
        public DbSet<PokemonAtaque> PokemonAtaque { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PokemonAtaque>()
                .HasKey(pa => new { pa.PokemonId, pa.AtaqueId });

            modelBuilder.Entity<PokemonAtaque>()
                .HasOne(pa => pa.Pokemon)
                .WithMany(p => p.PokemonAtaque)
                .HasForeignKey(pa => pa.PokemonId);

            modelBuilder.Entity<PokemonAtaque>()
                .HasOne(pa => pa.Ataque)
                .WithMany(a => a.PokemonAtaque)
                .HasForeignKey(pa => pa.AtaqueId);
        }
    }

    public class Pokemon
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public decimal Altura { get; set; }
        public decimal Peso { get; set; }
        public ICollection<PokemonAtaque> PokemonAtaque { get; set; }
    }

    public class Ataque
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Poder { get; set; }
        public decimal Precisio { get; set; }
        public string Tipo { get; set; }
        public ICollection<PokemonAtaque> PokemonAtaque { get; set; }
    }

    public class PokemonAtaque
    {
        public int PokemonId { get; set; }
        public Pokemon Pokemon { get; set; }
        public int AtaqueId { get; set; }
        public Ataque Ataque { get; set; }
    }
}
