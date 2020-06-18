using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace QuickSnip
{
    public partial class MainForm : Form
    {
        private Bitmap ScreenCapture { get; set; }
        private bool IsDragging { get; set; } = false;
        private Point StartDragLocation { get; set; }
        private Point EndDragLocation { get; set; }
        private SolidBrush WhiteOverlayBrush { get; set; }

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Prepare the main form including opacity
            Opacity = 0;
            Width = Screen.PrimaryScreen.Bounds.Width;
            Height = Screen.PrimaryScreen.Bounds.Height;
        }

        private void MainForm_Paint(object sender, PaintEventArgs e)
        {
            // If not initialised, capture the screen and prepare for drawing
            if (WhiteOverlayBrush == null || ScreenCapture == null)
            {
                WhiteOverlayBrush = new SolidBrush(Color.FromArgb(50, 255, 255, 255));
                ScreenCapture = ScreenHelper.CapturePrimaryScreen();
                Opacity = 100;
            }

            // Draw the screen and a transparent white overlay
            e.Graphics.DrawImage(ScreenCapture, Point.Empty);
            e.Graphics.FillRectangle(WhiteOverlayBrush, 0, 0, ScreenCapture.Width, ScreenCapture.Height);

            // If we are dragging the box to select a region to capture, let's draw that for the user
            if (!IsDragging || EndDragLocation.X <= StartDragLocation.X || EndDragLocation.Y <= StartDragLocation.Y)
                return;

            // Draw the region of the screen we have selected and a red border around the edge
            var targetRect = new Rectangle(StartDragLocation.X, StartDragLocation.Y, EndDragLocation.X - StartDragLocation.X, EndDragLocation.Y - StartDragLocation.Y);
            e.Graphics.DrawImage(ScreenCapture, targetRect, targetRect, GraphicsUnit.Pixel);
            e.Graphics.DrawRectangle(Pens.Red, targetRect);
        }

        private void MainForm_MouseDown(object sender, MouseEventArgs e)
        {
            // When we hold the mouse down, start dragging
            IsDragging = true;
            StartDragLocation = e.Location;
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            // When the mouse moves, if dragging, set the new end location and invalidate to redraw
            if (IsDragging)
            {
                EndDragLocation = e.Location;
                Invalidate();
            }
        }

        private void MainForm_MouseUp(object sender, MouseEventArgs e)
        {
            // When we release the mouse, if dragging, stop dragging and save the captured region
            // also add it to clipboard and close the application when finished.
            if (IsDragging)
            {
                if (EndDragLocation.X > StartDragLocation.X && EndDragLocation.Y > StartDragLocation.Y)
                {
                    IsDragging = false;
                    Rectangle targetRect = new Rectangle( StartDragLocation.X, StartDragLocation.Y,
                                                          EndDragLocation.X - StartDragLocation.X,
                                                          EndDragLocation.Y - StartDragLocation.Y);
                    Bitmap output = ScreenCapture.Crop(targetRect);
                    string myPicturesFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    string folder = Path.Combine(myPicturesFolder, "Snips");
                    
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var outputPath = FileHelper.GetNextUniquePath(Path.Combine(folder, "snip_"), ".png");
                    output.Save(outputPath, ImageFormat.Png);
                    Clipboard.SetImage(output);
                }
                Close();
            }
        }
    }
}
