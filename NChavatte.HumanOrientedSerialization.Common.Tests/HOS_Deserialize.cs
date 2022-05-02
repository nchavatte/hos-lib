using NChavatte.HumanOrientedSerialization.Common.Tests.Resources;
using NUnit.Framework;
using System.Linq;
using System.Text;

namespace NChavatte.HumanOrientedSerialization.Common.Tests
{
    [TestFixture]
    public class HOS_Deserialize
    {
        [TestCase("serial-form.0.txt", "source.0.bin")]
        [TestCase("serial-form.1.txt", "source.1.bin")]
        [TestCase("serial-form.2.txt", "source.2.bin")]
        public void Should_convert_serial_form_to_binary(string serialFormFileName, string expectedBinaryFileName)
        {
            // Arrange
            string serialForm = Encoding.ASCII.GetString(ResourceProvider.GetResourceBytes(serialFormFileName));
            byte[] expectedBytes = ResourceProvider.GetResourceBytes(expectedBinaryFileName);

            // Act
            DeserializationResult actualResult = HOS.Deserialize(serialForm);

            // Assert
            Assert.IsFalse(actualResult?.IsError);
            Assert.IsNull(actualResult.Error);
            Assert.AreEqual(expectedBytes.Length, actualResult?.Content?.Length);
            int byteNum = 0;
            foreach ((byte ExpectedByte, byte ActualByte) pair in expectedBytes.Zip(actualResult.Content))
            {
                byteNum++;
                Assert.AreEqual(pair.ExpectedByte, pair.ActualByte, $"Byte #{byteNum}");
            }
        }
    }
}
