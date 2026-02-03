namespace MvcCoreCrudPlantilla.Models
{
    public class ResumenPlantilla
    {
        public int MaximoSalario { get; set; }
        public int SumaSalarial { get; set; }
        public double MediaSalarial { get; set; }
        public List<Plantilla> Empleados { get; set; }
    }
}
