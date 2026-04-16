using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace generativeprogramming
{
    /// <summary>
    /// Interaction logic for FormGenerativeProgramming.xaml
    /// </summary>
    public partial class FormGenerativeProgramming : Window
    {


        public FormGenerativeProgramming()
        {
            InitializeComponent();
        }

        private double fromX, toX, incrementX;
        private string function;

        public FormGenerativeProgramming(string function, double from, double to, double increment) : this()
        {
            this.function = function;
            this.fromX = from;
            this.toX = to;
            this.incrementX = increment;


            double[] xs, ys;
            WorkOutValues(out xs, out ys);
            double maxX = xs.Max(), maxY = ys.Max(),
                minX = xs.Min(), minY = ys.Min();
            int widthPixels = (int)this.Width,
                heightPixels = (int)this.Height;
            DrawAxes(minX, maxX, minY, maxY, widthPixels, heightPixels);

            DrawChart(xs, ys, minX, maxX, minY, maxY, widthPixels, heightPixels);

        }

        private void DrawChart(double[] xs, double[] ys, double minX, double maxX, double minY, double maxY, int widthPixels, int heightPixels)
        {
            Polyline func = new Polyline();
            func.Stroke = Brushes.Blue;
            func.StrokeThickness = 1;

            for (int i = 0; i < xs.Length - 1; i++)
                func.Points.Add(new Point(TranslateX(xs[i], minX, maxX, widthPixels), TranslateY(ys[i], minY, maxY, heightPixels)));
            func.Points.Add(new Point(TranslateX(xs[xs.Length - 1], minX, maxX, widthPixels), TranslateY(ys[xs.Length - 1], minY, maxY, heightPixels)));
            this.paint.Children.Add(func);
        }

        private void WorkOutValues(out double[] xs, out double[] ys)
        {
            
            xs = new double[(int)((toX - fromX) / incrementX) + 1];
            ys = new double[(int)((toX - fromX) / incrementX) + 1];
            int i = 0;

            for (double localX = fromX; localX <= toX; localX = localX + incrementX, i++)
            {
                xs[i] = localX;

                using (var task = CSharpScript.EvaluateAsync<double>(function, 
                    options: ScriptOptions.Default.AddImports("System"),
                    globals: new ScriptGlobals() { x = localX }))
                {
                    ys[i] = task.Result;
                }
            }
        }

        private int TranslateX(double x, double minX, double maxX, int widthPixels)
        {
            return (int)(widthPixels / (maxX - minX) * (x - minX));
        }

        private int TranslateY(double y, double minY, double maxY, int heightPixels)
        {
            return heightPixels - (int)(heightPixels / (maxY - minY) * (y - minY));
        }

        private void DrawAxes(double minX, double maxX, double minY, double maxY, int widthPixels, int heightPixels)
        {
            //Pen pen = new Pen(Color.Black, 2.0f);
            int centerX = TranslateX(0, minX, maxX, widthPixels);
            int centerY = TranslateY(0, minY, maxY, heightPixels);
            Polyline axisX, axisY;
            axisX = new Polyline();
            axisX.Points.Add(new Point(TranslateX(minX, minX, maxX, widthPixels), centerY));
            axisX.Points.Add(new Point(centerX, centerY));
            axisX.Points.Add(new Point(TranslateX(maxX, minX, maxX, widthPixels), centerY));
            axisX.Stroke = Brushes.Black;
            axisX.StrokeThickness = 1;
            axisY = new Polyline();
            axisY.Points.Add(new Point(centerX, TranslateY(minY, minY, maxY, heightPixels)));
            axisY.Points.Add(new Point(centerX, centerY));
            axisY.Points.Add(new Point(centerX, TranslateY(maxY, minY, maxY, heightPixels)));
            axisY.Stroke = Brushes.Black;
            axisY.StrokeThickness = 1;
            this.paint.Children.Add(axisX);
            this.paint.Children.Add(axisY);

            TextBlock textLabe1 = new TextBlock();
            textLabe1.Text = minX.ToString();
            Canvas.SetLeft(textLabe1, 0);
            Canvas.SetTop(textLabe1, centerY);
            this.paint.Children.Add(textLabe1);

            TextBlock textLabe2 = new TextBlock();
            textLabe2.Text = maxX.ToString();
            Canvas.SetLeft(textLabe2, widthPixels - 40);
            Canvas.SetTop(textLabe2, centerY);
            this.paint.Children.Add(textLabe2);

            TextBlock textLabe3 = new TextBlock();
            textLabe3.Text = maxY.ToString();
            Canvas.SetLeft(textLabe3, centerX);
            Canvas.SetTop(textLabe3, 0);
            this.paint.Children.Add(textLabe3);

            TextBlock textLabe4 = new TextBlock();
            textLabe4.Text = minY.ToString();
            Canvas.SetLeft(textLabe4, centerX);
            Canvas.SetTop(textLabe4, heightPixels - 15);
            this.paint.Children.Add(textLabe4);
        }
    }
}
