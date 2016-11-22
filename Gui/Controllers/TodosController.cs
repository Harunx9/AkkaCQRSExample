using Domain.Messages.Commands;
using Domain.ReadModel;
using Gui.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gui.Controllers
{
    public class TodosController : Controller
    {
        private readonly ITodoReadSideService _readService;
        private readonly IDomainBus _bus;

        public TodosController(IDomainBus bus ,ITodoReadSideService readService)
        {
            _bus = bus;
            _readService = readService;
        }

        public ActionResult Index(string filter = "active")
        {
            var list = _readService.GetAllActive();
            switch (filter)
            {
                case "all":
                    list = _readService.GetAll();
                    break;
                case "inactive":
                    list = _readService.GetAllInactive();
                    break;
            }
            return View(list);
        }


        public ActionResult Create()
        {
            return View(new CreateViewModel { Id = Guid.NewGuid() });
        }

        [HttpPost]
        public ActionResult Create(CreateViewModel vm)
        {
            var command = new CreateTodoCommand(vm.Id, vm.Title);
            _bus.Send(command);
            return RedirectToAction("Index");
        }

        public ActionResult Details(Guid uuid)
        {
            var item = _readService.GetDetails(uuid);
            var vm = new UpdateViewModel
            {
                Id = item.UUID,
                Title = item.Title,
                Desctiption = item.Description
            };
            return View(vm);
        }

        [HttpPost]
        public ActionResult Details(UpdateViewModel vm)
        {
            var command = new ChangeTodoStateCommand(vm.Id, vm.Title, vm.Desctiption);
            _bus.Send(command);
            return View(vm);
        }

        public ActionResult Close(Guid uuid)
        {
            return View(new CloseViewModel { Id = uuid });
        }

        [HttpPost]
        public ActionResult Close(CloseViewModel vm)
        {
            var command = new CloseTodoCommand(vm.Id);
            _bus.Send(command);
            return RedirectToActionPermanent("Index");
        }
    }
}