using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace TowerOfHanoi
{
    public partial class MainWindow : Window
    {
        public static int blocks = 8;
        private readonly Random rand = new Random();
        public static bool selected = false;
        public MainWindow()
        {
            InitializeComponent();
            StartGame();
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

        private new void MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            if (!selected && canvas.Children.Count > 0)
            {
                selected = true;
                Rectangle rect = canvas.Children[canvas.Children.Count - 1] as Rectangle;
                canvas.Children.Remove(rect);
                AddBlock(Select, rect, (int)rect.Tag, rect.Width, rect.Height);
            }
            else if (selected)
            {
                Rectangle rect = Select.Children[0] as Rectangle;
                if (ValidPosition(canvas, rect))
                {
                    Select.Children.RemoveAt(0);
                    AddBlock(canvas, rect, (int)rect.Tag, CalculateWidth(canvas), CalculateHeight(canvas));
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

        private void AddBlock(Canvas canvas, Rectangle rectangle, int size, double width, double height)
        {
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
                Canvas.SetTop(rectangle, (Select.ActualHeight - height) / 2);
                Canvas.SetLeft(rectangle, (Select.ActualWidth - width) / 2);
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
            this.Title = "Tower Of Hanoi - Minimum Moves: " + (Math.Pow(2, blocks) - 1);
            Solve.IsEnabled = true;
            Increase.IsEnabled = true;
            Decrease.IsEnabled = true;
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
            int width = CalculateWidth(Left);
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
                AddBlock(canvas, rectangle, (int)rectangle.Tag, width, height);
            }
        }

        private async void Solve_Click(object sender, RoutedEventArgs e)
        {
            Solve.IsEnabled = false;
            Increase.IsEnabled= false;
            Decrease.IsEnabled= false;
            while(Right.Children.Count != blocks)
            {
                if (blocks % 2 == 0)
                {
                    await MakeMove(Left, Middle);
                    await MakeMove(Left, Right);
                }
                else
                {
                    await MakeMove(Left, Right);
                    await MakeMove(Left, Middle);
                }
                await MakeMove(Middle, Right);
            }
            CheckWin();
        }

        public async Task MakeMove(Canvas oldCanvas, Canvas newCanvas)
        {
            Rectangle rectangle;
            Canvas fromCanvas;
            Canvas toCanvas;
            if(oldCanvas.Children.Count > 0 && ValidPosition(newCanvas, (oldCanvas.Children[oldCanvas.Children.Count - 1] as Rectangle)))
            {
                fromCanvas = oldCanvas;
                toCanvas = newCanvas;
            }
            else
            {
                fromCanvas = newCanvas;
                toCanvas = oldCanvas;
            }
            rectangle = fromCanvas.Children[fromCanvas.Children.Count - 1] as Rectangle;
            fromCanvas.Children.Remove(rectangle);
            AddBlock(toCanvas, rectangle, (int)rectangle.Tag, CalculateWidth(toCanvas), CalculateHeight(toCanvas));
            await Task.Delay(100);
        }
    }
}
