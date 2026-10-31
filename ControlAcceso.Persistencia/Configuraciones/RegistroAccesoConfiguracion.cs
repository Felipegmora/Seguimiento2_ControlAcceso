using ControlAcceso.Dominio.Entidades.RegistrosAcceso;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlAcceso.Persistencia.Configuraciones
{
    public sealed class RegistroAccesoConfiguracion : IEntityTypeConfiguration<RegistroAcceso>
    {
        public void Configure(EntityTypeBuilder<RegistroAcceso> constructor)
        {
            constructor.ToTable("RegistrosAcceso");

            constructor.HasKey(registro => registro.Id);
            constructor.Property(registro => registro.Id).ValueGeneratedNever();

            constructor.Property(registro => registro.Movimiento)
                       .HasConversion<int>()
                       .IsRequired();

            constructor.Property(registro => registro.Resultado)
                       .HasConversion<int>()
                       .IsRequired();

            constructor.Property(registro => registro.Motivo)
                       .HasMaxLength(RegistroAcceso.LongitudMaximaMotivo);

            constructor.Property(registro => registro.FechaMovimiento)
                       .HasConversion<ConvertidorFechaUtc>()
                       .HasColumnType("datetime2")
                       .IsRequired();

            constructor.HasIndex(registro => new { registro.EmpleadoId, registro.FechaMovimiento });

            constructor.HasOne(registro => registro.Empleado)
                       .WithMany()
                       .HasForeignKey(registro => registro.EmpleadoId)
                       .OnDelete(DeleteBehavior.Restrict);

            constructor.HasOne(registro => registro.Credencial)
                       .WithMany()
                       .HasForeignKey(registro => registro.CredencialId)
                       .OnDelete(DeleteBehavior.Restrict);

            constructor.HasOne(registro => registro.PuntoAcceso)
                       .WithMany()
                       .HasForeignKey(registro => registro.PuntoAccesoId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
