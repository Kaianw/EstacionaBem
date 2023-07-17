using EstacionaBem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
namespace EstacionaBem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Precos()
        {
            List<PrecoModel> precos = new List<PrecoModel>();
            using (var db = new ParkContext())
            {
                precos = db.precos.ToList();
            }
            ViewBag.precos = precos;
            return View();
        }
        public IActionResult Estadias()
        {
            List<EstadiaModel> estadias = new List<EstadiaModel>();
            using (var db = new ParkContext())
            {
                estadias = db.estadias.ToList();
            }
            ViewBag.estadias = estadias;
            return View();
        }
        public IActionResult RegistrarEntrada()
        {
            return View();
        }
        public IActionResult RegistrarSaida()
        {
            return View();
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
                            return RedirectToAction("Precos", "Home");

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
                            return RedirectToAction("Estadias", "Home");
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
