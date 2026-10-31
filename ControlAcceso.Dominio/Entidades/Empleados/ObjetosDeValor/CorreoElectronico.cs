using System.Text.RegularExpressions;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor
{
    public sealed record CorreoElectronico
    {
        public const int LongitudMaxima = 150;

        private static readonly Regex FormatoCorreo = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

        public string Valor { get; }

        public CorreoElectronico(string valor)
        {
            AplicarReglasValor(valor);

            Valor = valor.Trim().ToLowerInvariant();
        }

        private static void AplicarReglasValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ExcepcionReglaNegocio("El correo electrónico es requerido.");
            }

            string valorDepurado = valor.Trim();

            if (valorDepurado.Length > LongitudMaxima)
            {
                throw new ExcepcionReglaNegocio($"El correo electrónico debe tener máximo {LongitudMaxima} caracteres.");
            }

            if (!FormatoCorreo.IsMatch(valorDepurado))
            {
                throw new ExcepcionReglaNegocio("El correo electrónico no tiene un formato válido.");
            }
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
