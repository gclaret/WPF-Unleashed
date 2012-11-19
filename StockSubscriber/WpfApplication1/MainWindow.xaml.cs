using Microsoft.Practices.Unity;
using StockSubscriber.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApplication1.Model;


namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewVm _vm;

        [Dependency]
        public MainViewVm VM
        {
            set
            {
                _vm = value;
                this.DataContext = _vm;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
        }

    }
}
