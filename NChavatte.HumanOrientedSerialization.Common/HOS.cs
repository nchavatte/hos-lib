using System;

namespace NChavatte.HumanOrientedSerialization.Common
{
    public static class HOS
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ346789";

        /// <summary>
        /// Converts bytes into <see href="https://github.com/nchavatte/hos-lib/wiki/Serial-form">serial form</see>
        /// </summary>
        /// <param name="source">Bytes to be converted</param>
        /// <returns>Serial form</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string Serialize(byte[] source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return SerialFormBuilder.GetSerialForm(source, Alphabet);
        }

        /// <summary>
        /// Converts <see href="https://github.com/nchavatte/hos-lib/wiki/Serial-form">serial form</see> into bytes
        /// or indicates serial form structure error.
        /// </summary>
        /// <param name="serialForm"></param>
        /// <returns>Object that contains deserialized bytes or error cause indication</returns>
        /// <exception cref="ArgumentException">
        /// when input serial form is null, empty or white space
        /// </exception>
        public static DeserializationResult Deserialize(string serialForm)
        {
            if (string.IsNullOrWhiteSpace(serialForm)) throw new ArgumentException("is empty", nameof(serialForm));

            return SerialFormDeserializer.Deserialize(serialForm, Alphabet);
        }
    }
}
