using System;
using System.Timers;

using log4net;

using PluginBase;
using AppWithPlugin;

namespace HelloPlugin
{
  public class HelloPlugin : IPlugin
  {
    public string Name { get => "HelloPlugin"; }
    public string Description { get => "Demonstration plugin."; }
    private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    private static System.Timers.Timer TickTimer;

    public void Startup()
    {
      log.Info("Startup");

      // Test we can pull things out of AppWithPlugin
      log.Info(Program.Name);

      DoSomethingPeriodic();

      return;
    }

    public void Shutdown() {
      // TODO: Is this the right way to do this?
      TickTimer.Enabled = false;
      TickTimer.Dispose();
    }

    private void DoSomethingPeriodic()
    {
      TickTimer = new System.Timers.Timer(1000);
      TickTimer.Elapsed += OnTick;
      TickTimer.AutoReset = true;
      TickTimer.Enabled = true;
    }

    public void OnTick(Object source, ElapsedEventArgs args) {
      log.Info("OnTick");
    }
  }
}
