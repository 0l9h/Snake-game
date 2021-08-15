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
using System.Windows.Media.Animation;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer = new DispatcherTimer(); // timer object
        private const int _speed = 3;
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
            InitializeComponent();
            KeyDown += MyCanvas_KeyDown;

            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += MoveSnake;
            timer.Start();
            
        }

        private void MyCanvas_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    direction = MovingDirection.Up;
                    break;

                case Key.Down:
                    direction = MovingDirection.Down;
                    break;

                case Key.Left:
                    direction = MovingDirection.Left;
                    break;

                case Key.Right:
                    direction = MovingDirection.Right;
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
    }
}
