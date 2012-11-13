using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Annotations.Storage;
using System.Windows.Annotations;

namespace Annotations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        FileStream stream;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            // Enables and Loads Annotations
            AnnotationService service = AnnotationService.GetService(reader);
            if (service == null)
            {
                stream = new FileStream("storage.xml", FileMode.OpenOrCreate);
                service = new AnnotationService(reader);
                AnnotationStore store = new XmlStreamStore(stream);
                store.AutoFlush = true;
                service.Enable(store);
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // Disables and saves annotations
            AnnotationService service = AnnotationService.GetService(reader);
            if (service != null && service.IsEnabled) 
            {
                service.Disable();
                stream.Close();
            }
        }
    }
}
