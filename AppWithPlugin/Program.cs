using System;
using System.Collections.Generic;
using System.Threading;

using log4net;
using log4net.Config;

namespace AppWithPlugin
{
  public class Program
  {
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    public static string Name { get; set; } = "PROGRAM";
    public static Dictionary<string, Thread> pluginThreads = new Dictionary<string, Thread>();

    static void Main(string[] args)
    {
      BasicConfigurator.Configure();
      log.Info("Main()");

      try
      {
        PluginManager.Init();

        // Hacky command prompt to keep the program running
        // so we can show plugins running continuously
        Prompt.Init();
      }
      catch (Exception ex)
      {
        Console.WriteLine(ex);
      }
    }
  }
}
