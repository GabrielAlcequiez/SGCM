namespace SGCM.Domain.Exceptions
{
    public class ExcepcionReglaNegocio : ExcepcionDominio
    {
        public ExcepcionReglaNegocio(string mensaje, string codigoError = "REGLA_DE_NEGOCIO_VIOLADA") 
            : base(mensaje, codigoError)
        {
        }
    }
}
