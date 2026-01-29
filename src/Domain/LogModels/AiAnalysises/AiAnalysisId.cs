namespace Domain.LogModels.AiAnalysises
{
    public sealed record AiAnalysisId(Guid Value)
    {
        public static AiAnalysisId Empty() => new(Guid.Empty);
        public static AiAnalysisId New() => new(Guid.NewGuid());
        public override string ToString() => Value.ToString();
    }
}
