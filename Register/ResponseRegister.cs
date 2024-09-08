namespace SharedDataTypes;

public record ResponseRegister(string AuthToken)
{
    public string AuthToken { get; init; } = AuthToken;
}
