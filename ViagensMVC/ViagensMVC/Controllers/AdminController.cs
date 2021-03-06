﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ViagensMVC.Bd;
using ViagensMVC.Models;

namespace ViagensMVC.Controllers
{
    public class AdminController : Controller
    {
        private const string ActionDestinoListagem = "DestinoListagem";

        private ViagensOnlineDb ObterDbContext()
        {
            return new ViagensOnlineDb();

        }

        [HttpGet]
        public ActionResult DestinoListagem()
        {
            List<Destino> lista = null;
            using (var db = ObterDbContext())
            {
                lista = db.Destino.ToList();
            }

            return View(lista);
        }

        [HttpGet]
        public ActionResult DestinoNovo()
        {
            ViewBag.Title = "Destino Novo";
            return View("DestinoFormulario");
        }

        [HttpGet]
        public ActionResult DestinoAlterar(int id)
        {
            return CarregarDestino(id, "Destino Alterar", "DestinoFormulario");
        }

        [HttpGet]
        public ActionResult DestinoExcluir(int id)
        {
            return CarregarDestino(id, "Destino Excluir", "DestinoExcluir");
        }

        [HttpPost]
        public ActionResult DestinoSalvar(Destino destino)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new ValidationException("Favor preencher todos os campos obrigatórios");
                }

                if (string.IsNullOrEmpty(destino.Foto) && (Request.Files.Count == 0 ||
                    Request.Files[0].ContentLength == 0))
                {
                    throw new ArgumentException("É Necessário enviar uma foto");
                }

                using (var db = ObterDbContext())
                {

                    if (destino.DestinoId > 0)
                    {
                        var destinoOriginal = db.Destino.Find(destino.DestinoId);

                        destinoOriginal.Nome = destino.Nome;
                        destinoOriginal.Pais = destino.Pais;
                        destinoOriginal.Cidade = destino.Cidade;

                        if (Request.Files.Count != 0 && Request.Files[0].ContentLength != 0)
                        {
                            destinoOriginal.Foto = GravarFoto(Request);
                        }
                    }
                    else
                    {
                        destino.Foto = GravarFoto(Request);
                        db.Destino.Add(destino);

                    }

                    db.SaveChanges();
                    return RedirectToAction(ActionDestinoListagem);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("DestinoFormulario", destino);
            }
        }

        [HttpPost]
        public ActionResult DestinoExcluir(int id, FormCollection form)
        {
            if (id > 0)
            {
                using (var db = ObterDbContext())
                {
                    var destino = db.Destino.Find(id);

                    if (destino != null)
                    {
                        db.Destino.Remove(destino);
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction(ActionDestinoListagem);
        }

        private string GravarFoto(HttpRequestBase Request)
        {
            string nome = Path.GetFileName(Request.Files[0].FileName);
            string pathVirtual = "~/Imagens";
            pathVirtual += ("/" + nome);
            string pathFisico = Request.MapPath(pathVirtual);

            Request.Files[0].SaveAs(pathFisico);

            return nome;
        }


        private ActionResult CarregarDestino(int id, string title, string view)
        {
            using (var db = ObterDbContext())
            {
                var destino = db.Destino.Find(id);

                if (destino != null)
                {
                    ViewBag.Title = title;
                    return View(view, destino);
                }
            }

            return RedirectToAction(ActionDestinoListagem);
        }
    }
}