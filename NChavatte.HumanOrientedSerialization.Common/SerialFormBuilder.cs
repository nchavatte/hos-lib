using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NChavatte.HumanOrientedSerialization.Common
{
    internal class SerialFormBuilder
    {
        private readonly IEnumerator<byte> _byteEnumerator;
        private readonly string _alphabet;
        private readonly TextWriter _writer;

        private bool _byteRemains;
        private int _globalLineIndex;
        private int _lineCheckSum;

        private SerialFormBuilder(IEnumerator<byte> byteEnumerator, string alphabet, TextWriter writer)
        {
            _byteEnumerator = byteEnumerator;
            _alphabet = alphabet;
            _writer = writer;
        }

        internal static string GetSerialForm(byte[] source, string alphabet)
        {
            using (StringWriter writer = new StringWriter())
            using (IEnumerator<byte> enumerator = ((IEnumerable<byte>)source).GetEnumerator())
            {
                SerialFormBuilder builder = new SerialFormBuilder(enumerator, alphabet, writer);
                builder.Build(source.Length);
                return writer.ToString();
            }
        }

        private void Build(int contentLength)
        {
            _writer.WriteLine("----------BEGIN HOS FORM----------");
            _writer.WriteLine($"Content-length: {contentLength}");
            BuildSections();
            _writer.WriteLine("----------END HOS FORM----------");
        }

        private void BuildSections()
        {
            _byteRemains = _byteEnumerator.MoveNext();
            _globalLineIndex = 0;
            for (int sectionNumber = 1; _byteRemains; sectionNumber++)
                BuildSection(sectionNumber);
        }

        private void BuildSection(int sectionNumber)
        {
            _writer.WriteLine();
            _writer.WriteLine($"(Section {sectionNumber})");
            for (int paragraphIndex = 0; _byteRemains && paragraphIndex < 5; paragraphIndex++)
            {
                _writer.WriteLine();
                for (int lineIndex = 0; _byteRemains && lineIndex < 5; lineIndex++, _globalLineIndex++)
                    BuildLine();
            }
        }

        private void BuildLine()
        {
            _lineCheckSum = _globalLineIndex;
            for (int groupIndex = 0; _byteRemains && groupIndex < 4; groupIndex++)
            {
                for (int wordIndex = 0; _byteRemains && wordIndex < 5; wordIndex++)
                    BuildWord();

                _writer.Write("  ");
            }

            string checkSumWord = ConvertValueToSerialWord(_lineCheckSum, 3);
            _writer.WriteLine(checkSumWord);
        }

        private void BuildWord()
        {
            int wordValue = 0;
            int byteCount;
            for (byteCount = 0; _byteRemains && byteCount < 3; byteCount++)
            {
                wordValue <<= 8;
                wordValue |= _byteEnumerator.Current;
                _byteRemains = _byteEnumerator.MoveNext();
            }

            wordValue = AddParityBit(wordValue);
            _lineCheckSum ^= wordValue;
            string word = ConvertValueToSerialWord(wordValue, byteCount);
            _writer.Write($"{word} ");
        }

        private static int AddParityBit(int wordValue)
        {
            int result = wordValue << 1;
            while (wordValue != 0)
            {
                result ^= wordValue & 1;
                wordValue >>= 1;
            }

            return result;
        }

        private string ConvertValueToSerialWord(int wordValue, int byteCount)
        {
            StringBuilder wordBuilder = new StringBuilder();
            for (int bitCount = 8 * byteCount + 1; bitCount > 0 ; bitCount -= 5, wordValue >>= 5)
                wordBuilder.Insert(0, _alphabet[wordValue & 31]);

            return wordBuilder.ToString();
        }
    }
}
