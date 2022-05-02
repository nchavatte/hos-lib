using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NChavatte.HumanOrientedSerialization.Common
{
    internal class SerialFormDeserializer
    {
        internal static DeserializationResult Deserialize(string serialForm, string alphabet)
        {
            using (var enumerator = ReadLines(serialForm).GetEnumerator())
            {
                if (!enumerator.MoveNext()
                    || !enumerator.Current.LineContent.Equals("----------BEGIN HOS FORM----------"))
                    return DeserializationResult.OpeningLineMissing();

                int contentLength;
                if (!TryParseContentLength(enumerator, out contentLength))
                    return DeserializationResult.ContentLengthMissing();

                List<byte> bytes = new List<byte>();
                for (int contentLineIndex = 0; enumerator.MoveNext(); contentLineIndex++)
                {
                    if (enumerator.Current.LineContent.Equals("----------END HOS FORM----------"))
                    {
                        if (contentLength == bytes.Count)
                            return new DeserializationResult(bytes.ToArray());

                        return DeserializationResult.ContentLengthDoesNotMatch();
                    }

                    DeserializationResult lineResult = DeserializeLine(
                        enumerator.Current.LineNumber
                        , contentLineIndex
                        , enumerator.Current.LineContent
                        , alphabet);
                    if (lineResult.IsError)
                        return lineResult;

                    bytes.AddRange(lineResult.Content);
                }

                return DeserializationResult.ClosingLineMissing();
            }
        }

        private static IEnumerable<(int LineNumber, string LineContent)> ReadLines(string text)
        {
            using (TextReader reader = new StringReader(text))
            {
                string line;
                for (int lineNumber = 1; (line = reader.ReadLine()) != null; lineNumber++)
                {
                    if (!string.IsNullOrWhiteSpace(line) && !line.StartsWith("("))
                        yield return (lineNumber, line.Trim());
                }
            }
        }

        private static bool TryParseContentLength(IEnumerator<(int LineNumber, string LineContent)> enumerator, out int contentLength)
        {
            contentLength = 0;
            if (!enumerator.MoveNext())
                return false;

            string[] lineParts = enumerator.Current.LineContent.Split(':').Select(p => p.Trim().ToLower()).ToArray();
            return lineParts.Length == 2
                && lineParts[0].Equals("content-length")
                && int.TryParse(lineParts[1], out contentLength);
        }

        private static DeserializationResult DeserializeLine(int lineNumber, int contentLineIndex, string lineContent, string alphabet)
        {
            string[] lineParts = lineContent.Split(' ', '\t')
                .Where(w => !string.IsNullOrWhiteSpace(w))
                .ToArray();
            if (lineParts.Length < 2)
                return DeserializationResult.LineCheckSumMissing(lineNumber);

            int lineCheckSum = contentLineIndex;
            int wordNumber = 0;
            List<byte> bytes = new List<byte>();
            var words = lineParts.SkipLast(1).ToArray();
            foreach (var word in words)
            {
                wordNumber++;
                int wordValue;
                if (!TryGetWordValue(word, alphabet, out wordValue))
                    return DeserializationResult.WordMalformed(lineNumber, wordNumber);

                byte[] wordBytes;
                if (!TryGetWordBytes(wordValue, word.Length, out wordBytes))
                    return DeserializationResult.ParityBitDoesNotMatch(lineNumber, wordNumber);

                bytes.AddRange(wordBytes);
                lineCheckSum ^= wordValue;
            }

            int expectedCheckSum;
            if (!TryGetWordValue(lineParts.Last(), alphabet, out expectedCheckSum))
                return DeserializationResult.WordMalformed(lineNumber, wordNumber + 1);

            if (expectedCheckSum != lineCheckSum)
                return DeserializationResult.LineCheckSumDoesNotMatch(lineNumber);

            return new DeserializationResult(bytes.ToArray());
        }

        private static bool TryGetWordValue(string word, string alphabet, out int wordValue)
        {
            wordValue = 0;
            foreach (char c in word)
            {
                if (!alphabet.Contains(c))
                    return false;

                wordValue <<= 5;
                wordValue |= alphabet.IndexOf(c);
            }

            return true;
        }

        private static bool TryGetWordBytes(int wordValue, int wordLength, out byte[] wordBytes)
        {
            wordBytes = Array.Empty<byte>();

            int bitCount = 5 * wordLength;

            int parity = wordValue;
            for (int i = 1; i < bitCount && parity != 0; i++)
                parity = (parity >> 1) ^ (parity & 1);

            if (parity != 0)
                return false;

            wordValue >>= 1;
            bitCount--;
            List<byte> bytes = new List<byte>();
            while (bitCount >= 8)
            {
                bytes.Insert(0, (byte)(wordValue & 255));
                wordValue >>= 8;
                bitCount -= 8;
            }

            wordBytes = bytes.ToArray();
            return true;
        }
    }
}
