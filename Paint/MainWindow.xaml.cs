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

namespace Paint
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool isDrawing;
        private int currentTool = 0; // 0 - Pencil, 1 - Rubber, 2 - Drip
        private int size = 1;
        private Brush color;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            isDrawing = true;

            if (currentTool == 0)
            {
                Draw(e.GetPosition(CnvPaint));
            }

            if (currentTool == 1)
            {
                Erase(e.GetPosition(CnvPaint));
            }
        }

        private void Canvas_MouseUp(object sender, MouseButtonEventArgs e)
        {
            isDrawing = false;
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing)
            {
                if (currentTool == 0)
                {
                    Draw(e.GetPosition(CnvPaint));
                }

                if (currentTool == 1)
                {
                    Erase(e.GetPosition(CnvPaint));
                }
            }
        }

        private void Draw(Point position)
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Fill = color;
            ellipse.Width = size;
            ellipse.Height = size;

            Canvas.SetTop(ellipse, position.Y - ellipse.Height / 2);
            Canvas.SetLeft(ellipse, position.X - ellipse.Width / 2);

            CnvPaint.Children.Add(ellipse);
        }

        private void Erase(Point position)
        {
            Rect erase = new Rect();
            erase.Width = size;
            erase.Height = size;

            Point place = new Point(
                position.X - erase.Width / 2, 
                position.Y - erase.Height / 2
            );
            erase.Location = place;

            for (int i = 0; i < CnvPaint.Children.Count; i++)
            {
                Rect point = new Rect();
                point.Size = CnvPaint.Children[i].RenderSize;
                point.Location = new Point(
                    Canvas.GetLeft(CnvPaint.Children[i]),
                    Canvas.GetTop(CnvPaint.Children[i])
                );

                if (erase.IntersectsWith(point))
                {
                    CnvPaint.Children.RemoveAt(i);
                    i--;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BtnPencil.Background = Brushes.LightGray;
            BtnRubber.Background = Brushes.LightGray;
            BtnDrip.Background = Brushes.LightGray;

            Button button = (Button)sender;
            button.Background = Brushes.Yellow;

            if (button == BtnPencil)
            {
                SliderSize.Visibility = Visibility.Visible;
                ColorPicker.Visibility = Visibility.Visible;
                currentTool = 0;
            }

            if (button == BtnRubber)
            {
                SliderSize.Visibility = Visibility.Visible;
                ColorPicker.Visibility = Visibility.Hidden;
                currentTool = 1;
            }

            if (button == BtnDrip)
            {
                SliderSize.Visibility = Visibility.Hidden;
                ColorPicker.Visibility = Visibility.Visible;
                currentTool = 2;
            }
        }

        private void SliderSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            size = (int)SliderSize.Value;
        }

        private void ColorPicker_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ColorPicker.SelectedIndex)
            {
                case 0:
                    color = Brushes.Black;
                    break;
                case 1:
                    color = Brushes.Red;
                    break;
                case 2:
                    color = Brushes.Green;
                    break;
                case 3:
                    color = Brushes.Blue;
                    break;
                default:
                    color = Brushes.Black;
                    break;
            }
        }
    }
}
