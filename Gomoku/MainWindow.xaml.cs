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

namespace Gomoku
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += Window_Loaded;
        }

        private const int GRID = 15;
        private const double CANVAS_SIZE = 460;
        private const double MARGIN = 20;           // 20px margin
        private const double StoneSize = 10;
        private readonly Brush GridColor = Brushes.SaddleBrown;
        private int counter = 0;
        private char[,] grid = new char[15, 15];

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stoneCanvas.Width = CANVAS_SIZE;
            stoneCanvas.Height = CANVAS_SIZE;

            FillGridCells();
            DrawGridLines();
            AddClickAreas();
        }

        private void FillGridCells()
        {
            double step = (CANVAS_SIZE - 2 * MARGIN) / (GRID - 1);

            for (int row = 0; row < GRID - 1; row++)
            {
                for (int col = 0; col < GRID - 1; col++)
                {
                    double x = MARGIN + col * step;
                    double y = MARGIN + row * step;

                    var cell = new Rectangle
                    {
                        Width = step,
                        Height = step,
                        Fill = Brushes.BurlyWood
                    };

                    Canvas.SetLeft(cell, x);
                    Canvas.SetTop(cell, y);
                    stoneCanvas.Children.Add(cell);
                }
            }
        }


        private void DrawGridLines()
        {
            double step = (CANVAS_SIZE - 2 * MARGIN) / (GRID - 1);

            // Vertical lines
            for (int i = 0; i < GRID; i++)
            {
                double x = MARGIN + i * step;
                var line = new Line
                {
                    X1 = x,
                    Y1 = MARGIN,
                    X2 = x,
                    Y2 = CANVAS_SIZE - MARGIN,
                    Stroke = GridColor,
                    StrokeThickness = 1
                };
                stoneCanvas.Children.Add(line);
            }

            // Horizontal lines
            for (int i = 0; i < GRID; i++)
            {
                double y = MARGIN + i * step;
                var line = new Line
                {
                    X1 = MARGIN,
                    Y1 = y,
                    X2 = CANVAS_SIZE - MARGIN,
                    Y2 = y,
                    Stroke = GridColor,
                    StrokeThickness = 1
                };
                stoneCanvas.Children.Add(line);
            }
        }

        private void AddClickAreas()
        {
            double step = (CANVAS_SIZE - 2 * MARGIN) / (GRID - 1);

            for (int row = 0; row < GRID; row++)
            {
                for (int col = 0; col < GRID; col++)
                {
                    var clickArea = new Ellipse
                    {
                        Width = StoneSize,
                        Height = StoneSize,
                        Fill = Brushes.Transparent,
                        Cursor = Cursors.Hand,
                        Name = $"B{row}{col}",
                    };

                    clickArea.MouseLeftButtonDown += OnGridButtonClick;

                    double x = MARGIN + col * step - StoneSize / 2;
                    double y = MARGIN + row * step - StoneSize / 2;

                    Canvas.SetLeft(clickArea, x);
                    Canvas.SetTop(clickArea, y);
                    stoneCanvas.Children.Add(clickArea);
                }
            }
        }

        private void OnGridButtonClick(object sender, RoutedEventArgs e)
        {
            Ellipse btn = sender as Ellipse;
            char stone;
            if (counter % 2 == 0)
            {
                btn.Fill = Brushes.Black;
                stone = 'b';
            }
            else
            {
                btn.Fill = Brushes.White;
                stone = 'w';
            }
            string name = btn.Name;
            int row = (int)Char.GetNumericValue(name[1]);
            int col = (int)Char.GetNumericValue(name[2]);
            grid[row, col] = stone;
            counter++;

            if (Check(grid, stone))
            {
                MessageBox.Show($"Player {(stone == 'b' ? "1" : "2")} won");
            }
        }

        private bool Check(char[,] nums, char symbol)
        {
            for (int row = 0; row < GRID; row++)
            {
                for (int col = 0; col < GRID; col++)
                {
                    // Check vertical
                    if (row <= GRID - 5 && grid[row, col] == symbol
                        && grid[row+1, col] == symbol
                        && grid[row + 2, col] == symbol
                        && grid[row + 3, col] == symbol
                        && grid[row + 4, col] == symbol)
                    {
                        return true;
                    }

                    //Check Horizontal
                    if (col <= GRID - 5 && grid[row, col] == symbol
                        && grid[row, col+1] == symbol
                        && grid[row, col+2] == symbol
                        && grid[row, col+3] == symbol
                        && grid[row, col+4] == symbol)
                    {
                        return true;
                    }

                    if (row <= GRID - 5 && col <=GRID - 5
                        && grid[row, col] == symbol
                        && grid[row + 1, col + 1] == symbol
                        && grid[row + 2, col + 2] == symbol
                        && grid[row + 3, col + 3] == symbol
                        && grid[row + 4, col + 4] == symbol)
                    {
                        return true;
                    }


                    if (row <= GRID - 5 && col >= 4
                        && grid[row, col] == symbol
                        && grid[row + 1, col - 1] == symbol
                        && grid[row + 2, col - 2] == symbol
                        && grid[row + 3, col - 3] == symbol
                        && grid[row + 4, col - 4] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
