using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NUnit.Framework;

namespace InversionOfControl {
  [TestFixture]
  public class Tests {

    IOCContainer container;

    [SetUp]
    public void SetUp() {
      this.container = new IOCContainer();
    }

    [Test]
    public void RegisterType() {
      container.Register<IEnumerable<string>, List<string>>();
      Assert.True(container.IsTypeRegistered(typeof(IEnumerable<string>)));
    }

    [Test]
    public void ResolveType() {
      container.Register<IEnumerable<string>, List<string>>();
      IEnumerable<string> list = container.Resolve<IEnumerable<string>>();
      Assert.IsInstanceOf(typeof(List<string>), list);
      Assert.True(container.IsTypeRegistered(typeof(IEnumerable<string>)));
    }

    [Test]
    public void ResolveTypeAlternateMethod() {
      container.Register<IEnumerable<string>, List<string>>();
      object list = container.Resolve(typeof(IEnumerable<string>));
      Assert.IsInstanceOf(typeof(List<string>), list);
      Assert.True(container.IsTypeRegistered(typeof(IEnumerable<string>)));
    }

    [Test]
    [ExpectedException()]
    public void ResolveUnregisteredType() {
      container.Resolve<IBox>();
    }

    [Test]
    public void TypeNotRegistered() {
      Assert.False(container.IsTypeRegistered(typeof(IBox)));
    }

    [Test]
    public void TransientLifeCycle() {
      container.Register<IEnumerable<string>, List<string>>();
      IEnumerable<string> list = container.Resolve<IEnumerable<string>>();
      Assert.False(list == container.Resolve<IEnumerable<string>>());
    }

    [Test]
    public void SingletonLifeCycle() {
      container.Register<IEnumerable<string>, List<string>>(LifeCycle.Singleton);
      IEnumerable<string> list = container.Resolve<IEnumerable<string>>();
      Assert.True(list == container.Resolve<IEnumerable<string>>());
    }

    [Test]
    public void DependencyInjection() {
      container.Register<IBox, Box>();
      container.Register<IBoxConsumer, BoxConsumer>();
      Assert.True(container.Resolve<IBox>().Name == container.Resolve<IBoxConsumer>().TheString);
    }

    [Test]
    public void DependencyInjectionLifeCycles() {
      container.Register<IBox, Box>(LifeCycle.Singleton);
      container.Register<IBoxConsumer, BoxConsumer>(LifeCycle.Transient);
      Assert.True(container.Resolve<IBox>().Name == container.Resolve<IBoxConsumer>().TheString);
      Assert.True(container.Resolve<IBox>() == container.Resolve<IBox>());
      Assert.False(container.Resolve<IBoxConsumer>() == container.Resolve<IBoxConsumer>());
    }

    [Test]
    [ExpectedException()]
    public void DependencyInjectionUnregisteredType() {
      container.Register<IBoxConsumer, BoxConsumer>();
    }
  }
}