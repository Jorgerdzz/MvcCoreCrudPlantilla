namespace MvcCoreCrudPlantilla.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Imagen { get; set; }
        public int IdCurso { get; set; }
    }
}
