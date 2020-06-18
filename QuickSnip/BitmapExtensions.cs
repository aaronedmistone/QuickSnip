using System.Drawing;

namespace QuickSnip
{
    static class BitmapExtensions
    {
        /// <summary>
        /// Crops the Bitmap to the specified rectangle
        /// </summary>
        /// <param name="bitmap">The source bitmap</param>
        /// <param name="cropArea">The crop area rectangle</param>
        /// <returns>A bitmap cropped to the given region</returns>
        public static Bitmap Crop(this Bitmap bitmap, Rectangle cropArea)
        {
            Bitmap target = new Bitmap(cropArea.Width, cropArea.Height);
            using (Graphics gfx = Graphics.FromImage(target))
            {
                var destRect = new Rectangle(0, 0, target.Width, target.Height);
                gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
                gfx.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gfx.DrawImage(bitmap, destRect, cropArea,GraphicsUnit.Pixel);
            }
            return target;
        }
    }
}
