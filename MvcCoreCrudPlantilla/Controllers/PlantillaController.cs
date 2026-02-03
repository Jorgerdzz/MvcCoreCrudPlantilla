using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudPlantilla.Models;
using MvcCoreCrudPlantilla.Repositories;

namespace MvcCoreCrudPlantilla.Controllers
{
    public class PlantillaController : Controller
    {
        RepositoryPlantilla repo;

        public PlantillaController()
        {
            this.repo = new RepositoryPlantilla();
        }
        public IActionResult Index()
        {
            List<Plantilla> plantilla = this.repo.GetPlantilla();
            return View(plantilla);
        }

        public IActionResult Details(int id)
        {
            Plantilla empleado = this.repo.FindEmpleadoPlantilla(id);
            return View(empleado);
        }

        public IActionResult BuscadorPlantilla()
        {
            List<string> funciones = this.repo.GetFuncionesPlantilla();
            ViewData["FUNCIONES"] = funciones;
            return View();
        }

        [HttpPost]
        public IActionResult BuscadorPlantilla(string funcion)
        {
            ResumenPlantilla plantilla = this.repo.GetEmpleadosPlantillaFuncion(funcion);
            List<string> funciones = this.repo.GetFuncionesPlantilla();
            ViewData["FUNCIONES"] = funciones;
            return View(plantilla);
        }

        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(int idHospital, int idSala, int idEmpleado, string apellido, string funcion, string turno, int salario)
        {
            await this.repo.InsertEmpleado(idHospital, idSala, idEmpleado, apellido, funcion, turno, salario);
            return RedirectToAction("Index");
        }

        public IActionResult Update(int id)
        {
            Plantilla empleado = this.repo.FindEmpleadoPlantilla(id);
            return View(empleado);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int idHospital, int idSala, int idEmpleado, string apellido, string funcion, string turno, int salario)
        {
            await this.repo.UpdateEmpleado(idHospital, idSala, idEmpleado, apellido, funcion, turno, salario);
            return RedirectToAction("Index");
        }

    }
}
