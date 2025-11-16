using SkiaSharp;
using SkiaSharp.Views.Desktop;
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
  private const int SliceCount = 11;

  // Slice sizes (must sum to 360); example uneven slices
  private readonly float[] sliceAngles = new float[]
                                         {
                                           20, 15, 25, 40, 10, 35, 30, 45, 20, 30, 70
                                         };

  // Colors for each slice (computed dynamically)
  private readonly SKColor[] sliceColors = new SKColor[SliceCount];

  public MainWindow(MainViewModel mainViewModel)
  {
    InitializeComponent();
    _viewModel = mainViewModel;
    DataContext = mainViewModel;

    Loaded += MainWindow_Loaded;

    for (int i = 0; i < SliceCount; i++)
    {
      float t = i / (float)(SliceCount - 1); // 0..1
      sliceColors[i] = InterpolateGradient(t);
    }
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

  private SKColor InterpolateGradient(float t)
  {
    // 0 = green, 0.5 = yellow, 1 = red
    if (t < 0.5f)
    {
      float u = t / 0.5f;
      return new SKColor(
          (byte)(0 + u * 255),
          (byte)(255 - u * 255),
          0);
    }
    else
    {
      float u = (t - 0.5f) / 0.5f;
      return new SKColor(
          255,
          (byte)(255 - u * 255),
          0);
    }
  }

  private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
  {
    var canvas = e.Surface.Canvas;
    canvas.Clear(SKColors.White);

    float w = e.Info.Width;
    float h = e.Info.Height;

    float radius = Math.Min(w, h) * 0.4f;
    float strokeWidth = radius; // full pie

    var center = new SKPoint(w / 2, h / 2);
    float startAngle = -90f; // top

    for (int i = 0; i < SliceCount; i++)
    {
      float sweepAngle = sliceAngles[i];
      var prevColor = i == 0 ? sliceColors[0] : sliceColors[i - 1];
      var currentColor = sliceColors[i];

      using var paint = new SKPaint
      {
        Style = SKPaintStyle.Stroke,
        StrokeWidth = strokeWidth,
        IsAntialias = true,
        Shader = SKShader.CreateSweepGradient(
              center,
              new[] { prevColor, currentColor },
              new[] { 0f, 1f },
              SKMatrix.CreateRotationDegrees(startAngle, center.X, center.Y)
          )
      };

      // Draw arc
      canvas.DrawArc(
                     new SKRect(center.X - radius, center.Y - radius, center.X + radius, center.Y + radius),
                     startAngle,
                     sweepAngle,
                     false,
                     paint);

      startAngle += sweepAngle;
    }
  }
}