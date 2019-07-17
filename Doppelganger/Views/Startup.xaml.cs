using Doppelganger.Models;
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
using System.Windows.Shapes;

namespace Doppelganger.Views
{
    /// <summary>
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Window
    {
        ObservableCollection<Macro> Items = new ObservableCollection<Macro>(); 
        public Startup()
        {
            InitializeComponent();
            this.DataContext = App.macroViewModel;
            //this.DataContext = this;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_Selected(object sender, RoutedEventArgs e)
        {
            Macro macro = aa.SelectedItem as Macro;
            macro.KeyboardInputs.ForEach(x =>
            {
                Console.WriteLine(x.Key);
            });
        }

        private void ListView_Selected(object sender, SelectionChangedEventArgs e)
        {
            App.macroViewModel.ExcuteInputs((Macro)aa.SelectedItem);
            //App.macroViewModel.ExcuteKeyboardInputs(((Macro)aa.SelectedItem).KeyboardInputs.ToList());
        }
    }
}
