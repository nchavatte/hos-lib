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
            for (int i = 0; i < expectedBytes.Length; i++)
                Assert.AreEqual(expectedBytes[i], actualResult.Content[i], $"Byte index: {i}");
        }

        [TestCase("serial-form.1.checksum-error-L7W3.txt", DeserializationErrorType.LineCheckSumDoesNotMatch, 7, 0)]
        [TestCase("serial-form.1.parity-error-L6W5.txt", DeserializationErrorType.ParityBitDoesNotMatch, 6, 5)]
        public void Should_indicate_located_error(string serialFormFileName, DeserializationErrorType errorType, int lineNumber, int wordNumner)
        {
            // Arrange
            string serialForm = Encoding.ASCII.GetString(ResourceProvider.GetResourceBytes(serialFormFileName));

            // Act
            DeserializationResult actualResult = HOS.Deserialize(serialForm);

            // Assert
            Assert.IsTrue(actualResult.IsError, nameof(actualResult.IsError));
            Assert.AreEqual(errorType, actualResult.Error.ErrorType, nameof(actualResult.Error.ErrorType));
            Assert.AreEqual(lineNumber, actualResult.Error.LineNumber, nameof(actualResult.Error.LineNumber));
            Assert.AreEqual(wordNumner, actualResult.Error.WordNumber, nameof(actualResult.Error.WordNumber));
        }

        [TestCase("----------BEGIN HOS FORM----------", "", DeserializationErrorType.OpeningLineMissing)]
        [TestCase("----------BEGIN HOS FORM----------", "--------BEGIN HOS FORM----------", DeserializationErrorType.OpeningLineMissing)]
        [TestCase("----------END HOS FORM----------", "", DeserializationErrorType.ClosingLineMissing)]
        [TestCase("----------END HOS FORM----------", "--------END HOS FORM----------", DeserializationErrorType.WordMalformed)]
        [TestCase("Content-length: 64", "Content-length: 63", DeserializationErrorType.ContentLengthDoesNotMatch)]
        [TestCase("Content-length: 64", "Content-length: 65", DeserializationErrorType.ContentLengthDoesNotMatch)]
        [TestCase("Content-length: 64", "Content-length: xy", DeserializationErrorType.ContentLengthMissing)]
        [TestCase("Content-length: 64", "Content-length 64", DeserializationErrorType.ContentLengthMissing)]
        [TestCase("Content-length: 64", "64", DeserializationErrorType.ContentLengthMissing)]
        [TestCase("Content-length: 64", "", DeserializationErrorType.ContentLengthMissing)]
        public void Should_indicate_unlocated_error(string oldText, string newText, DeserializationErrorType errorType)
        {
            // Arrange
            string serialForm = Encoding.ASCII.GetString(ResourceProvider.GetResourceBytes("serial-form.1.txt"));
            serialForm = serialForm.Replace(oldText, newText);

            // Act
            DeserializationResult actualResult = HOS.Deserialize(serialForm);

            // Assert
            Assert.IsTrue(actualResult.IsError, nameof(actualResult.IsError));
            Assert.AreEqual(errorType, actualResult.Error.ErrorType, nameof(actualResult.Error.ErrorType));
        }
    }
}
