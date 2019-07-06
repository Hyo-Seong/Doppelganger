using Doppelganger.Models;
using Doppelganger.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Doppelganger
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Macro macro = new Macro();
        private UserActivityHook _hook;
        public MainWindow()
        {
            InitializeComponent();
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _hook = new UserActivityHook(true, true);
            _hook.KeyDown += _hook_KeyDown;
        }

        void _hook_KeyDown(object sender, CustomKeyEventArgs e)
        {
            macro.InputValues.Add(new KeyboardInput
            {
                InputType = InputType.Keyboard,
                Key = e.Key,
                KeyStatus = KeyStatus.Down,
                Millis = DateTime.Now.Millisecond
            });
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _hook.KeyDown -= _hook_KeyDown;
        }
    }
}
