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

namespace envset.Views
{
    /// <summary>
    /// EditEnvWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class EditEnvWindow : Window
    {
        public EditEnvWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModels.EditEnvWindowViewModel).TargetEnvItem = Tag as Models.EnvItem;
        }
    }
}