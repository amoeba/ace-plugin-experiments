using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;

using log4net;

using PluginBase;

namespace AppWithPlugin;

public static class PluginManager
{
  private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
  private static IEnumerable<IPlugin> plugins;

  public static void Init()
  {
    log.Info("Init()");

    // TODO: Factor out into config
    string[] pluginPaths = new string[]
    {
          @"HelloPlugin\bin\Debug\net6.0\HelloPlugin.dll"
    };

    plugins = pluginPaths.SelectMany(pluginPath =>
    {
      Assembly pluginAssembly = LoadPluginAssembly(pluginPath);
      return LoadPlugin(pluginAssembly);
    }).ToList();

    foreach (IPlugin plugin in plugins)
    {
      log.InfoFormat("Starting plugin {0}", plugin.Name);

      Thread t = new Thread(new ThreadStart(plugin.Startup));
      t.Name = plugin.Name;
      t.Start();
    }
  }

  public static void Shutdown()
  {
    foreach (IPlugin plugin in plugins)
    {
      try
      {
        log.InfoFormat("Attempting to shut down plugin {0}", plugin.Name);
        plugin.Shutdown();
        log.InfoFormat("Successfully shut down plugin {0}", plugin.Name);
      }
      catch (Exception ex)
      {
        log.Error(ex);
      }
    }
  }

  static Assembly LoadPluginAssembly(string relativePath)
  {
    // Navigate up to the solution root
    string root = Path.GetFullPath(Path.Combine(
        Path.GetDirectoryName(
            Path.GetDirectoryName(
                Path.GetDirectoryName(
                    Path.GetDirectoryName(
                        Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));

    string pluginLocation = Path.GetFullPath(Path.Combine(root, relativePath.Replace('\\', Path.DirectorySeparatorChar)));
    Console.WriteLine($"Loading plugins from: {pluginLocation}");
    PluginLoadContext loadContext = new PluginLoadContext(pluginLocation);
    return loadContext.LoadFromAssemblyName(new AssemblyName(Path.GetFileNameWithoutExtension(pluginLocation)));
  }

  static IEnumerable<IPlugin> LoadPlugin(Assembly assembly)
  {
    log.InfoFormat("LoadPlugin {0}", assembly.FullName);

    int count = 0;

    foreach (Type type in assembly.GetTypes())
    {
      if (typeof(IPlugin).IsAssignableFrom(type))
      {
        IPlugin result = Activator.CreateInstance(type) as IPlugin;

        if (result != null)
        {
          count++;
          yield return result;
        }
      }
    }

    if (count == 0)
    {
      string availableTypes = string.Join(",", assembly.GetTypes().Select(t => t.FullName));
      throw new ApplicationException(
          $"Can't find any type which implements IPlugin in {assembly} from {assembly.Location}.\n" +
          $"Available types: {availableTypes}");
    }
  }
}
