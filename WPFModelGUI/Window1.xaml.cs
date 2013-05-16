using System;
using System.Collections.Generic;
//using System.Linq;
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
using SimpleModel;
using System.Diagnostics;

namespace WpfTrainingApp
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        private User _user = new User();
        
        public Window1()
        {
            _user.Age.Value = 42;
            _user.Name.Value = "Fred";
            _user.InitAttributes();

            
            InitializeComponent();

            button1.Click += button1_Click;
            btnSave.Click += btnSave_Click;


            this.DataContext = _user;            
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            _user.InitAttributes();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            _user.Name.Value = "Chris";
            _user.Age.Value = 38;           
        }
    }
}
