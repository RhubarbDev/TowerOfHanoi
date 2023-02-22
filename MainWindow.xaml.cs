using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace TowerOfHanoi
{
    public partial class MainWindow : Window
    {
        public static int blocks = 8;
        private readonly Random rand = new Random();
        public static bool selected = false;
        readonly DispatcherTimer resizeTimer = new DispatcherTimer { Interval = new TimeSpan(0, 0, 0, 0, 0), IsEnabled = false };
    public MainWindow()
        {
            resizeTimer.Tick += resizeTimer_Tick;
            InitializeComponent();
            Loaded += delegate
            {
                StartGame();
            };
        }

        private int CalculateWidth(Canvas canvas)
        {
            int width = (int)(canvas.ActualWidth / (blocks + 4));
            return width;
        }

        private int CalculateHeight(Canvas canvas)
        {
            int height = (int)(canvas.ActualHeight/ (blocks + 2));
            return height;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Canvas canvas = sender == RButton ? Right : (sender == MButton ? Middle : Left);
            if (!selected && canvas.Children.Count > 0)
            {
                selected = true;
                Rectangle rect = canvas.Children[canvas.Children.Count - 1] as Rectangle;
                canvas.Children.Remove(rect);
                AddBlock(Select, rect, null, rect.ActualWidth, rect.ActualHeight);
            }
            else if (selected)
            {
                Rectangle rect = Select.Children[0] as Rectangle;
                if (ValidPosition(canvas, rect))
                {
                    Select.Children.RemoveAt(0);
                    AddBlock(canvas, rect, null, CalculateWidth(canvas), CalculateHeight(canvas));
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
                StartGame();
            }
        }

        private void AddBlock(Canvas canvas, Rectangle rectangle, int? s, double width, double height)
        {
            int size;
            if (s == null)
            {
                size = (int)rectangle.Tag;
            }
            else
            {
                size = s.Value;
            }
            if (rectangle == null)
            {
                rectangle = new Rectangle
                {
                    Width = size * width,
                    Height = height,
                    Fill = new SolidColorBrush(Color.FromRgb((byte)rand.Next(255), (byte)rand.Next(255), (byte)rand.Next(255))),
                    Tag = size
                };
            }
            if (canvas == Select)
            {
                Canvas.SetTop(rectangle, (Select.ActualHeight - rectangle.Height) / 2);
                Canvas.SetLeft(rectangle, (Select.ActualWidth - rectangle.Width) / 2);
            }
            else
            {
                Canvas.SetTop(rectangle, (blocks - canvas.Children.Count) * height);
                Canvas.SetLeft(rectangle, ((blocks - size) * (width / 2)) + (int)(canvas.ActualWidth - (blocks * width)) / 2);
            }
            canvas.Children.Add(rectangle);
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            ResetBlocks();
            StartGame();
        }

        private bool ValidPosition(Canvas target, Rectangle rect)
        {
            if(target.Children.Count == 0 || (int)(target.Children[target.Children.Count - 1] as Rectangle).Tag > (int)rect.Tag)
            {
                return true;
            }
            return false;
        }

        private void ResetBlocks()
        {
            Left.Children.Clear();
            Middle.Children.Clear();
            Right.Children.Clear();
            Select.Children.Clear();
        }

        private void StartGame()
        {
            int width = CalculateWidth(Left);
            int height = CalculateHeight(Left);
            for (int i = blocks; i > 0; i--)
            {
                AddBlock(Left, null, i, width, height);
            }
        }

        private void Increase_Click(object sender, RoutedEventArgs e)
        {
            ResetBlocks();
            blocks++;
            StartGame();
        }

        private void Decrease_Click(object sender, RoutedEventArgs e)
        {
            if (blocks > 1)
            {
                ResetBlocks();
                blocks--;
                StartGame();
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            resizeTimer.IsEnabled = true;
            resizeTimer.Stop();
            resizeTimer.Start();
        }

        void resizeTimer_Tick(object sender, EventArgs e)
        {
            resizeTimer.IsEnabled = false;
            int width = CalculateWidth(Left); // Left Middle Right should always be the same
            int height = CalculateHeight(Left);
            ResizeBlocks(Left, width, height);
            ResizeBlocks(Middle, width, height);
            ResizeBlocks(Right, width, height);
            ResizeBlocks(Select, width, height);
        }

        void ResizeBlocks(Canvas canvas, int width, int height)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (Rectangle rectangle in canvas.Children)
            {
                rectangles.Add(rectangle);
            }
            canvas.Children.Clear();
            foreach(Rectangle rectangle in rectangles)
            {
                rectangle.Width = (int)rectangle.Tag * width;
                rectangle.Height = height;
                AddBlock(canvas, rectangle, null, width, height);
            }
        }
    }
}
