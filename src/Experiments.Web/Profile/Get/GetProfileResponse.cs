namespace Experiments.Web.Profile.Get;

public class GetProfileResponse
{
  public int Id { get; set; }
  public string? UserName { get; set; }
  public string PhoneNumber { get; set; } = default!;
  public DateTimeOffset LastLoginAt { get; set; }
}
