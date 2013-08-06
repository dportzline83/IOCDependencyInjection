using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using InversionOfControl;
using InversionOfControl.Web.Controllers;

namespace InversionOfControl.Web.Infrastructure {
  public class CustomControllerFactory : DefaultControllerFactory {

    private IOCContainer _container;

    public CustomControllerFactory(IOCContainer container) : base() {
      this._container = container;
      RegisterTypes();
    }

    private void RegisterTypes() {
      // This is where types for the MVC App are registered. This gives single point of control for any types handled by the IOC container.s
      _container.Register<IBox, Box>();
      _container.Register<HomeController, HomeController>();
    }

    protected override IController GetControllerInstance(RequestContext context, Type controllerType) {
      return this._container.Resolve(controllerType) as Controller;
    }
  }
}