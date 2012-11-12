using System;
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

namespace Panes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ColumnDefinition column1CloneForLayer0;
        public ColumnDefinition column2CloneForLayer0;
        public ColumnDefinition column2CloneForLayer1;

        public MainWindow()
        {
            InitializeComponent();

            //Dummy Columns used for docking
            column1CloneForLayer0 = new ColumnDefinition();
            column1CloneForLayer0.SharedSizeGroup = "column1";
            column2CloneForLayer0 = new ColumnDefinition();
            column2CloneForLayer0.SharedSizeGroup = "column2";
            column2CloneForLayer1 = new ColumnDefinition();
            column2CloneForLayer1.SharedSizeGroup = "column2";
        }

        // Show Pane1 when hovering over the button
        private void button1_MouseEnter(object sender, MouseEventArgs e)
        {
            layer1.Visibility = Visibility.Visible;
            Grid.SetZIndex(layer1, 1);
            Grid.SetZIndex(layer2, 0);
            if (button2.Visibility == Visibility.Visible)
                layer2.Visibility = Visibility.Collapsed;
        }

        // Show Pane2 when hovering over the button
        private void button2_MouseEnter(object sender, MouseEventArgs e)
        {
            layer2.Visibility = Visibility.Visible;
            Grid.SetZIndex(layer1, 0);
            Grid.SetZIndex(layer2, 1);
            if (button1.Visibility == Visibility.Visible)
                layer1.Visibility = Visibility.Collapsed;
        }

        // When mouse enters the main pane, if layers aren't docked, they are collapsed
        private void layer0_MouseEnter(object sender, MouseEventArgs e)
        {
            if (button1.Visibility == Visibility.Visible)
                layer1.Visibility = Visibility.Collapsed;
            if (button2.Visibility == Visibility.Visible)
                layer2.Visibility = Visibility.Collapsed;

        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {            
            if (button2.Visibility == Visibility.Visible)
                layer2.Visibility = Visibility.Collapsed;
        }

        // Toggle between docked and undocked for Pane1
        private void panel1Pin_Click(object sender, RoutedEventArgs e)
        {
            if (button1.Visibility == Visibility.Visible)
                DockPane(1);
            else if (button1.Visibility == Visibility.Collapsed)
                UndockPane(1);
        }

        private void Grid_MouseEnter_1(object sender, MouseEventArgs e)
        {
            if (button1.Visibility == Visibility.Visible)
                layer1.Visibility = Visibility.Collapsed;
        }

        // Toggle between docked and undocked for Pane2
        private void panel2Pin_Click(object sender, RoutedEventArgs e)
        {
            if (button2.Visibility == Visibility.Visible)
                DockPane(2);
            else if (button2.Visibility == Visibility.Collapsed)
                UndockPane(2);
        }

        public void DockPane(int paneNumber) 
        {
            if (paneNumber == 1) 
            {
                button1.Visibility = Visibility.Collapsed;
                //Adds the cloned column to layer 0
                layer0.ColumnDefinitions.Add(column1CloneForLayer0);
                //Adds the cloned column to layer 1, only if pane 2 is docked
                if (button2.Visibility == Visibility.Collapsed)
                    layer1.ColumnDefinitions.Add(column2CloneForLayer1);
            }
            else if (paneNumber == 2) 
            {
                button2.Visibility = Visibility.Collapsed;
                //Adds the cloned column to layer 0
                layer0.ColumnDefinitions.Add(column2CloneForLayer0);
                //Adds the cloned column to layer 1, only if pane 1 is docked
                if (button1.Visibility == Visibility.Collapsed)
                    layer1.ColumnDefinitions.Add(column2CloneForLayer1);

            }
        }

        public void UndockPane(int paneNumber) 
        {
            if (paneNumber == 1) 
            {
                layer1.Visibility = Visibility.Visible;
                button1.Visibility = Visibility.Visible;
                // Removed the cloned columns
                layer0.ColumnDefinitions.Remove(column1CloneForLayer0);
                layer1.ColumnDefinitions.Remove(column2CloneForLayer1);
            }
            else if (paneNumber == 2) 
            {
                layer2.Visibility = Visibility.Visible;
                button2.Visibility = Visibility.Visible;

                // Removed the cloned columns
                layer0.ColumnDefinitions.Remove(column2CloneForLayer0);
                layer1.ColumnDefinitions.Remove(column2CloneForLayer1);
            }
        }
    }
}
