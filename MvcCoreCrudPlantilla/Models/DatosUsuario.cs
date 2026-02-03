namespace MvcCoreCrudPlantilla.Models
{
    public class DatosUsuario
    {
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Imagen { get; set; }
        public string NombreActividad { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public bool Capitan { get; set; }
        public string NombreCurso { get; set; }
    }
}
