namespace SharedDataTypes;

public record ReceivedData(string AuthToken, int Id, DateTime StartedTime, string PureData)
{
    public string AuthToken { get; init; } = AuthToken;
    public int TargetId { get; init; } = Id;
    public DateTime StartedTime { get; init; } = StartedTime;
    public string PureData { get; init; } = PureData;
}
