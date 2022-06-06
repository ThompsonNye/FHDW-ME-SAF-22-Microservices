namespace Cars.Models.Entities;

/// <summary>
/// The identifier for a <see cref="Car"/> entity.
/// </summary>
[StronglyTypedId(jsonConverter: StronglyTypedIdJsonConverter.SystemTextJson)]
public partial struct CarId
{
}