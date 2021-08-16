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

    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer(); // timer object
        
        private MediaPlayer crunch = new MediaPlayer();
        private int score = 0;
        bool _isAppleLoaded = false;
        private Image apple = new Image();
        private List<Rectangle> Snake = new List<Rectangle>();
        private const int _onePartOfBodyLen = 30;
        private const int _speed = _onePartOfBodyLen;
        enum MovingDirection
        {
            Left,
            Right,
            Up,
            Down,
            None
        }

        private MovingDirection direction = MovingDirection.None;
        private MovingDirection prevDirection = MovingDirection.None;

        public MainWindow()
        {

            Uri crunchUri = new Uri(@"C:\Users\olehb\Desktop\Programming\C#\Snake\sounds\crunch.mp3");
            crunch.Open(crunchUri);
            InitializeComponent();

            Snake.Add(SnakeHead);

            YieldApple();

            KeyDown += MyCanvas_KeyDown;

            timer.Interval = TimeSpan.FromMilliseconds(300);
            timer.Tick += MoveSnake;
            timer.Tick += CheckIfLostByBorderCollision;
            timer.Tick += CheckIfLostByBodyCollision;
            timer.Tick += CheckIfAteApple;
            
            timer.Start();
            
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
                {
                    //  Uncomment rotation code if specificated images instead of rectangles as part of snake's body.

                    case Key.Up:
                        direction = MovingDirection.Up;
                        /*RotateTransform rotateUp = new RotateTransform(180, SnakeHead.Width/2, SnakeHead.Height/2);
                        part.RenderTransform = rotateUp;*/
                        break;

                    case Key.Down:
                        direction = MovingDirection.Down;
                        /*RotateTransform rotateDown = new RotateTransform(0, SnakeHead.Width / 2, SnakeHead.Height / 2);
                        part.RenderTransform = rotateDown;*/
                        break;

                    case Key.Left:
                        direction = MovingDirection.Left;
                        /*RotateTransform rotateLeft = new RotateTransform(90, SnakeHead.Width / 2, SnakeHead.Height / 2);
                        part.RenderTransform = rotateLeft;*/
                        break;

                    case Key.Right:
                        direction = MovingDirection.Right;
                        /*RotateTransform rotateRight = new RotateTransform(270, SnakeHead.Width / 2, SnakeHead.Height / 2);
                        part.RenderTransform = rotateRight;*/
                        break;

                }   
            
            
        }
        private void MoveSnake(object sender, EventArgs args)
        {
            
            double prevTop = Canvas.GetTop(SnakeHead);
            double prevLeft = Canvas.GetLeft(SnakeHead);


            switch (direction)
            {
                case MovingDirection.Up:
                    if (prevDirection != MovingDirection.Down)      //  To not to die by changing moving direction to opposite
                    {
                        Canvas.SetTop(SnakeHead, Canvas.GetTop(SnakeHead) - _speed);
                        prevDirection = direction;
                    }
                    else {
                        Canvas.SetTop(SnakeHead, Canvas.GetTop(SnakeHead) + _speed);
                        direction = prevDirection; 
                    }
                    break;

                case MovingDirection.Down:
                    if (prevDirection != MovingDirection.Up)
                    {
                        Canvas.SetTop(SnakeHead, Canvas.GetTop(SnakeHead) + _speed);
                        prevDirection = direction;
                    }
                    else
                    {
                        Canvas.SetTop(SnakeHead, Canvas.GetTop(SnakeHead) - _speed);
                        direction = prevDirection;
                    }
                    break;

                case MovingDirection.Left:
                    if (prevDirection != MovingDirection.Right)
                    {
                        Canvas.SetLeft(SnakeHead, Canvas.GetLeft(SnakeHead) - _speed);
                        prevDirection = direction;
                    }
                    else
                    {
                        Canvas.SetLeft(SnakeHead, Canvas.GetLeft(SnakeHead) + _speed);
                        direction = prevDirection;
                    }
                    break;

                case MovingDirection.Right:
                    if (prevDirection != MovingDirection.Left)
                    {
                        Canvas.SetLeft(SnakeHead, Canvas.GetLeft(SnakeHead) + _speed);
                        prevDirection = direction;
                    }
                    else
                    {
                        Canvas.SetLeft(SnakeHead, Canvas.GetLeft(SnakeHead) - _speed);
                        direction = prevDirection;
                    }
                    break;
            }

            for(int i = 1; i < Snake.Count; i++)        //  Move the whole body in "snake" way
            {
                double tmpTop = Canvas.GetTop(Snake[i]);
                double tmpLeft = Canvas.GetLeft(Snake[i]);

                Canvas.SetTop(Snake[i], prevTop);
                Canvas.SetLeft(Snake[i], prevLeft);

                prevTop = tmpTop;
                prevLeft = tmpLeft;

            }

        }

        private void CheckIfLostByBorderCollision(object sender, EventArgs args)
        {
            if (Canvas.GetLeft(SnakeHead) < 0  || Canvas.GetLeft(SnakeHead) > MyCanvas.ActualWidth - SnakeHead.Width
             || Canvas.GetTop(SnakeHead) < 0 || Canvas.GetTop(SnakeHead) > MyCanvas.ActualHeight - statusbar.Height - SnakeHead.Height)
            {
                GameLost();
            }
        }

        private void GameLost()
        {
            timer.Stop();
            MessageBoxResult result = MessageBox.Show("Would you like to try again?", "You lost!", 
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
                apple.Width = _onePartOfBodyLen;
                apple.Height = _onePartOfBodyLen;
                BitmapImage bitmapApple = new BitmapImage();
                bitmapApple.BeginInit();
                bitmapApple.UriSource = new Uri(@"C:\Users\olehb\Desktop\Programming\C#\Snake\images\apple.png");

                bitmapApple.DecodePixelWidth = _onePartOfBodyLen;
                bitmapApple.DecodePixelHeight = _onePartOfBodyLen;
                bitmapApple.EndInit();

                apple.Source = bitmapApple;
                MyCanvas.Children.Add(apple);
                _isAppleLoaded = true;
            }
            Random r = new Random();
            Canvas.SetLeft(apple, r.Next(0, (int)Math.Floor(MyCanvas.Width/_onePartOfBodyLen)*_onePartOfBodyLen));
            Canvas.SetTop(apple, r.Next(0, (int)Math.Floor(MyCanvas.Height-statusbar.Height) / _onePartOfBodyLen) * _onePartOfBodyLen);
            
        }

        private void CheckIfAteApple(object sender, EventArgs args)
        {
            if(Math.Abs(Canvas.GetLeft(SnakeHead) - Canvas.GetLeft(apple)) < _onePartOfBodyLen &&
               Math.Abs(Canvas.GetTop(SnakeHead) - Canvas.GetTop(apple)) < _onePartOfBodyLen)
            {
                AppleEaten();
            }
        }

        private void AppleEaten()
        {
            score++;
            Scoresheet.Content = score.ToString();
            crunch.Play();
            crunch.Position = TimeSpan.FromMilliseconds(0);
            Grow();
            YieldApple();
        }

        private void Grow()
        {
            Rectangle body = new Rectangle();
            body.Width = _onePartOfBodyLen;
            body.Height = _onePartOfBodyLen;

            body.Fill = new SolidColorBrush(Colors.Black);
            body.Stroke = new SolidColorBrush(Colors.White);

            MyCanvas.Children.Add(body);
            Canvas.SetLeft(body, Canvas.GetLeft(Snake.Last()));
            Canvas.SetTop(body, Canvas.GetTop(Snake.Last()));

            Snake.Add(body);
        }

        private void CheckIfLostByBodyCollision(object sender, EventArgs args)
        {
            var headTopPos = Canvas.GetTop(SnakeHead);
            var headLeftPos = Canvas.GetLeft(SnakeHead);
            for (int i = 1; i < Snake.Count; i++)
            {
                if(Math.Abs(headTopPos-Canvas.GetTop(Snake[i]))<=_onePartOfBodyLen/2 && Math.Abs(headLeftPos - Canvas.GetLeft(Snake[i])) <= _onePartOfBodyLen / 2)
                {
                    GameLost();
                }
            }
        }
    }
}
