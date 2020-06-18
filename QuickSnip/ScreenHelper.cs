using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace QuickSnip
{
    class ScreenHelper
    {
        /// <summary>
        /// Captures the primary screen (Use CaptureVirtualScreen to capture both monitors)
        /// </summary>
        /// <returns>A Bitmap of the captured primary screen</returns>
        public static Bitmap CapturePrimaryScreen()
        {
            var result = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppArgb);
            var gfx = Graphics.FromImage(result);
            gfx.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            return result;
        }
    }
}
