using System.ComponentModel.DataAnnotations;

namespace webapi_administradores.ModelViews
{
    public class AdmLoginView
    {
        #region "Propriedades"

        [Required]
        [MaxLength(150)]
        public string Email { get; set; }
        
        [Required]
        [MaxLength(10)]
        public string Senha { get; set; }

        #endregion
    }
}