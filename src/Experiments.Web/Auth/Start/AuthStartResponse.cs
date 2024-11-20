namespace Experiments.Web.Auth.Start;

public class AuthStartResponse(string confirmationId)
{
  public string ConfirmationId { get; set; } = confirmationId;
}
