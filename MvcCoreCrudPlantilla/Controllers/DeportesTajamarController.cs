using Microsoft.AspNetCore.Mvc;
using MvcCoreCrudPlantilla.Models;
using MvcCoreCrudPlantilla.Repositories;

namespace MvcCoreCrudPlantilla.Controllers
{
    public class DeportesTajamarController : Controller
    {
        RepositoryDeportesTajamar repo;

        public DeportesTajamarController()
        {
            this.repo = new RepositoryDeportesTajamar();
        }

        public IActionResult Index()
        {
            List<Usuario> usuarios = this.repo.GetUsuarios();
            return View(usuarios);
        }

        public async Task<IActionResult> DatosUsuario(int id)
        {
            DatosUsuario user = await this.repo.GetDatosUsuario(id);
            return View(user);
        }

    }
}
