using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TowerOfHanoi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        const int blocks = 8;
        private Random rand = new Random();
        public static int selectTop { get; set; }
        public static bool selected = false;
        public static int rectangleMargin { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                rectangleMargin = ((int)Left.ActualWidth - (blocks * 20)) / 2;
                for (int i = blocks; i > 0; i--)
                {
                    AddBlock(i, 1);
                }
            };

        }

        void rectangle_MouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            if (!selected)
            {
                selected = true;
                Canvas parent = VisualTreeHelper.GetParent(sender as DependencyObject) as Canvas;
                parent.Children.Remove(sender as Rectangle);
                Rectangle rect = sender as Rectangle;
                selectTop = (int)Canvas.GetTop(rect);
                Canvas.SetTop(rect, 0);
                Select.Children.Add(rect);
                SolidColorBrush brush = new SolidColorBrush(Colors.Pink) { Opacity = 0.5 };
                Left.Background = brush;
                Middle.Background = brush;
                Right.Background = brush;
                parent.ClearValue(BackgroundProperty);
            }
        }

        private void AddBlock(int size, int pos)
        {
            Rectangle rect = new Rectangle();
            rect.Width = size * 20;
            rect.Height = 25;
            Canvas.SetLeft(rect, ((blocks - size) * 10 * pos) + rectangleMargin);
            Canvas.SetTop(rect, size * 25);
            rect.Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(256), (byte)rand.Next(256), (byte)rand.Next(256)));
            rect.MouseLeftButtonDown += rectangle_MouseLeftDown;
            switch(pos)
            {
                case 1:
                    Left.Children.Add(rect);
                    break;
                case 2:
                    Middle.Children.Add(rect);
                    break;
                case 3:
                    Right.Children.Add(rect);
                    break;
            }
        }

        private void ClearBlocks()
        {
            Left.Children.Clear();
            Middle.Children.Clear();
            Right.Children.Clear();
        }
    }
}
