using System;
using System.Linq;

namespace NChavatte.HumanOrientedSerialization.Common
{
    public static class HOS
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ346789";

        public static string Serialize(byte[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return SerialFormBuilder.GetSerialForm(source, Alphabet);
        }

        public static DeserializationResult Deserialize(string serialForm)
        {
            if (string.IsNullOrWhiteSpace(serialForm)) throw new ArgumentException("is empty", nameof(serialForm));

            return SerialFormDeserializer.Deserialize(serialForm, Alphabet);
        }
    }
}
