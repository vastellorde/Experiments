namespace Experiments.Web.Profile.Get;

public class GetProfileRequest
{
  public const string Route = "/profile/{Id}";

  public int Id { get; set; }
}
