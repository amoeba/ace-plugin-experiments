using System;
using System.Threading;

using log4net;

namespace AppWithPlugin;

public static class Prompt
{
  private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

  public static void Init()
  {
    var thread = new Thread(new ThreadStart(PromptThread));
    thread.Name = "Prompt Manager";
    thread.IsBackground = true;
    thread.Start();
    thread.Join(); // Removeme
  }

  private static void PromptThread()
  {
    bool running = true;

    Console.CancelKeyPress += delegate
    {
      log.Info("Preparing to shut down server");
      PluginManager.Shutdown();
      running = false;
    };

    Console.WriteLine("> ");

    while (running)
    {
      //
    }
  }
}
