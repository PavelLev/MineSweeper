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
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for OptionsWindow.xaml
    /// </summary>
    public partial class OptionsWindow : Window
    {
        public OptionsWindow()
        {
            InitializeComponent();
            var options = MainWindow.Options;
            if (options.Height == 9 && options.Width == 9 && options.Mines == 10)
            {
                RadioButtonBeginner.IsChecked = true;
            }
            else if (options.Height == 16 && options.Width == 16 && options.Mines == 40)
            {
                RadioButtonIntermediate.IsChecked = true;
            }
            else if (options.Height == 16 && options.Width == 30 && options.Mines == 40)
            {
                RadioButtonIntermediate.IsChecked = true;
            }
            else
            {
                RadioButtonCustom.IsChecked = true;
            }
            TextBoxHeight.Text = options.Height.ToString();
            TextBoxWidth.Text = options.Width.ToString();
            TextBoxMines.Text = options.Mines.ToString();
        }

        private void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            if (RadioButtonCustom.IsChecked ?? false)
            {
                var options = MainWindow.Options;
                if (!int.TryParse(TextBoxHeight.Text, out options.Height) || options.Height < 9 || options.Height > 24)
                {
                    TextBoxHeight.BorderBrush = Brushes.Red;
                    return;
                }
                if (!int.TryParse(TextBoxWidth.Text, out options.Width) || options.Width < 9 || options.Width > 30)
                {
                    TextBoxWidth.BorderBrush = Brushes.Red;
                    return;
                }
                if (!int.TryParse(TextBoxMines.Text, out options.Mines) || options.Mines < 10 || options.Mines > 668 || options.Mines > options.Width*options.Height)
                {
                    TextBoxMines.BorderBrush = Brushes.Red;
                    return;
                }
                MainWindow.Options = options;
            }
            else if (RadioButtonBeginner.IsChecked ?? false)
            {
                MainWindow.Options = new Options
                {
                    Height = 9,
                    Width = 9,
                    Mines = 10
                };
            }
            else if (RadioButtonIntermediate.IsChecked ?? false)
            {
                MainWindow.Options = new Options
                {
                    Height = 16,
                    Width = 16,
                    Mines = 40
                };
            }
            else if (RadioButtonAdvanced.IsChecked ?? false)
            {
                MainWindow.Options = new Options
                {
                    Height = 16,
                    Width = 30,
                    Mines = 99
                };
            }
            MainWindow.CurrentWindow.NewGame();
            Close();
        }

        private void ButtonCancelClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.BorderBrush = Brushes.DarkGray;
            }
        }
    }
}
