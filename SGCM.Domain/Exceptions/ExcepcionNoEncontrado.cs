namespace SGCM.Domain.Exceptions
{
    public class ExcepcionNoEncontrado : ExcepcionDominio
    {
        public ExcepcionNoEncontrado(string nombreEntidad, object key) 
            : base($"El recurso/entidad '{nombreEntidad}' no existe.", "ENTIDAD_NO_ENCONTRADA")
        {

        }
    }
}
