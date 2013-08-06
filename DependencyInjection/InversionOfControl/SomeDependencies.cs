using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InversionOfControl {

  // These are just some classes to use for testing purposes.

  public interface IBoxConsumer { string TheString { get; } }

  public class BoxConsumer : IBoxConsumer {

    public string TheString { get; set; }

    public BoxConsumer(IBox b) {
      this.TheString = b.Name;
    }
  }

  public interface IBox { string Name { get; } }

  public class Box : IBox {
    public string Name { get; set; }

    public Box() {
      this.Name = "A really cool box.";
    }
  }
}
