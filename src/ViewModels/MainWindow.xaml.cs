using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;

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

      if (viewModel.Score != null)
      {
        DrawPizzaSlices();
        UpdateScoreIndicator();
      }
    }
  }

  private void DrawPizzaSlices()
  {
    double centerX = PizzaCanvas.ActualWidth / 2;
    double centerY = PizzaCanvas.ActualHeight / 2;
    const double RADIUS = 180;

    double startAngle = 0;
    int sliceIndex = 0;

    // Get individual scores and max scores for each criterion
    int[] individualScores = GetIndividualScores();
    int[] maxScores = SliceConfiguration.GetMaxScores();

    foreach (double sliceAngle in SliceConfiguration.GetSliceAngles())
    {
      // Calculate color index based on the score ratio
      int score = individualScores[sliceIndex];
      int maxScore = maxScores[sliceIndex];

      Brush sliceBrush = GetColorForScore(score, maxScore);

      Path path = new()
                  {
                    Fill = sliceBrush,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                  };

      PathGeometry pizzaSliceGeometry = new();

      PathFigure figure = new()
                          {
                            StartPoint = new Point(centerX, centerY),
                            IsClosed = true
                          };

      double startAngleRadians = startAngle * Math.PI / 180;
      double endAngleRadians = (startAngle + sliceAngle) * Math.PI / 180;

      Point arcStartPoint = new(
                           centerX + RADIUS * Math.Cos(startAngleRadians),
                           centerY + RADIUS * Math.Sin(startAngleRadians)
                          );

      Point arcEndPoint = new(
                         centerX + RADIUS * Math.Cos(endAngleRadians),
                         centerY + RADIUS * Math.Sin(endAngleRadians)
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
      sliceIndex++;
    }
  }

  private void UpdateScoreIndicator()
  {
    int totalScore = viewModel.Score ?? 0;
    const int MAX_TOTAL_SCORE = 100; // Sum of all max scores

    // Calculate the ratio: 1.0 = highest score, 0.0 = lowest score
    double ratio = (double)totalScore / MAX_TOTAL_SCORE;

    // Map ratio to color index: ratio 1.0 -> index 0 (green), ratio 0.0 -> index 10 (red)
    int colorIndex = (int)Math.Round((1.0 - ratio) * (viewModel.SliceColors.Count - 1));
    colorIndex = Math.Clamp(colorIndex, 0, viewModel.SliceColors.Count - 1);

    // Get the base color
    SolidColorBrush baseColorBrush = (SolidColorBrush)viewModel.SliceColors[colorIndex];
    Color baseColor = baseColorBrush.Color;

    // Create shiny balloon effect with radial gradient
    RadialGradientBrush shinyBrush = new()
    {
      GradientOrigin = new Point(0.3, 0.3),
      Center = new Point(0.5, 0.5),
      RadiusX = 0.8,
      RadiusY = 0.8
    };

    // Add gradient stops for shiny effect
    shinyBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0)); // Highlight
    shinyBrush.GradientStops.Add(new GradientStop(LightenColor(baseColor, 0.4), 0.3));
    shinyBrush.GradientStops.Add(new GradientStop(baseColor, 0.6));
    shinyBrush.GradientStops.Add(new GradientStop(DarkenColor(baseColor, 0.3), 1.0)); // Shadow

    ScoreIndicatorBallon.Fill = shinyBrush;
  }

  private Color LightenColor(Color color, double amount)
  {
    return Color.FromArgb(
        color.A,
        (byte)Math.Min(255, color.R + (255 - color.R) * amount),
        (byte)Math.Min(255, color.G + (255 - color.G) * amount),
        (byte)Math.Min(255, color.B + (255 - color.B) * amount)
    );
  }

  private Color DarkenColor(Color color, double amount)
  {
    return Color.FromArgb(
        color.A,
        (byte)(color.R * (1 - amount)),
        (byte)(color.G * (1 - amount)),
        (byte)(color.B * (1 - amount))
    );
  }

  private int[] GetIndividualScores()
  {
    var selection = viewModel.Selection;

    return new[]
    {
      selection.PrecursorOrigin.HasValue ? (int)selection.PrecursorOrigin.Value : 0,
      selection.SolventGreenness.HasValue ? (int)selection.SolventGreenness.Value : 0,
      selection.EnergyInput.HasValue ? (int)selection.EnergyInput.Value : 0,
      selection.EFactorWasteGeneration.HasValue ? (int)selection.EFactorWasteGeneration.Value : 0,
      selection.SynthesisTime.HasValue ? (int)selection.SynthesisTime.Value : 0,
      selection.SimplicityScalability.HasValue ? (int)selection.SimplicityScalability.Value : 0,
      selection.PurificationSimplicity.HasValue ? (int)selection.PurificationSimplicity.Value : 0,
      selection.ReactionMassEfficiency.HasValue ? (int)selection.ReactionMassEfficiency.Value : 0,
      selection.QuantumYield.HasValue ? (int)selection.QuantumYield.Value : 0,
      selection.MorphologyUniformity.HasValue ? (int)selection.MorphologyUniformity.Value : 0,
      selection.PerformanceApplicability.HasValue ? (int)selection.PerformanceApplicability.Value : 0
    };
  }

  private Brush GetColorForScore(int score, int maxScore)
  {
    if (maxScore == 0)
      return viewModel.SliceColors[^1]; // Last color (red) for 0 max score

    // Calculate the ratio: 1.0 = highest score, 0.0 = lowest score
    double ratio = (double)score / maxScore;

    // Map ratio to color index: ratio 1.0 -> index 0 (green), ratio 0.0 -> index 10 (red)
    int colorIndex = (int)Math.Round((1.0 - ratio) * (viewModel.SliceColors.Count - 1));

    // Clamp to valid range
    colorIndex = Math.Clamp(colorIndex, 0, viewModel.SliceColors.Count - 1);

    return viewModel.SliceColors[colorIndex];
  }
}