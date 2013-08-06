using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using NUnit.Framework;

namespace InversionOfControl {

  public enum LifeCycle {
    Transient,
    Singleton
  }

  public class IOCContainer {

    private Dictionary<Type, Func<object>> RegisteredTypes = new Dictionary<Type, Func<object>>();
    private Dictionary<Type, object> Objects = new Dictionary<Type, object>();

    public void Register<TInterface, TClass>(LifeCycle lc = LifeCycle.Transient)
      where TClass : class, TInterface {
      Type type = typeof(TClass);
      ConstructorInfo constructor = type.GetConstructors().FirstOrDefault();
      object[] paramObj = this.GetArguments(constructor);

      // Register the new type with the parameters found.
      RegisteredTypes.Add(typeof(TInterface), () => constructor.Invoke(paramObj));
      if (lc == LifeCycle.Singleton) {
        Objects.Add(typeof(TInterface), constructor.Invoke(paramObj));
      }
    }

    public TInterface Resolve<TInterface>() {
      Type type = typeof(TInterface);
      return (TInterface) this.GetRegisteredObject(type);
    }

    public object Resolve(Type t) {
      return this.GetRegisteredObject(t);
    }

    public bool IsTypeRegistered(Type t) {
      if (RegisteredTypes.ContainsKey(t))
        return true;
      return false;
    }

    #region Private helper methods

    private object[] GetArguments(ConstructorInfo constructor) {
      ParameterInfo[] parameters = constructor.GetParameters();
      List<object> arguments = new List<object>();
      // create list of arguments with which to construct the new type from types already registered.
      foreach (ParameterInfo p in parameters) {
        Type parameterType = p.ParameterType;
        if (this.IsTypeRegistered(parameterType)) {
          arguments.Add(this.GetRegisteredObject(parameterType));
        } else {
          // At least one parameter could not be resolved.
          throw new Exception("At least one of the required parameters for the type you tried to register could not be resolved. Please register these types first and try again.");
        }
      }
      return arguments.ToArray();
    }

    private object GetRegisteredObject(Type type) {
      if (Objects.ContainsKey(type))
        return Objects[type];
      else if (RegisteredTypes.ContainsKey(type)) {
        return RegisteredTypes[type]();
      } else
        throw new Exception("The type you tried to resolve has not been registered.");
    }
    #endregion
  }
}