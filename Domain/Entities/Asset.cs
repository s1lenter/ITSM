namespace Domain.Entities;

public class Asset
{
    public Guid Id { get; private set; }
    public string Code { get; set; }
    public string Name { get; set; }

    public Asset()
    {
        Id = Guid.NewGuid();
    }
}
