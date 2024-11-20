namespace Experiments.Core.SessionAggregate;

public class Session : EntityBase<Guid>, IAggregateRoot
{
  public SessionPlatform Platform { get; set; }
  public DateTimeOffset CreatedAt { get; set; }
}

public enum SessionPlatform
{
  Android,
  Ios,
  Web,
  Pc,
  Unknown
}

public static class SessionPlatformExtensions
{
  public static SessionPlatform ToSessionPlatform(this string platform)
  {
    return platform switch
    {
      "Android" => SessionPlatform.Android,
      "Ios" => SessionPlatform.Ios,
      "Pc" => SessionPlatform.Pc,
      "Web" => SessionPlatform.Web,
      _ => SessionPlatform.Unknown
    };
  }
}
