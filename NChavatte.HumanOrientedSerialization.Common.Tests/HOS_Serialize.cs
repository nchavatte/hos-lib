using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NChavatte.HumanOrientedSerialization.Common.Tests
{
    [TestFixture]
    public class HOS_Serialize
    {
        [TestCase("source.0.bin", "serial-form.0.txt")]
        public void Should_convert_binary_content_to_serial_form(string sourceFileName, string expectedSerialFormFileName)
        {
            // Arrange
            byte[] source = GetResourceBytes(sourceFileName);
            string expectedSerialForm = Encoding.ASCII.GetString(GetResourceBytes(expectedSerialFormFileName));

            // Act
            string actualSerialForm = HOS.Serialize(source);

            // Assert
            Assert.AreEqual(expectedSerialForm.Trim(), actualSerialForm?.Trim());
        }

        private static byte[] GetResourceBytes(string resourceFileName)
        {
            Type type = typeof(HOS_Serialize);
            string resourceName = $"{type.Namespace}.Resources.{resourceFileName}";
            using (Stream stream = type.Assembly.GetManifestResourceStream(resourceName))
            using (MemoryStream memoryStream = new MemoryStream())
            {
                stream.CopyTo(memoryStream);
                return memoryStream.ToArray();
            }
        }
    }
}
