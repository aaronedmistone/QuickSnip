using System.IO;

namespace QuickSnip
{
    class FileHelper
    {
        /// <summary>
        /// Gets the next unique path in a sequence where the resultant file name is {pathPrefix}{uniqueNumber}{pathSuffix}
        /// </summary>
        /// <param name="pathPrefix">The prefix of the path, e.g. @"C:\Pictures\log_"</param>
        /// <param name="pathSuffix">The suffix of the path, e.g. @".txt"</param>
        /// <returns>A unique path in a sequence; e.g. @"C:\Pictures\log_00231.txt"</returns>
        public static string GetNextUniquePath(string pathPrefix, string pathSuffix)
        {
            int id = 0;
            while (File.Exists(pathPrefix + id.ToString().PadLeft(5, '0') + pathSuffix))
            {
                id++;
            }
            return pathPrefix + id.ToString().PadLeft(5, '0') + pathSuffix;
        }
    }
}
