public class URL
{
    public Guid Id { get; private set; }
    public string LongUrl { get; init; }
    public string Code { get; private set; }
    public string? CustomAlias { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? ExpirationDate { get; private set; }

    public URL(string longUrl, string code, string? customAlias, DateTime? expirationDate)
    {
        Id = Guid.NewGuid();
        LongUrl = longUrl;
        Code = code;
        CreatedAt = DateTime.UtcNow;
        ExpirationDate = expirationDate;
        CustomAlias = customAlias;

        Validate();
    }

    public void Validate()
    {
        if (ExpirationDate is not null && ExpirationDate < DateTime.UtcNow)
            throw new Exception("Expiration date cannot be before than now.");
        if (CustomAlias is not null && CustomAlias.Length > 10)
            throw new Exception("Custom Alias cannot be bigger than 10 chars");
        if (!Uri.TryCreate(LongUrl, UriKind.Absolute, out _))
            throw new Exception("Invalid URL.");
    }
}