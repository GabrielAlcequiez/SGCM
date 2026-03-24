using SGCM.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace SGCM.Domain.Validaciones
{
    public static class ValidacionBase<T>
    {
        private static readonly Regex EmailRegex = new(
            @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
            RegexOptions.Compiled);

        private static readonly Regex TelefonoRegex = new(
            @"^\+?[0-9]{9,15}$",
            RegexOptions.Compiled);

        private static readonly Regex RNCRegex = new(
            @"^[0-9]{9,11}$",
            RegexOptions.Compiled);

        public static string Requerido(string? valor, string nombreCampo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new ExcepcionValidacion($"{nombreCampo} es obligatorio.");
            return valor;
        }

        public static string Longitud(string valor, int max, string nombreCampo, int min = 0)
        {
            if (valor.Length > max)
                throw new ExcepcionValidacion($"{nombreCampo} no puede exceder {max} caracteres.");
            if (min > 0 && valor.Length < min)
                throw new ExcepcionValidacion($"{nombreCampo} debe tener al menos {min} caracteres.");
            return valor;
        }

        public static string Email(string email)
        {
            if (!EmailRegex.IsMatch(email))
                throw new ExcepcionValidacion("El formato del email es inválido.");
            return email;
        }

        public static string Telefono(string telefono)
        {
            if (!TelefonoRegex.IsMatch(telefono))
                throw new ExcepcionValidacion("El formato del teléfono es inválido.");
            return telefono;
        }

        public static string RNC(string rnc)
        {
            if (!RNCRegex.IsMatch(rnc))
                throw new ExcepcionValidacion("El formato del RNC es inválido.");
            return rnc;
        }

        public static DateOnly FechaNoFutura(DateOnly fecha, string nombreCampo)
        {
            if (fecha > DateOnly.FromDateTime(DateTime.Now))
                throw new ExcepcionValidacion($"{nombreCampo} no puede ser en el futuro.");
            return fecha;
        }

        public static DateTime FechaHoraNoPasada(DateTime fecha, string nombreCampo)
        {
            if (fecha < DateTime.Now)
                throw new ExcepcionValidacion($"{nombreCampo} no puede ser en el pasado.");
            return fecha;
        }

        public static int IdValido(int id, string nombreCampo)
        {
            if (id <= 0)
                throw new ExcepcionValidacion($"{nombreCampo} debe ser válido.");
            return id;
        }

        public static TimeOnly RangoHora(TimeOnly hora, TimeOnly min, TimeOnly max, string nombreCampo)
        {
            if (hora < min || hora > max)
                throw new ExcepcionValidacion($"{nombreCampo} debe estar entre {min} y {max}.");
            return hora;
        }

        public static TimeOnly HoraValida(TimeOnly hora, string nombreCampo)
        {
            if (hora < TimeOnly.MinValue || hora > TimeOnly.MaxValue)
                throw new ExcepcionValidacion($"{nombreCampo} no es una hora válida.");
            return hora;
        }
    }
}
