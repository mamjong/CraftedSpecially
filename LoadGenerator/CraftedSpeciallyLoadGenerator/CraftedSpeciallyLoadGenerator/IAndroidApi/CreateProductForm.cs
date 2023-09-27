namespace CraftedSpeciallyLoadGenerator.IAndroidApi;

public class CreateProductForm
{
    public string Name { get; set; }
    public string Description { get; set; }

    public static CreateProductForm Create(bool slow = false)
    {
        return new CreateProductForm
        {
            Name = slow ? "slow" + Guid.NewGuid() : Guid.NewGuid().ToString(),
            Description = Guid.NewGuid().ToString()
        };
    }

    private static readonly CreateProductForm FaultyForm = new()
    {
        Name = new string('a', 101),
        Description = Guid.NewGuid().ToString()
    };

    public static CreateProductForm Faulty() => FaultyForm;
}