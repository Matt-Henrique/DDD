using System.ComponentModel.DataAnnotations;

namespace Invoisys.Presentation.Web.ViewModels
{
    public class ModelViewModel
    {
        [Key]
        public string Id { get; set; }
        [MaxLength(100, ErrorMessage = "{0} deve conter no máximo {1} caracteres")]
        [Required(ErrorMessage = "Por favor, informe o Nome")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}