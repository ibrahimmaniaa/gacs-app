using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GacsApp.ViewModels;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  private readonly MainViewModel _viewModel;

  public MainWindow(MainViewModel mainViewModel)
  {
    InitializeComponent();
    _viewModel = mainViewModel;
    DataContext = mainViewModel;

    Loaded += MainWindow_Loaded;
  }

  private void MainWindow_Loaded(object sender, RoutedEventArgs e)
  {
    DrawPizzaSlices();
  }

  private void DrawPizzaSlices()
  {
    double centerX = 300;
    double centerY = 250;
    const double RADIUS = 180;

    double startAngle = 0;

    for (int i = 0; i < _viewModel.SliceSizes.Length; i++)
    {
      double sliceAngle = _viewModel.SliceSizes[i];

      Brush sliceBrush = _viewModel.SliceColors;

      Path path = new()
      {
        Fill = sliceBrush,
        Stroke = sliceBrush, // Set the stroke color to match the slice color
      };

      // Create the geometry for the pizza slice
      PathGeometry geometry = new();
      PathFigure figure = new()
                          {
                            StartPoint = new Point(centerX, centerY),
                            IsClosed = true
                          };

      // Calculate start and end points of the arc
      double startRadians = startAngle * Math.PI / 180;
      double endRadians = (startAngle + sliceAngle) * Math.PI / 180;

      Point startPoint = new(
                             centerX + RADIUS * Math.Cos(startRadians),
                             centerY + RADIUS * Math.Sin(startRadians)
                            );

      Point endPoint = new(
                           centerX + RADIUS * Math.Cos(endRadians),
                           centerY + RADIUS * Math.Sin(endRadians)
                          );

      // Add line to start of arc
      figure.Segments.Add(new LineSegment(startPoint, true));

      // Add arc segment
      figure.Segments.Add(new ArcSegment(
        endPoint,
        new Size(RADIUS, RADIUS),
        0,
        sliceAngle > 180, // isLargeArc
        SweepDirection.Clockwise,
        true
      ));

      geometry.Figures.Add(figure);
      path.Data = geometry;

      // Add the path to the Canvas
      PizzaCanvas.Children.Add(path);

      startAngle += sliceAngle;
    }
  }
}