using System.Text.Json.Serialization;
using ControlAcceso.Api.Manejadores;
using ControlAcceso.Aplicacion;
using ControlAcceso.Persistencia;
using ControlAcceso.Persistencia.Semillas;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder constructor = WebApplication.CreateBuilder(args);

constructor.Services.AddControllers()
                    .AddJsonOptions(opciones =>
                    {
                        opciones.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });

constructor.Services.AddOpenApi();

constructor.Services.AgregarServiciosAplicacion();
constructor.Services.AgregarServiciosPersistencia(constructor.Configuration);

constructor.Services.AddExceptionHandler<ManejadorExcepcionesApi>();
constructor.Services.AddProblemDetails();

WebApplication aplicacion = constructor.Build();

aplicacion.UseExceptionHandler();

if (aplicacion.Environment.IsDevelopment())
{
    aplicacion.MapOpenApi();

    using (IServiceScope alcance = aplicacion.Services.CreateScope())
    {
        ContextoDatos contexto = alcance.ServiceProvider.GetRequiredService<ContextoDatos>();

        await contexto.Database.MigrateAsync();
    }

    await SembradorDatos.SembrarAsync(aplicacion.Services);
}

aplicacion.MapControllers();

aplicacion.Run();
