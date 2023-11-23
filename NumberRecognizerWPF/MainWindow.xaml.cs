using MLModel1_ConsoleApp1;
using System.IO;
using System.Windows;
using System.Windows.Ink;
using System.Windows.Media.Imaging;

namespace NumberRecognizerWPF
{
    public partial class MainWindow : Window
    {
        private StrokeCollection strokes = new StrokeCollection();

        public MainWindow()
        {
            InitializeComponent();
            drawingCanvas.Strokes = strokes;
        }

        private void RecognizeButton_Click(object sender, RoutedEventArgs e)
        {
            // Convert the drawing to a bitmap
            var bitmap = new RenderTargetBitmap((int)drawingCanvas.ActualWidth, (int)drawingCanvas.ActualHeight, 96, 96, System.Windows.Media.PixelFormats.Default);
            bitmap.Render(drawingCanvas);

            // Convert the bitmap to bytes
            var imageBytes = ImageToBytes(bitmap);

            // Make a single prediction on the sample data and print results.
            var sortedScoresWithLabel = MLModel1.PredictAllLabels(new MLModel1.ModelInput { ImageSource = imageBytes });

            // Display the predicted number
            var predictedNumber = sortedScoresWithLabel.First().Key;
            MessageBox.Show($"Predicted Number: {predictedNumber}");
        }

        private byte[] ImageToBytes(BitmapSource bitmap)
        {
            var encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (var stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }
    }
}
