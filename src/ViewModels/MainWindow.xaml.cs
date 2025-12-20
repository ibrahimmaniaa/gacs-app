using System.Windows;
using System.Windows.Media;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using Microsoft.Win32;

using PdfSharp.Pdf;
using PdfSharp.Drawing;

using GacsApp.Models;

using Path = System.Windows.Shapes.Path;


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

      if (Score != null)
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
    const double IMAGE_DISTANCE = 210; // Distance from center for images
    const double IMAGE_SIZE = 24; // Size of the icon images
    const double CENTER_IMAGE_SIZE = 150; // Size of the center image

    double startAngle = 0;
    int sliceIndex = 0;

    int[] individualScores = GetIndividualScores();
    int[] maxScores = SliceConfiguration.GetMaxScores();
    string[] sliceIcons = GetSliceIcons();

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

      // Add icon image for this slice
      double midAngle = startAngle + (sliceAngle / 2);
      double midAngleRadians = midAngle * Math.PI / 180;

      double imageX = centerX + IMAGE_DISTANCE * Math.Cos(midAngleRadians) - (IMAGE_SIZE / 2);
      double imageY = centerY + IMAGE_DISTANCE * Math.Sin(midAngleRadians) - (IMAGE_SIZE / 2);

      Image icon = new()
                   {
                     Width = IMAGE_SIZE,
                     Height = IMAGE_SIZE,
                     Source = new BitmapImage(new Uri(sliceIcons[sliceIndex], UriKind.Relative))
                   };

      Canvas.SetLeft(icon, imageX);
      Canvas.SetTop(icon, imageY);

      PizzaCanvas.Children.Add(icon);

      startAngle += sliceAngle;
      sliceIndex++;
    }

    // Add center image on top of all slices
    Image centerImage = new()
    {
      Width = CENTER_IMAGE_SIZE,
      Height = CENTER_IMAGE_SIZE,
      Source = new BitmapImage(new Uri("/Assets/canvas-logo.png", UriKind.Relative)),
      Clip = new EllipseGeometry(
        new Point(CENTER_IMAGE_SIZE / 2, CENTER_IMAGE_SIZE / 2),
        CENTER_IMAGE_SIZE / 2,
        CENTER_IMAGE_SIZE / 2)
    };

    Canvas.SetLeft(centerImage, centerX - (CENTER_IMAGE_SIZE / 2));
    Canvas.SetTop(centerImage, centerY - (CENTER_IMAGE_SIZE / 2));

    // Set ZIndex to ensure it's on top
    Canvas.SetZIndex(centerImage, 1000);

    PizzaCanvas.Children.Add(centerImage);
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
    GacsSelection selection = viewModel.Selection;

    return
    [
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
    ];
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


  /// <summary>
  /// Gets the icon paths for each criterion slice in order.
  /// </summary>
  private string[] GetSliceIcons() =>
  [
    "/Assets/precursor-origin.png",
    "/Assets/solvent-greeness.png",
    "/Assets/energy-input.png",
    "/Assets/waste-generation.png",
    "/Assets/synthesis-time.png",
    "/Assets/synthesis-simplicity.png",
    "/Assets/purification-simplicity.png",
    "/Assets/yield.png",
    "/Assets/quantum-yield.png",
    "/Assets/morphology-uniformity.png",
    "/Assets/performance.png"
  ];


  #region Canvas Save Methods

  private void SaveImage_Click(object sender, RoutedEventArgs e)
  {
    if (PizzaCanvas.Children.Count == 0)
    {
      MessageBox.Show("Please calculate the score first to generate the visualization.", "No Content", MessageBoxButton.OK, MessageBoxImage.Information);
      return;
    }

    SaveFileDialog saveDialog = new()
                                {
                                  Filter = "PNG Image|*.png|JPG Image|*.jpg|PDF Document|*.pdf",
                                  DefaultExt = "png",
                                  FileName = $"GACS_TotalScore_{viewModel.Score}_{DateTime.Now:yyyyMMdd_HHmm}"
                                };

    if (saveDialog.ShowDialog() == true)
    {
      try
      {
        string extension = System.IO.Path.GetExtension(saveDialog.FileName).ToLower();

        if (extension == ".pdf")
        {
          SaveCanvasAsPdf(saveDialog.FileName);
        }
        else
        {
          SaveCanvasAsImage(saveDialog.FileName, extension);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
      }
    }
  }


  private void SaveCanvasAsImage(string filePath, string extension)
  {
    // Ensure canvas is properly laid out
    PizzaCanvas.UpdateLayout();

    // High resolution scale factor (2x for better quality)
    const double SCALE = 2.0;
    const double EXTRA_HEIGHT = 60; // Extra space below canvas for score info

    // Use ActualWidth and ActualHeight to get the full dimensions of the canvas
    double canvasWidth = PizzaCanvas.ActualWidth;
    double canvasHeight = PizzaCanvas.ActualHeight;
    double totalHeight = canvasHeight + EXTRA_HEIGHT;

  int pixelWidth = (int)(canvasWidth * SCALE);
    int pixelHeight = (int)(totalHeight * SCALE);

    // Create a render target bitmap with high resolution
    RenderTargetBitmap renderBitmap = new(
        pixelWidth,
 pixelHeight,
     96 * SCALE, // DPI X
      96 * SCALE, // DPI Y
    PixelFormats.Pbgra32
             );

    // Create a drawing visual to apply scaling
    DrawingVisual drawingVisual = new();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
      // Draw white background first
      drawingContext.DrawRectangle(
         Brushes.White,
    null,
      new Rect(0, 0, canvasWidth * SCALE, totalHeight * SCALE)
         );

      // Apply scaling transform for all content
      //drawingContext.PushTransform(new ScaleTransform(SCALE, SCALE));

      // Create a visual brush from the canvas
      VisualBrush visualBrush = new(PizzaCanvas)
              {
       Stretch = Stretch.None
};

      // Draw the canvas content
      drawingContext.DrawRectangle(
          visualBrush,
         null,
       new Rect(0, 0, canvasWidth, canvasHeight)
     );

      // Draw score information below the canvas
      if (viewModel.Score != null)
      {
        // Create formatted text for score
        FormattedText scoreText = new(
          $"Total Score: {viewModel.Score}",
          System.Globalization.CultureInfo.CurrentCulture,
 FlowDirection.LeftToRight,
    new Typeface("Segoe UI"),
          18,
      Brushes.Black,
          VisualTreeHelper.GetDpi(this).PixelsPerDip
        );

        // Center the text horizontally below the canvas
        double textX = (canvasWidth - scoreText.Width) / 2;
        double textY = canvasHeight + 18;
   drawingContext.DrawText(scoreText, new Point(textX, textY));

        // Draw score indicator balloon next to the text
        int totalScore = viewModel.Score.Value;
     const int MAX_TOTAL_SCORE = 100;
     double ratio = (double)totalScore / MAX_TOTAL_SCORE;
        int colorIndex = (int)Math.Round((1.0 - ratio) * (viewModel.SliceColors.Count - 1));
        colorIndex = Math.Clamp(colorIndex, 0, viewModel.SliceColors.Count - 1);

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
        shinyBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
   shinyBrush.GradientStops.Add(new GradientStop(LightenColor(baseColor, 0.4), 0.3));
        shinyBrush.GradientStops.Add(new GradientStop(baseColor, 0.6));
        shinyBrush.GradientStops.Add(new GradientStop(DarkenColor(baseColor, 0.3), 1.0));

        // Position balloon to the right of the score text
        double balloonX = textX + scoreText.Width + 18;
        double balloonY = textY + (scoreText.Height / 2);
        double balloonRadius = 14;

        drawingContext.DrawEllipse(shinyBrush, null, new Point(balloonX, balloonY), balloonRadius, balloonRadius);
      }

      //drawingContext.Pop(); // Pop the transform
    }

    // Render the drawing visual
    renderBitmap.Render(drawingVisual);

    // Encode and save
    BitmapEncoder encoder = extension switch
    {
      ".jpg" => new JpegBitmapEncoder
    {
          QualityLevel = 95
           },
      _ => new PngBitmapEncoder()
    };

    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

    using FileStream fileStream = new(filePath, FileMode.Create);
    encoder.Save(fileStream);
  }


  private void SaveCanvasAsPdf(string filePath)
  {
    // Ensure canvas is properly laid out
    PizzaCanvas.UpdateLayout();

    // Create a very high-resolution bitmap for embedding in PDF
    const double SCALE = 3.0;
    const double EXTRA_HEIGHT = 60; // Extra space below canvas for score info

    double canvasWidth = PizzaCanvas.ActualWidth;
    double canvasHeight = PizzaCanvas.ActualHeight;
    double totalHeight = canvasHeight + EXTRA_HEIGHT;

    int pixelWidth = (int)(canvasWidth * SCALE);
    int pixelHeight = (int)(totalHeight * SCALE);
    double dpi = 96 * SCALE; // 288 DPI

  // Create render target bitmap
    RenderTargetBitmap renderBitmap = new(
pixelWidth,
      pixelHeight,
      dpi,
      dpi,
      PixelFormats.Pbgra32
    );

    // Create a drawing visual to apply scaling
    DrawingVisual drawingVisual = new();
    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
    {
  // Draw white background first
      drawingContext.DrawRectangle(
        Brushes.White,
        null,
        new Rect(0, 0, canvasWidth * SCALE, totalHeight * SCALE)
      );

      // Create a visual brush from the canvas
      VisualBrush visualBrush = new(PizzaCanvas)
      {
        Stretch = Stretch.None
      };

      // Draw the canvas content
 drawingContext.DrawRectangle(
        visualBrush,
        null,
        new Rect(0, 0, canvasWidth, canvasHeight)
      );

 // Draw score information below the canvas
      if (viewModel.Score != null)
      {
        // Create formatted text for score
        FormattedText scoreText = new(
          $"Total Score: {viewModel.Score}",
          System.Globalization.CultureInfo.CurrentCulture,
          FlowDirection.LeftToRight,
    new Typeface("Segoe UI"),
          18,
     Brushes.Black,
    VisualTreeHelper.GetDpi(this).PixelsPerDip
        );

        // Center the text horizontally below the canvas
        double textX = (canvasWidth - scoreText.Width) / 2;
        double textY = canvasHeight + 18;
  drawingContext.DrawText(scoreText, new Point(textX, textY));

      // Draw score indicator balloon next to the text
        int totalScore = viewModel.Score.Value;
        const int MAX_TOTAL_SCORE = 100;
        double ratio = (double)totalScore / MAX_TOTAL_SCORE;
  int colorIndex = (int)Math.Round((1.0 - ratio) * (viewModel.SliceColors.Count - 1));
        colorIndex = Math.Clamp(colorIndex, 0, viewModel.SliceColors.Count - 1);

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
     shinyBrush.GradientStops.Add(new GradientStop(Colors.White, 0.0));
      shinyBrush.GradientStops.Add(new GradientStop(LightenColor(baseColor, 0.4), 0.3));
        shinyBrush.GradientStops.Add(new GradientStop(baseColor, 0.6));
        shinyBrush.GradientStops.Add(new GradientStop(DarkenColor(baseColor, 0.3), 1.0));

        // Position balloon to the right of the score text
 double balloonX = textX + scoreText.Width + 18;
        double balloonY = textY + (scoreText.Height / 2);
        double balloonRadius = 14;

        drawingContext.DrawEllipse(shinyBrush, null, new Point(balloonX, balloonY), balloonRadius, balloonRadius);
      }
    }

    // Render the drawing visual
    renderBitmap.Render(drawingVisual);

    // Convert to PNG in memory
    PngBitmapEncoder encoder = new();
    encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

    using MemoryStream pngStream = new();
    encoder.Save(pngStream);
    pngStream.Flush();
    pngStream.Position = 0;

    // Verify the stream has content
    if (pngStream.Length == 0)
    {
      throw new InvalidOperationException("Failed to generate image from canvas.");
    }

    // Create PDF document
    PdfDocument document = new();
    document.Info.Title = "GACS Visualization";
 document.Info.Author = "GACS App";
    document.Info.Subject = "Green Assessment of CQDs Synthesis";
    document.Info.Creator = "GACS Application";
    document.Info.CreationDate = DateTime.Now;

    // Add a page
    PdfPage page = document.AddPage();

    // Set page size to accommodate the image at high quality (including extra height)
    double pageWidth = canvasWidth * 1.2; // Add some margin
    double pageHeight = totalHeight * 1.2;

    page.Width = XUnit.FromPoint(pageWidth);
    page.Height = XUnit.FromPoint(pageHeight);

    // Get graphics object for drawing
    XGraphics gfx = XGraphics.FromPdfPage(page);

    // Set high quality rendering
  gfx.SmoothingMode = XSmoothingMode.HighQuality;

    // Load the image from memory stream
    XImage xImage = XImage.FromStream(pngStream);

    // Calculate centered position with margins
    double margin = pageWidth * 0.1;
    double imageWidth = pageWidth - (2 * margin);
    double imageHeight = pageHeight - (2 * margin);

    // Draw image centered on page
    gfx.DrawImage(xImage, margin, margin, imageWidth, imageHeight);

    // Dispose the image
    xImage.Dispose();

  // Save the PDF
    document.Save(filePath);
  }

  #endregion
}