using NChavatte.HumanOrientedSerialization.Common.Tests.Resources;
using NUnit.Framework;
using System.Text;

namespace NChavatte.HumanOrientedSerialization.Common.Tests
{
    [TestFixture]
    public class HOS_Serialize
    {
        [TestCase("source.0.bin", "serial-form.0.txt")]
        [TestCase("source.1.bin", "serial-form.1.txt")]
        [TestCase("source.2.bin", "serial-form.2.txt")]
        public void Should_convert_binary_content_to_serial_form(string sourceFileName, string expectedSerialFormFileName)
        {
            // Arrange
            byte[] source = ResourceProvider.GetResourceBytes(sourceFileName);
            string expectedSerialForm = Encoding.ASCII.GetString(ResourceProvider.GetResourceBytes(expectedSerialFormFileName));

            // Act
            string actualSerialForm = HOS.Serialize(source);

            // Assert
            Assert.AreEqual(expectedSerialForm.Trim(), actualSerialForm?.Trim());
        }
    }
}
