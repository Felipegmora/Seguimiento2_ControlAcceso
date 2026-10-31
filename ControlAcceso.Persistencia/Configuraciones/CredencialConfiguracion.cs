using ControlAcceso.Dominio.Entidades.Credenciales;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ControlAcceso.Persistencia.Configuraciones
{
    public sealed class CredencialConfiguracion : IEntityTypeConfiguration<Credencial>
    {
        public void Configure(EntityTypeBuilder<Credencial> constructor)
        {
            constructor.ToTable("Credenciales");

            constructor.HasKey(credencial => credencial.Id);
            constructor.Property(credencial => credencial.Id).ValueGeneratedNever();

            constructor.Property(credencial => credencial.Codigo)
                       .HasMaxLength(Credencial.LongitudMaximaCodigo)
                       .IsRequired();

            constructor.HasIndex(credencial => credencial.Codigo).IsUnique();

            constructor.Property(credencial => credencial.Estado)
                       .HasConversion<int>()
                       .IsRequired();

            constructor.Property(credencial => credencial.FechaCreacion)
                       .HasConversion<ConvertidorFechaUtc>()
                       .HasColumnType("datetime2")
                       .IsRequired();

            constructor.Property(credencial => credencial.FechaBloqueo)
                       .HasConversion<ConvertidorFechaUtc>()
                       .HasColumnType("datetime2");

            constructor.Property(credencial => credencial.FechaRevocacion)
                       .HasConversion<ConvertidorFechaUtc>()
                       .HasColumnType("datetime2");

            constructor.Ignore(credencial => credencial.PuedeUtilizarse);

            constructor.HasOne(credencial => credencial.Empleado)
                       .WithMany()
                       .HasForeignKey(credencial => credencial.EmpleadoId)
                       .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
