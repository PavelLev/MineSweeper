using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly Random Random = new Random();
        private int _rightPresses;

        public int RightPresses
        {
            get { return _rightPresses; }
            set
            {
                _rightPresses = value;
                if (_rightPresses == Options.Width*Options.Height - Options.Mines)
                {
                    Won();
                }
            }
        }
        public static bool IsFirst { get; set; }

        public static MainWindow CurrentWindow { get; set; }
        public static Options Options { get; set; } = new Options
        {
            Height = 9,
            Width = 9,
            Mines = 10
        };

        public MainWindow()
        {
            InitializeComponent();
            CurrentWindow = this;
            NewGame();
        }

        
        public void Lost()
        {
            foreach (Tile tile in MinesWrapPanel.Children)
            {
                if (!tile.IsMine)
                    continue;
                tile.FrontImage.Visibility = Visibility.Hidden;
            }
            MessageBox.Show("You lost", "", MessageBoxButton.OK);
            NewGame();
        }

        public void Won()
        {
            MessageBox.Show("You won", "", MessageBoxButton.OK);
            NewGame();
        }

        public void PlaceMines(int index)
        {
            for (var i = 0; i < Options.Mines;)
            {
                var newIndex = Random.Next(0, Options.Width * Options.Height);
                var tile = MinesWrapPanel.Children[newIndex] as Tile;
                if (newIndex == index || tile == null || tile.IsMine)
                    continue;
                tile.BecomeMine();
                i++;
            }

            foreach (Tile tile in MinesWrapPanel.Children)
            {
                if (tile.IsMine)
                    continue;
                tile.BecomeBlank();
            }
        }

        public void NewGame()
        {
            RightPresses = 0;
            IsFirst = true;

            var tiles = new List<Tile>();

            for (var i = 0; i < Options.Width * Options.Height; i++)
            {
                tiles.Add(new Tile());
            }

            //Set pointers to adjacent tiles
            //top left
            tiles[0].AdjacentTiles = new List<Tile>
            {
                tiles[1],
                tiles[Options.Width],
                tiles[Options.Width + 1]
            };

            //top right
            tiles[Options.Width - 1].AdjacentTiles = new List<Tile>
            {
                tiles[Options.Width - 2],
                tiles[2 * Options.Width - 1],
                tiles[2 * Options.Width - 2]
            };

            //bottom left
            tiles[(Options.Height - 1) * Options.Width].AdjacentTiles = new List<Tile>
            {
                tiles[(Options.Height - 1) * Options.Width + 1],
                tiles[(Options.Height - 2) * Options.Width],
                tiles[(Options.Height - 2) * Options.Width + 1]
            };

            //bottom right
            tiles[Options.Height * Options.Width - 1].AdjacentTiles = new List<Tile>
            {
                tiles[Options.Height * Options.Width - 2],
                tiles[(Options.Height - 1) * Options.Width - 1],
                tiles[(Options.Height - 1) * Options.Width - 2]
            };

            //top
            var deltas = new[] {-1, 1, Options.Width - 1, Options.Width, Options.Width + 1};
            for (var index = 1; index < Options.Width - 1; index++)
            {
                tiles[index].AdjacentTiles = new List<Tile>(5);
                foreach (var delta in deltas)
                {
                    tiles[index].AdjacentTiles.Add(tiles[index + delta]);
                }
            }

            //left
            deltas = new[] { -Options.Width, -Options.Width + 1, 1, Options.Width, Options.Width + 1 };
            for (var index = Options.Width; index < (Options.Height - 1) * Options.Width; index += Options.Width)
            {
                tiles[index].AdjacentTiles = new List<Tile>(5);
                foreach (var delta in deltas)
                {
                    tiles[index].AdjacentTiles.Add(tiles[index + delta]);
                }
            }

            //bottom
            deltas = new[] { -Options.Width - 1, -Options.Width, -Options.Width + 1, -1, 1 };
            for (var index = (Options.Height - 1) * Options.Width + 1; index < Options.Height * Options.Width - 1; index++)
            {
                tiles[index].AdjacentTiles = new List<Tile>(5);
                foreach (var delta in deltas)
                {
                    tiles[index].AdjacentTiles.Add(tiles[index + delta]);
                }
            }

            //right
            deltas = new[] { -Options.Width - 1, -Options.Width, -1, Options.Width - 1, Options.Width };
            for (var index = 2 * Options.Width - 1; index < Options.Height * Options.Width - 1; index += Options.Width)
            {
                tiles[index].AdjacentTiles = new List<Tile>(5);
                foreach (var delta in deltas)
                {
                    tiles[index].AdjacentTiles.Add(tiles[index + delta]);
                }
            }

            //middle
            deltas = new[]
            {
                -Options.Width - 1, -Options.Width, -Options.Width + 1, -1, 1, Options.Width - 1, Options.Width,
                Options.Width + 1
            };
            for (var j = 1; j < Options.Height - 1; j++)
            {
                for (var i = 1; i < Options.Width - 1; i++)
                {
                    var index = j * Options.Width + i;
                    tiles[index].AdjacentTiles = new List<Tile>(8);
                    foreach (var delta in deltas)
                    {
                        tiles[index].AdjacentTiles.Add(tiles[index + delta]);
                    }
                }
            }

            Width = Options.Width * 28 + 30;
            Height = Options.Height * 28 + 70;

            MinesWrapPanel.Children.Clear();
            tiles.ForEach((tile) =>
            {
                MinesWrapPanel.Children.Add(tile);
            });
        }

        private void RestartClick(object sender, RoutedEventArgs e)
        {
            NewGame();
        }

        private void OptionsClick(object sender, RoutedEventArgs e)
        {
            (new OptionsWindow()).ShowDialog();
        }
    }
}
