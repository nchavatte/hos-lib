namespace NChavatte.HumanOrientedSerialization.Common
{
    public enum DeserializationErrorType
    {
        WordMalformed,
        ParityBitDoesNotMatch,
        LineCheckSumDoesNotMatch,
        LineCheckSumMissing,
        ContentLengthDoesNotMatch,
        ContentLengthMissing,
        OpeningLineMissing,
        ClosingLineMissing,
    }
}
