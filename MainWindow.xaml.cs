using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TowerOfHanoi
{
    public partial class MainWindow : Window
    {
        const int blocks = 8;
        private readonly Random rand = new Random();
        public static bool selected = false;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += delegate
            {
                for (int i = blocks; i > 0; i--)
                {
                    AddBlock(i, Left);
                }
            };

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = sender == RButton ? Right : (sender == MButton ? Middle : Left);
            if (!selected && canvas.Children.Count > 0)
            {
                selected = true;
                Rectangle rect = canvas.Children[canvas.Children.Count - 1] as Rectangle;
                canvas.Children.Remove(rect);
                Canvas.SetTop(rect, (Select.ActualHeight - rect.Height) / 2);
                Canvas.SetLeft(rect, (Select.ActualWidth - rect.Width) / 2);
                Select.Children.Add(rect);
            }
            else if (selected)
            {
                Rectangle rect = Select.Children[0] as Rectangle;
                if (canvas.Children.Count == 0 || (canvas.Children[canvas.Children.Count - 1] as Rectangle).Width > rect.Width)
                {
                    Select.Children.RemoveAt(0);
                    Canvas.SetTop(rect, (blocks - canvas.Children.Count) * 25);
                    Canvas.SetLeft(rect, ((blocks - (rect.Width / 20)) * 10) + (int)(canvas.ActualWidth - (blocks * 20)) / 2);
                    canvas.Children.Add(rect);
                    selected = false;
                    CheckWin();
                }
            }
        }

        private void CheckWin()
        {
            if(Right.Children.Count == blocks)
            {
                MessageBox.Show("You Win!");
                Right.Children.Clear();
                for (int i = blocks; i > 0; i--)
                {
                    AddBlock(i, Left);
                }
            }
        }

        private void AddBlock(int size, Canvas canvas)
        {
            Rectangle rect = new Rectangle
            {
                Width = size * 20,
                Height = 25,
                Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255)))
            };
            Canvas.SetLeft(rect, ((blocks - size) * 10) + (int)(canvas.ActualWidth - (blocks * 20)) / 2);
            Canvas.SetTop(rect, size * 25);
            canvas.Children.Add(rect);
        }
    }
}
