using NUnit.Framework;
using System;

namespace NChavatte.HumanOrientedSerialization.Common.Tests
{
    [TestFixture]
    public class HOS_Serialize_then_Deserialize
    {
        private static readonly Random R = new Random();

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(4)]
        [TestCase(5)]
        [TestCase(1600)]
        public void Should_be_identity(int sourceLength)
        {
            // Arrange
            byte[] source = new byte[sourceLength];
            R.NextBytes(source);

            // Act
            string serialForm = HOS.Serialize(source);
            DeserializationResult deserializationResult = HOS.Deserialize(serialForm);

            //Assert
            Assert.IsFalse(deserializationResult.IsError, nameof(deserializationResult.IsError));
            Assert.AreEqual(source.Length, deserializationResult.Content.Length, "Byte array length");
            for (int i = 0; i < source.Length; i++)
                Assert.AreEqual(source[i], deserializationResult.Content[i], $"Byte index: {i}");
        }
    }
}
