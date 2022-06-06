namespace Cars.Models.Entities;

public class Car
{
    public CarId Id { get; set; } = CarId.New();

    public string Name { get; set; } = null!;
}