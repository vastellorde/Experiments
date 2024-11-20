﻿namespace Experiments.Core.IdentityAggregate;

public class Jwt
{
  public string AccessToken { get; set; } = string.Empty;
  public string RefreshToken { get; set; } = string.Empty;
  public DateTime AccessTokenExpiry { get; set; }
  public DateTime RefreshTokenExpiry { get; set; }
}
