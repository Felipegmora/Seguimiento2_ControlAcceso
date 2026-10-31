using ControlAcceso.Dominio.Entidades.Areas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlAcceso.Persistencia.Configuraciones
{
    public sealed class AreaConfiguracion : IEntityTypeConfiguration<Area>
    {
        public void Configure(EntityTypeBuilder<Area> constructor)
        {
            constructor.ToTable("Areas");

            constructor.HasKey(area => area.Id);
            constructor.Property(area => area.Id).ValueGeneratedNever();

            constructor.Property(area => area.Nombre)
                       .HasMaxLength(Area.LongitudMaximaNombre)
                       .IsRequired();

            constructor.HasIndex(area => area.Nombre).IsUnique();

            constructor.Property(area => area.Descripcion)
                       .HasMaxLength(Area.LongitudMaximaDescripcion);
        }
    }
}
