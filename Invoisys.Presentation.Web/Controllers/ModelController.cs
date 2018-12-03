using AutoMapper;
using Invoisys.AppService.Interface;
using Invoisys.Domain.Entity;
using Invoisys.Infrastructure.CrossCutting.Util;
using Invoisys.Presentation.Web.DTO;
using Invoisys.Presentation.Web.Filters;
using Invoisys.Presentation.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Invoisys.Presentation.Web.Controllers
{
    [AccessDeniedAuthorize(Roles = "Admin")]
    public class ModelController : Controller
    {
        private readonly IModelAppService _appService;
        private readonly MapperConfiguration _config;
        public ModelController(IModelAppService appService)
        {
            _appService = appService;
            _config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Model, ModelDTO>();
                cfg.CreateMap<ModelDTO, Model>();
                cfg.CreateMap<ModelDTO, ModelViewModel>();
                cfg.CreateMap<ModelViewModel, ModelDTO>();
            });
        }
        [Authorize]
        public ActionResult Index()
        {
            var mapper = _config.CreateMapper();
            var models = _appService.GetAll().ToArray();
            return View(mapper.Map<IEnumerable<Model>, IEnumerable<ModelDTO>>(models));
        }
        [Authorize]
        public ActionResult Create() => View();
        [Authorize]
        [EncryptedActionParameter]
        public ActionResult Edit(int id)
        {
            var mapper = _config.CreateMapper();
            var model = mapper.Map<Model, ModelDTO>(_appService.GetById(id));
            var modelViewModel = mapper.Map<ModelDTO, ModelViewModel>(model);
            return View(modelViewModel);
        }
        [Authorize]
        [EncryptedActionParameter]
        public ActionResult Delete(int id)
        {
            var mapper = _config.CreateMapper();
            var model = mapper.Map<Model, ModelDTO>(_appService.GetById(id));
            var modelViewModel = mapper.Map<ModelDTO, ModelViewModel>(model);
            return View(modelViewModel);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ModelViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var mapper = _config.CreateMapper();
            var modelDto = mapper.Map<ModelViewModel, ModelDTO>(model);
            var modelEntity = mapper.Map<ModelDTO, Model>(modelDto);
            try
            {
                _appService.Add(modelEntity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR", ex);
                return View(model);
            }
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(string id, ModelViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            model.Id = EncodedActionLinkExtensions.Decrypt(id);
            var mapper = _config.CreateMapper();
            var modelDto = mapper.Map<ModelViewModel, ModelDTO>(model);
            var modelEntity = mapper.Map<ModelDTO, Model>(modelDto);
            try
            {
                _appService.Edit(modelEntity);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("ERROR", ex);
                return View(model);
            }
        }
        [Authorize]
        [ValidateAntiForgeryToken]
        [EncryptedActionParameter]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _appService.DeleteById(id);
            return RedirectToAction("Index");
        }
        [Authorize]
        public ActionResult DeleteAll()
        {
            _appService.DeleteAll();
            return RedirectToAction("Index");
        }
    }
}