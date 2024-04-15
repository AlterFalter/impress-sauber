using System.Windows;

namespace DataGenerator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private CursorPositionDataGenerator _dataGenerator;

        public MainWindow()
        {
            log.Info("Started building main window");
            InitializeComponent();
            SetToggleButtonInactive();
            _dataGenerator = new CursorPositionDataGenerator();
            log.Info("Initialized main window for data generator.");
        }

        private void ToggleDataStreamClicked(object sender, RoutedEventArgs e)
        {
            if (_dataGenerator.ShouldGenerate)
            {
                StopCursorTracking();
            }
            else
            {
                StartCursorTracking();
            }
        }

        private void StartCursorTracking()
        {
            _dataGenerator.ShouldGenerate = true;
            SetToggleButtonActive();
        }

        private void StopCursorTracking()
        {
            _dataGenerator.ShouldGenerate = false;
            SetToggleButtonInactive();
        }

        private void SetToggleButtonActive()
        {
            this.ToggleDataStreamButton.Content = "Click to stop cursor tracking\n(currently: active)";
            this.ToggleDataStreamButton.Background = System.Windows.Media.Brushes.Green;
        }

        private void SetToggleButtonInactive()
        {
            this.ToggleDataStreamButton.Content = "Click to start cursor tracking\n(currently: inactive)";
            this.ToggleDataStreamButton.Background = System.Windows.Media.Brushes.Red;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this._dataGenerator.ShouldStop = true;
        }
    }
}