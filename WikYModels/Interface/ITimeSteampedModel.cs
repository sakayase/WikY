namespace WikYModels.Interface
{
    public interface ITimeStampedModel
    {
        DateTime CreatedAt { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
