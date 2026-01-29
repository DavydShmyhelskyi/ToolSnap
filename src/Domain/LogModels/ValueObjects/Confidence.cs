namespace Domain.LogModels.ValueObjects
{
    public sealed record Confidence
    {
        public float Value { get; init; }

        private Confidence(float value) => Value = value;

        public static Confidence FromFloat(float value)
        {
            if (value < 0f || value > 1f) throw new ArgumentOutOfRangeException(nameof(value), "Confidence must be between 0 and 1.");
            return new Confidence(value);
        }

        public static Confidence High() => new Confidence(0.9f);
        public static Confidence Medium() => new Confidence(0.6f);
        public static Confidence Low() => new Confidence(0.3f);

        public override string ToString() => Value.ToString("0.##");
    }
}
