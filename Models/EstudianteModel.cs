using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RegistroAlumnos0.Models
{
    public class EstudianteModel
    {
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Nombre { get; set; }

        
        [Display(Name = "Fecha Nacimiento")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string FechaNacimiento { get; set; }

        [Display(Name = "Curso")]
        [Required(ErrorMessage = "Campo obligatorio")]
        public string Curso { get; set; }

        [Display (Name = "Estado")]
       
        public int Estado { get; set; }

        public string DescripEstado { get; set; }
    }
}
