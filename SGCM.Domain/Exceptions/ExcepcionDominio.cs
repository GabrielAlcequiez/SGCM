namespace SGCM.Domain.Exceptions
{
    public abstract class ExcepcionDominio : Exception
    {
        public string CodigoError { get; }
        protected ExcepcionDominio(string mensaje, string codigoError) 
        {
            CodigoError = codigoError;
        }
    }
}
