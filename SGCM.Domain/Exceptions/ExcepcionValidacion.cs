namespace SGCM.Domain.Exceptions
{
    public class ExcepcionValidacion : ExcepcionDominio
    {
        public ExcepcionValidacion(string mensaje) 
            : base(mensaje, "ERROR_DE_VALIDACION")
        {
        }
    }
}
