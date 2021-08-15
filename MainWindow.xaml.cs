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
using System.Windows.Threading;
using System.Diagnostics;
using System.Media;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer(); // timer object
        private const int _speed = 3;
        private MediaPlayer crunch = new MediaPlayer();
        private int score = 0;
        bool _isAppleLoaded = false;
        private Image apple = new Image();
        enum MovingDirection
        {
            Left,
            Right,
            Up,
            Down,
            None
        }

        private MovingDirection direction = MovingDirection.None;

        public MainWindow()
        {
            Uri crunchUri = new Uri(@"C:\Users\olehb\Desktop\Programming\C#\Snake\sounds\crunch.mp3");
            crunch.Open(crunchUri);
            InitializeComponent();
            YieldApple();

            KeyDown += MyCanvas_KeyDown;

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += MoveSnake;
            timer.Tick += CheckIfLostByBorderCollision;
            timer.Tick += CheckIfAteApple;
            timer.Start();
            
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    direction = MovingDirection.Up;
                    RotateTransform rotateUp = new RotateTransform(180, SnakeHead.Width/2, SnakeHead.Height/2);
                    SnakeHead.RenderTransform = rotateUp;
                    break;

                case Key.Down:
                    direction = MovingDirection.Down;
                    RotateTransform rotateDown = new RotateTransform(0, SnakeHead.Width / 2, SnakeHead.Height / 2);
                    SnakeHead.RenderTransform = rotateDown;
                    break;

                case Key.Left:
                    direction = MovingDirection.Left;
                    RotateTransform rotateLeft = new RotateTransform(90, SnakeHead.Width / 2, SnakeHead.Height / 2);
                    SnakeHead.RenderTransform = rotateLeft;
                    break;

                case Key.Right:
                    direction = MovingDirection.Right;
                    RotateTransform rotateRight = new RotateTransform(270, SnakeHead.Width / 2, SnakeHead.Height / 2);
                    SnakeHead.RenderTransform = rotateRight;
                    break;

            }   
        }
        private void MoveSnake(object sender, EventArgs args)
        {

            switch (direction)
            {
                case MovingDirection.Up:
                    Canvas.SetTop(SnakeHead, Canvas.GetTop(SnakeHead) - _speed);
                    break;
                case MovingDirection.Down:
                    Canvas.SetTop(SnakeHead, Canvas.GetTop(SnakeHead) + _speed);
                    break;
                case MovingDirection.Left:
                    Canvas.SetLeft(SnakeHead, Canvas.GetLeft(SnakeHead) - _speed);
                    break;
                case MovingDirection.Right:
                    Canvas.SetLeft(SnakeHead, Canvas.GetLeft(SnakeHead) + _speed);
                    break;
            } 
        }

        private void CheckIfLostByBorderCollision(object sender, EventArgs args)
        {
            if (Canvas.GetLeft(SnakeHead) <= 0  || Canvas.GetLeft(SnakeHead) > MyCanvas.ActualWidth - SnakeHead.Width
             || Canvas.GetTop(SnakeHead) <= 0 || Canvas.GetTop(SnakeHead) > MyCanvas.ActualHeight - statusbar.Height - SnakeHead.Height)
            {
                GameLost();
            }
        }

        private void GameLost()
        {
            timer.Stop();
            MessageBoxResult result = MessageBox.Show("Would you like to try again", "You lost!", 
                MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                Process.Start(Process.GetCurrentProcess().MainModule.FileName);
                Application.Current.Shutdown();
            }
            else
            {
                Application.Current.Shutdown();
            }
        }

        private void YieldApple()
        {
            if (!_isAppleLoaded)
            {
                apple.Width = 30;
                apple.Height = 30;
                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"C:\Users\olehb\Desktop\Programming\C#\Snake\images\apple.png");

                myBitmapImage.DecodePixelWidth = 30;
                myBitmapImage.DecodePixelHeight = 30;
                myBitmapImage.EndInit();

                apple.Source = myBitmapImage;
                MyCanvas.Children.Add(apple);
                _isAppleLoaded = true;
            }
            Random r = new Random();
            Canvas.SetLeft(apple, r.Next(0, (int)(MyCanvas.Width - apple.Width*2)));
            Canvas.SetTop(apple, r.Next(0, (int)(MyCanvas.Height - statusbar.Height - apple.Height*2)));
            
        }

        private void CheckIfAteApple(object sender, EventArgs args)
        {
            if(Math.Abs(Canvas.GetLeft(SnakeHead) - Canvas.GetLeft(apple)) <= 30 &&
               Math.Abs(Canvas.GetTop(SnakeHead) - Canvas.GetTop(apple)) <= 30)
            {
                score++;
                Scoresheet.Content = score.ToString();
                crunch.Play();
                crunch.Position = TimeSpan.FromMilliseconds(0);
                
                YieldApple();
            }
        }
    }
}
