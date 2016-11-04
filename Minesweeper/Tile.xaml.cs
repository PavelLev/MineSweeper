using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
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

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for Tile.xaml
    /// </summary>
    public partial class Tile : UserControl
    {
        public int AdjacentMines { get; private set; }
        public bool IsMine { get; private set; }
        public bool IsMarked { get; private set; }
        public bool IsPressed { get; private set; }
        public List<Tile> AdjacentTiles;
        public Tile()
        {
            InitializeComponent();
            FrontImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/blank.png"));
            Button.PreviewMouseDown += FirstClick;
            Button.MouseRightButtonUp += (sender, args) =>
            {
                if (IsPressed)
                    return;
                IsMarked = !IsMarked;
                FrontImage.Source =
                    new BitmapImage(
                        new Uri("pack://application:,,,/Images/" + (IsMarked ? "flag" : "blank") + ".png"));
            };
        }

        private void FirstClick(object sender, MouseButtonEventArgs args)
        {
            if (!MainWindow.IsFirst)
                return;
            MainWindow.CurrentWindow.PlaceMines(MainWindow.CurrentWindow.MinesWrapPanel.Children.IndexOf(this));
            MainWindow.IsFirst = false;
        }

        public void BecomeMine()
        {
            BackImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/mine.png"));
            IsMine = true;
            Button.PreviewMouseLeftButtonUp += (sender, args) =>
            {
                if (IsMarked)
                    return;
                MainWindow.CurrentWindow.Lost();
            };
        }

        public void BecomeBlank()
        {
            AdjacentMines = AdjacentTiles.Count((tile) => tile.IsMine);
            BackImage.Source = new BitmapImage(new Uri("pack://application:,,,/Images/" + AdjacentMines + ".png"));
            Button.PreviewMouseLeftButtonUp += (sender, args) =>
            {
                if (IsMarked || IsPressed)
                    return;
                FrontImage.Visibility = Visibility.Hidden;
                IsPressed = true;
                MainWindow.CurrentWindow.RightPresses++;
                if (AdjacentMines != 0)
                    return;
                foreach (var tile in AdjacentTiles)
                {
                    if (tile.IsPressed)
                        continue;
                    tile.Button.RaiseEvent(new MouseButtonEventArgs(args.MouseDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.PreviewMouseUpEvent,
                        Source = tile
                    });
                }
            };
            Button.PreviewMouseDoubleClick += (sender, args) =>
            {
                if (args.LeftButton == MouseButtonState.Released || AdjacentTiles.Count((tile) => tile.IsMarked) != AdjacentMines)
                    return;
                foreach (var tile in AdjacentTiles)
                {
                    if (tile.IsPressed)
                        continue;
                    tile.Button.RaiseEvent(new MouseButtonEventArgs(args.MouseDevice, 0, MouseButton.Left)
                    {
                        RoutedEvent = Mouse.PreviewMouseUpEvent,
                        Source = tile
                    });
                }
                
            };
        }
    }
}
