using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace gacs_app;

public class Program
{
  [STAThread]
  public static void Main(string[] args)
  {
    var host = Host.CreateDefaultBuilder(args)
                   .ConfigureServices((context, services) =>
                                      {
                                        services.AddSingleton<MainViewModel>();
                                        services.AddSingleton<MainWindow>();
                                        services.AddSingleton<IMyService, MyService>();
                                      })
                   .Build();

    var app = new App();
    var mainWindow = host.Services.GetRequiredService<MainWindow>();
    app.Run(mainWindow);
  }
}