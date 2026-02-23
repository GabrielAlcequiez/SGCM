using SGCM.Domain.Exceptions;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;

namespace SGCM.Domain.Services
{
    public class AuditoriaLogsDomainService : IAuditoriaLogsDomainService
    {
        public bool EsAccionCritica(string accion)
        {
            if (string.IsNullOrWhiteSpace(accion)) return false;

            var accionesCriticas = new[]
            {
                "ELIMINAR_PACIENTE",
                "ELIMINAR_MEDICO",
                "ELIMINAR_USUARIO",
                "ASIGNAR_PERFIL_ADMIN",
                "MODIFICAR_HISTORIAL_CLINICO", 
                "DESACTIVAR_PROVEEDOR_ARS"
            };

            return accionesCriticas.Contains(accion.ToUpper());
        }

    }
}
