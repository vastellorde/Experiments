namespace Experiments.UseCases.Profile;

public class UserDto
{
  public int Id { get; set; }
  public string PhoneNumber { get; set; } = default!;
  public string? UserName { get; set; }
  public DateTimeOffset LastLoginAt { get; set; }
}
