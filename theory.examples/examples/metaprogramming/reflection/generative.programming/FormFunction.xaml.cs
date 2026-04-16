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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.CodeAnalysis.CSharp.Scripting;

namespace generativeprogramming
{
    /// <summary>
    /// Interaction logic for FormFunction.xaml
    /// </summary>
    public partial class FormFunction : Window
    {
        public FormFunction()
        {
            InitializeComponent();
        }

        public double From { get; private set; }
        public double To { get; private set; }
        public double Increment { get; private set; }
        public string FunctionBody { get; private set; }

        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            this.From = Convert.ToDouble(this.textBoxFrom.Text);
            this.To = Convert.ToDouble(this.textBoxTo.Text);
            this.Increment = Convert.ToDouble(this.textBoxIncrement.Text);
            this.FunctionBody = this.textBoxFunctionBody.Text;

            try
            {
                var chart = new FormGenerativeProgramming(FunctionBody, From, To, Increment);
                chart.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Fatal Error");
            }
            this.Close();
            
        }
    }
}
