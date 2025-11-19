using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

using GacsApp.Models;


namespace GacsApp.ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
  private readonly MainViewModel viewModel;


  public MainWindow(MainViewModel mainViewModel)
  {
    InitializeComponent();
    viewModel = mainViewModel;
    DataContext = mainViewModel;

    viewModel.PropertyChanged += ViewModel_PropertyChanged;
  }

  private void ViewModel_PropertyChanged(object? sender, PropertyChangedEventArgs e)
  {
    if (e.PropertyName == nameof(MainViewModel.Score))
    {
      PizzaCanvas.Children.Clear();

      if (viewModel.Score != 0)
      {
        DrawPizzaSlices();
      }
    }
  }

  private void DrawPizzaSlices()
  {
    Random random = new();
    const double CENTER_X = 300;
    const double CENTER_Y = 250;
    const double RADIUS = 180;

    double startAngle = 0;

    foreach (int sliceAngle in SliceConfiguration.GetSliceAngles())
    {
      int randomIndex = random.Next(viewModel.SliceColors.Count);
      Brush sliceBrush = viewModel.SliceColors[randomIndex];

      Path path = new()
                  {
                    Fill = sliceBrush,
                    Stroke = Brushes.White,
                    StrokeThickness = 1
                  };

      PathGeometry pizzaSliceGeometry = new();

      PathFigure figure = new()
                          {
                            StartPoint = new Point(CENTER_X, CENTER_Y),
                            IsClosed = true
                          };

      double startAngleRadians = startAngle * Math.PI / 180;
      double endAngleRadians = (startAngle + sliceAngle) * Math.PI / 180;

      Point arcStartPoint = new(
                           CENTER_X + RADIUS * Math.Cos(startAngleRadians),
                           CENTER_Y + RADIUS * Math.Sin(startAngleRadians)
                          );

      Point arcEndPoint = new(
                         CENTER_X + RADIUS * Math.Cos(endAngleRadians),
                         CENTER_Y + RADIUS * Math.Sin(endAngleRadians)
                        );


      figure.Segments.Add(new LineSegment(arcStartPoint, true));

      figure.Segments.Add(new ArcSegment(
                                       arcEndPoint,
                                       new Size(RADIUS, RADIUS),
                                       0,
                                       sliceAngle > 180,
                                       SweepDirection.Clockwise,
                                       true
                                      ));

      pizzaSliceGeometry.Figures.Add(figure);

      path.Data = pizzaSliceGeometry;

      PizzaCanvas.Children.Add(path);

      startAngle += sliceAngle;
    }
  }
}