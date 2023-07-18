using EstacionaBem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace EstacionaBem.Controllers
{
    public class PrecosController : Controller
    {
        public async Task<IActionResult> Index(int? pageIndex)
        {
            IQueryable<PrecoModel> precos;
            PagedList<PrecoModel> list;
            int pageSize = 10;
            using (var db = new ParkContext())
            {
                precos = db.precos.AsNoTracking().OrderByDescending(o => o.id);
                list = await PagedList<PrecoModel>.CreateAsync(precos, pageIndex ?? 1, pageSize);
            }

            return View(list);
        }

        public IActionResult RegistrarPreco()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarPreco(PrecoModel price)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ParkContext())
                {
                    PrecoModel precoConflitos;
                    using (var transaction = db.Database.BeginTransaction())
                    {

                        precoConflitos =
                            (from p in db.precos
                             where price.vigenciaInicio > p.vigenciaInicio
                             && price.vigenciaInicio < p.vigenciaFim
                             || price.vigenciaFim > p.vigenciaInicio
                             && price.vigenciaFim < p.vigenciaFim
                             || p.vigenciaInicio > price.vigenciaInicio
                             && p.vigenciaInicio < price.vigenciaFim
                             || p.vigenciaFim > price.vigenciaInicio
                             && p.vigenciaFim < price.vigenciaFim
                             select p).SingleOrDefault();

                        if (precoConflitos == null)
                        {

                            db.Add(price);
                            db.SaveChanges();
                            transaction.Commit();
                            return RedirectToAction("Index", "Precos");

                        }
                        else
                        {
                            ViewBag.errorMessages += "O preço é conflitante com a vigência de outro preço já existente";
                        }

                    }

                }
            }
            return View();
        }

    }
}
