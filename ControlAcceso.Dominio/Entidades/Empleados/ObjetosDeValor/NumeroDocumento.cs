using System.Text.RegularExpressions;
using ControlAcceso.Dominio.Excepciones;

namespace ControlAcceso.Dominio.Entidades.Empleados.ObjetosDeValor
{
    public sealed record NumeroDocumento
    {
        public const int LongitudMinima = 5;
        public const int LongitudMaxima = 20;

        private static readonly Regex FormatoDocumento = new(@"^[A-Za-z0-9]+$", RegexOptions.Compiled);

        public string Valor { get; }

        public NumeroDocumento(string valor)
        {
            AplicarReglasValor(valor);

            Valor = valor.Trim().ToUpperInvariant();
        }

        private static void AplicarReglasValor(string valor)
        {
            if (string.IsNullOrWhiteSpace(valor))
            {
                throw new ExcepcionReglaNegocio("El número de identificación es requerido.");
            }

            string valorDepurado = valor.Trim();

            if (valorDepurado.Length < LongitudMinima || valorDepurado.Length > LongitudMaxima)
            {
                throw new ExcepcionReglaNegocio($"El número de identificación debe tener entre {LongitudMinima} y {LongitudMaxima} caracteres.");
            }

            if (!FormatoDocumento.IsMatch(valorDepurado))
            {
                throw new ExcepcionReglaNegocio("El número de identificación solo puede contener letras y números.");
            }
        }

        public override string ToString()
        {
            return Valor;
        }
    }
}
