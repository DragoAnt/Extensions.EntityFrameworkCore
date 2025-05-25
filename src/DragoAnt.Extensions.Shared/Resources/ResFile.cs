using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Stenn.Shared.Resources
{
    public class ResFile
    {
        private ResFile(Assembly assembly, string path, Encoding? encoding = null)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Encoding = encoding;
        }

        public Assembly Assembly { get; }
        public string Path { get; }
        public Encoding? Encoding { get; }

        private static string PrepareResPath(Assembly assembly, string resPath)
        {
            resPath = resPath.Replace('\\', '.').Replace('/', '.');
            if (resPath.StartsWith('.'))
            {
                var assemblyName = assembly.GetName().Name;
                //NOTE: This mean relative path
                return assemblyName + "." + resPath.TrimStart('.');
            }
            return resPath;
        }

        public static ResFile Absolute(string absolutePath, Assembly? assembly = null, Encoding? encoding = null)
        {
            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(absolutePath));
            }
            assembly ??= Assembly.GetCallingAssembly();
            absolutePath = PrepareResPath(assembly, absolutePath);
            return new ResFile(assembly, absolutePath);
        }

        public static ResFile Relative(string relativePath, Assembly? assembly = null, Encoding? encoding = null)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(relativePath));
            }
            relativePath = "." + relativePath;
            assembly ??= Assembly.GetCallingAssembly();
            relativePath = PrepareResPath(assembly, relativePath);
            return new ResFile(assembly, relativePath, encoding);
        }

        public bool Exists()
        {
            return Assembly.ResExists(Path);
        }

        public string Read()
        {
            return Assembly.ResRead(Path, Encoding);
        }

        public Stream ReadStream()
        {
            return Assembly.ResReadStream(Path);
        }
    }
}