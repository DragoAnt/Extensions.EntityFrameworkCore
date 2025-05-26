using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Stenn.Shared.Resources
{
    /// <summary>
    /// Resources extensions
    /// </summary>
    public static class ResourcesExtensions
    {
        /// <summary>
        /// Resource file exists or not
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="embeddedResFileName"></param>
        /// <returns></returns>
        public static bool ResExists(this Assembly assembly, string embeddedResFileName)
        {
            var info = assembly.GetManifestResourceInfo(embeddedResFileName);
            return info != null;
        }

        /// <summary>
        /// Read resource file as a stream
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="embeddedResFileName"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static Stream ResReadStream(this Assembly assembly, string embeddedResFileName)
        {
            var stream = assembly.GetManifestResourceStream(embeddedResFileName);
            if (stream == null)
            {
                throw new ArgumentException($"Can't find embedded resource with name '{embeddedResFileName}' in assembly '{assembly.FullName}'");
            }
            return stream;
        }

        /// <summary>
        /// Read resource file as a string
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="embeddedResFileName"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string ResRead(this Assembly assembly, string embeddedResFileName, Encoding? encoding = null)
        {
            using var stream = assembly.ResReadStream(embeddedResFileName);
            using var reader = new StreamReader(stream, encoding, true, -1, false);
            return reader.ReadToEnd();
        }
    }
}