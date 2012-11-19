using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApplication1.Model;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IUnityContainer container = new UnityContainer();

            RandomQuoteSource source = new RandomQuoteSource();
            container.RegisterInstance<IQuoteSource>(source);

            MainWindow window = container.Resolve<MainWindow>();
            window.Show();

            MainWindow window2 = container.Resolve<MainWindow>();
            window2.Show();
        }

    }
}
