using System;

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
            throw new NotImplementedException();
        }
    }
}
