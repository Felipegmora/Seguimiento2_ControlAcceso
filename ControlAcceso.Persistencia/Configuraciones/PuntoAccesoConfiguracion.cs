using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlAcceso.Persistencia.Configuraciones
{
    public sealed class PuntoAccesoConfiguracion : IEntityTypeConfiguration<PuntoAcceso>
    {
        public void Configure(EntityTypeBuilder<PuntoAcceso> constructor)
        {
            constructor.ToTable("PuntosAcceso");

            constructor.HasKey(puntoAcceso => puntoAcceso.Id);
            constructor.Property(puntoAcceso => puntoAcceso.Id).ValueGeneratedNever();

            constructor.Property(puntoAcceso => puntoAcceso.Nombre)
                       .HasMaxLength(PuntoAcceso.LongitudMaximaNombre)
                       .IsRequired();

            constructor.HasIndex(puntoAcceso => puntoAcceso.Nombre).IsUnique();

            constructor.Property(puntoAcceso => puntoAcceso.Tipo)
                       .HasConversion<int>()
                       .IsRequired();

            constructor.Property(puntoAcceso => puntoAcceso.Habilitado)
                       .IsRequired();

            constructor.Property(puntoAcceso => puntoAcceso.Descripcion)
                       .HasMaxLength(PuntoAcceso.LongitudMaximaDescripcion);
        }
    }
}
