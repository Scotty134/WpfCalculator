using Calculation.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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

namespace Calculation.View
{
    /// <summary>
    /// Interaction logic for CalcView.xaml
    /// </summary>
    public partial class CalcView : MetroWindow
    {
        public CalcView()
        {
            InitializeComponent();
            this.DataContext = new CalcViewModel();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(Data.Text.Length > 24)
                Data.FontSize = 9;
            else if (Data.Text.Length > 12)
            {
                Data.FontSize = 12;
            }
            else
            {
                Data.FontSize = 18;
            }
        }
    }
}
