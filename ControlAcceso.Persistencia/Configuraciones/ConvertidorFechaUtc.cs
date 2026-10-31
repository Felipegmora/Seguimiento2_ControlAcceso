using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ControlAcceso.Persistencia.Configuraciones
{
    public sealed class ConvertidorFechaUtc : ValueConverter<DateTime, DateTime>
    {
        public ConvertidorFechaUtc()
            : base(fecha => fecha.ToUniversalTime(),
                   fecha => DateTime.SpecifyKind(fecha, DateTimeKind.Utc))
        {
        }
    }
}
