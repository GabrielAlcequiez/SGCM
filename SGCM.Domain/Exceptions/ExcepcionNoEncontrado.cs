namespace SGCM.Domain.Exceptions
{
    public class ExcepcionNoEncontrado : ExcepcionDominio
    {
        public ExcepcionNoEncontrado(string nombreEntidad, object key) 
            : base($"El recurso/entidad '{nombreEntidad}' con ID: '{key}' no existe.", "NO_ENCONTRADO")
        {
        }

        public ExcepcionNoEncontrado(string mensaje) 
            : base(mensaje, "NO_ENCONTRADO")
        {
        }
    }
}
