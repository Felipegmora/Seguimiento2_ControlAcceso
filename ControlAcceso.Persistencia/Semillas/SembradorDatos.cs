using ControlAcceso.Dominio.Entidades.Areas;
using ControlAcceso.Dominio.Entidades.PuntosAcceso;
using ControlAcceso.Dominio.Entidades.PuntosAcceso.ObjetosDeValor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ControlAcceso.Persistencia.Semillas
{
    public static class SembradorDatos
    {
        public static async Task SembrarAsync(IServiceProvider proveedorServicios, CancellationToken cancelacion = default)
        {
            using IServiceScope alcance = proveedorServicios.CreateScope();

            ContextoDatos contexto = alcance.ServiceProvider.GetRequiredService<ContextoDatos>();

            await SembrarAreasAsync(contexto, cancelacion);
            await SembrarPuntosAccesoAsync(contexto, cancelacion);
        }

        private static async Task SembrarAreasAsync(ContextoDatos contexto, CancellationToken cancelacion)
        {
            if (await contexto.Areas.AnyAsync(cancelacion))
            {
                return;
            }

            List<Area> areas =
            [
                new Area("Administración", "Gerencia, contabilidad y gestión administrativa"),
                new Area("Talento Humano", "Gestión del personal y vinculación laboral"),
                new Area("Logística", "Planeación y despacho de operaciones logísticas"),
                new Area("Almacenamiento", "Recepción, custodia y control de inventario en bodega"),
                new Area("Seguridad", "Control de accesos y vigilancia de las instalaciones"),
                new Area("Tecnología", "Soporte de sistemas e infraestructura tecnológica")
            ];

            await contexto.Areas.AddRangeAsync(areas, cancelacion);
            await contexto.SaveChangesAsync(cancelacion);
        }

        private static async Task SembrarPuntosAccesoAsync(ContextoDatos contexto, CancellationToken cancelacion)
        {
            if (await contexto.PuntosAcceso.AnyAsync(cancelacion))
            {
                return;
            }

            List<PuntoAcceso> puntosAcceso =
            [
                new PuntoAcceso("Recepción principal", TipoPuntoAcceso.Recepcion, "Ingreso principal de la sede"),
                new PuntoAcceso("Parqueadero", TipoPuntoAcceso.Parqueadero, "Acceso vehicular de empleados"),
                new PuntoAcceso("Bodega central", TipoPuntoAcceso.Bodega, "Zona de almacenamiento de mercancía"),
                new PuntoAcceso("Centro de datos", TipoPuntoAcceso.AreaRestringida, "Área restringida de servidores")
            ];

            await contexto.PuntosAcceso.AddRangeAsync(puntosAcceso, cancelacion);
            await contexto.SaveChangesAsync(cancelacion);
        }
    }
}
