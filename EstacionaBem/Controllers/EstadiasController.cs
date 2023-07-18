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
    public class EstadiasController : Controller
    {
        public async Task<IActionResult> Index(int? pageIndex)
        {
            IQueryable<EstadiaModel> estadias;
            PagedList<EstadiaModel> list;
            int pageSize = 10;
            using (var db = new ParkContext())
            {
                estadias = db.estadias.AsNoTracking().OrderByDescending(o => o.id);
                list = await PagedList<EstadiaModel>.CreateAsync(estadias, pageIndex ?? 1, pageSize);
            }

            return View(list);
        }

        public IActionResult RegistrarEntrada()
        {
            return View();
        }

        public IActionResult RegistrarSaida()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RegistrarEntrada(EntradaModel entrada)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ParkContext())
                {
                    EstadiaModel verificaEntradaExistente =
                                        (from e in db.estadias
                                         where e.placa == entrada.placa
                                         && e.saida == null
                                         select e).SingleOrDefault();
                    PrecoModel precoExistente =
                                        (from p in db.precos
                                         where entrada.chegada >= p.vigenciaInicio
                                         && entrada.chegada <= p.vigenciaFim
                                         select p).SingleOrDefault();
                    if (verificaEntradaExistente == null)
                    {
                        if (precoExistente != null)
                        {
                            EstadiaModel novaEstadia = new EstadiaModel();
                            novaEstadia.placa = entrada.placa;
                            novaEstadia.chegada = entrada.chegada;
                            db.Add(novaEstadia);
                            db.SaveChanges();
                            return RedirectToAction("Index", "Estadias");
                        }
                        else
                        {
                            ViewBag.errorMessages += "Não existe vigência de preço no periodo desta entrada";
                        }
                    }
                    else
                        ViewBag.errorMessages += "Este Veiculo já está no pátio";
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult RegistrarSaida(SaidaModel saidaModelo)
        {
            if (ModelState.IsValid)
            {
                using (var db = new ParkContext())
                {
                    EstadiaModel estadia = (from e in db.estadias
                                            where saidaModelo.placa == e.placa
                                            && e.saida == null
                                            select e).SingleOrDefault();
                    if (estadia != null)
                    {
                        estadia.saida = saidaModelo.saida;
                        TimeSpan? diferenca = estadia.saida - estadia.chegada;
                        PrecoModel precoCorreto =
                                        (from p in db.precos
                                         where estadia.chegada >= p.vigenciaInicio
                                         && estadia.chegada <= p.vigenciaFim
                                         select p).SingleOrDefault();
                        if (precoCorreto != null)
                        {
                            if (diferenca.Value.Days == 0 && diferenca.Value.Hours == 0 && diferenca.Value.Minutes <= 30)
                                estadia.preco = (precoCorreto.precoHoraInicial.Value / 2);
                            else if (diferenca.Value.Minutes > 10)
                                estadia.preco = (precoCorreto.precoHoraInicial.Value + precoCorreto.precoHoraAdicional.Value * (diferenca.Value.Hours + 1) + precoCorreto.precoHoraAdicional.Value * diferenca.Value.Days * 24);
                            else
                                estadia.preco = (precoCorreto.precoHoraInicial.Value + precoCorreto.precoHoraAdicional.Value * diferenca.Value.Hours + precoCorreto.precoHoraAdicional.Value * diferenca.Value.Days * 24);
                            db.Attach(estadia);
                            db.SaveChanges();
                            return RedirectToAction("Estadias", "Home");
                        }
                        else
                            ViewBag.errorMessages += "Não existe um preço vigente para esta data de entrada";
                    }
                    else
                        ViewBag.errorMessages += "Não existe um veiculo estacionado no pátio com esta placa";


                }
            }

            return View();
        }
    }
}
