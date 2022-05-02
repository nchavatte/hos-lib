using System;
using System.IO;

namespace NChavatte.HumanOrientedSerialization.Common.Tests.Resources
{
    internal static class ResourceProvider
    {
        internal static byte[] GetResourceBytes(string resourceFileName)
        {
            Type type = typeof(ResourceProvider);
            string resourceName = $"{type.Namespace}.{resourceFileName}";
            using (Stream stream = type.Assembly.GetManifestResourceStream(resourceName))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
