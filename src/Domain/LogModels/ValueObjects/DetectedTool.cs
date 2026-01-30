namespace Domain.LogModels.ValueObjects
{
    public sealed record DetectedTool
    {
        public string Name { get; init; }
        public string? Brand { get; init; }
        public string? Model { get; init; }
        public string? SerialNumber { get; init; }
        public float Confidence { get; init; }
        public bool RedFlagged { get; init; }

        private DetectedTool(
            string name,
            float confidence,
            string? brand,
            string? model,
            string? serialNumber,
            bool redFlagged)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required.", nameof(name));

            if (confidence < 0f || confidence > 1f)
                throw new ArgumentOutOfRangeException(nameof(confidence), "Confidence must be between 0 and 1.");

            Name = name.Trim();
            Confidence = confidence;
            Brand = string.IsNullOrWhiteSpace(brand) ? null : brand.Trim();
            Model = string.IsNullOrWhiteSpace(model) ? null : model.Trim();
            SerialNumber = string.IsNullOrWhiteSpace(serialNumber) ? null : serialNumber.Trim();
            RedFlagged = redFlagged;
        }

        public static DetectedTool Custom(
            string name,
            float confidence,
            bool redFlagged,
            string? brand = null,
            string? model = null,
            string? serialNumber = null)
        {
            return new DetectedTool(name, confidence, brand, model, serialNumber, redFlagged);
        }
    }
}
