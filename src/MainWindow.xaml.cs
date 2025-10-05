using System.Windows;


namespace gacs_app;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
  public MainWindow(MainViewModel vm)
  {
    InitializeComponent();
    DataContext = vm;
  }
}