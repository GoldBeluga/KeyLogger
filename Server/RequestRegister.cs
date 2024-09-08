namespace SharedDataTypes;

public record RequestRegister(int Id, string Password)
{
    public int Id { get; set; } = Id;
    public string Password { get; set; } = Password;
}
