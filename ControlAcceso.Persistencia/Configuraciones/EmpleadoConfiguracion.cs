using ControlAcceso.Dominio.Entidades.Empleados;
using ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlAcceso.Persistencia.Configuraciones
{
    public sealed class EmpleadoConfiguracion : IEntityTypeConfiguration<Empleado>
    {
        public void Configure(EntityTypeBuilder<Empleado> constructor)
        {
            constructor.ToTable("Empleados");

            constructor.HasKey(empleado => empleado.Id);
            constructor.Property(empleado => empleado.Id).ValueGeneratedNever();

            constructor.Property(empleado => empleado.NumeroDocumento)
                       .HasConversion(numeroDocumento => numeroDocumento.Valor,
                                      valor => new NumeroDocumento(valor))
                       .HasMaxLength(NumeroDocumento.LongitudMaxima)
                       .IsRequired();

            constructor.HasIndex(empleado => empleado.NumeroDocumento).IsUnique();

            constructor.Property(empleado => empleado.CorreoElectronico)
                       .HasConversion(correoElectronico => correoElectronico.Valor,
                                      valor => new CorreoElectronico(valor))
                       .HasMaxLength(CorreoElectronico.LongitudMaxima)
                       .IsRequired();

            constructor.HasIndex(empleado => empleado.CorreoElectronico).IsUnique();

            constructor.Property(empleado => empleado.Nombres)
                       .HasMaxLength(Empleado.LongitudMaximaNombres)
                       .IsRequired();

            constructor.Property(empleado => empleado.Apellidos)
                       .HasMaxLength(Empleado.LongitudMaximaNombres)
                       .IsRequired();

            constructor.Property(empleado => empleado.Cargo)
                       .HasMaxLength(Empleado.LongitudMaximaCargo)
                       .IsRequired();

            constructor.Property(empleado => empleado.Estado)
                       .HasConversion<int>()
                       .IsRequired();

            constructor.Property(empleado => empleado.FechaCreacion)
                       .HasConversion<ConvertidorFechaUtc>()
                       .HasColumnType("datetime2")
                       .IsRequired();

            constructor.Property(empleado => empleado.FechaDesactivacion)
                       .HasConversion<ConvertidorFechaUtc>()
                       .HasColumnType("datetime2");

            constructor.Ignore(empleado => empleado.EstaActivo);

            constructor.HasOne(empleado => empleado.Area)
                       .WithMany()
                       .HasForeignKey(empleado => empleado.AreaId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
