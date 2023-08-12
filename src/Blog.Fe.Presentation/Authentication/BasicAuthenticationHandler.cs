using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;

namespace Blog.Fe.Presentation.Authentication;

internal sealed class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
{
    private const char Separator = ':';

    private readonly record struct Credentials(string UserName, string Password);

    public BasicAuthenticationHandler(
        IOptionsMonitor<BasicAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (!Request.Headers.TryGetValue(HeaderNames.Authorization, out var authorizationHeaders))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var encodedCredentials = GetEncodedCredentials(authorizationHeaders);
            if (encodedCredentials.Count == 0)
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var verifiedCredentials = GetVerifiedCredentials(encodedCredentials);
            if (!verifiedCredentials.HasValue)
            {
                Logger.LogInformation("Authentication failed: invalid userName or password");
                return Task.FromResult(AuthenticateResult.Fail("Invalid userName or password"));
            }

            var principal = CreatePrincipal(verifiedCredentials.Value, ClaimsIssuer);
            var authenticationTicket = new AuthenticationTicket(principal, BasicAuthenticationDefaults.AuthenticationScheme);

            return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Authentication failed: unexpected exception occured");
            return Task.FromResult(AuthenticateResult.Fail("Unexpected exception occured"));
        }
    }

    private static IReadOnlyCollection<string> GetEncodedCredentials(StringValues authorizationHeaders)
    {
        var encodedCredentials = new List<string>();

        var schemeToken = $"{BasicAuthenticationDefaults.AuthenticationScheme} ";
        foreach (var authorizationHeader in authorizationHeaders)
        {
            if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith(schemeToken))
            {
                encodedCredentials.Add(authorizationHeader[schemeToken.Length..]);
            }
        }

        return encodedCredentials;
    }

    private Credentials? GetVerifiedCredentials(IReadOnlyCollection<string> encodedCredentials)
    {
        var expectedCredentials = new Credentials(Options.UserName, Options.Password);
        foreach (var encodedItem in encodedCredentials)
        {
            try
            {
                var actualCredentials = DecodeCredentials(encodedItem);
                if (VerifyCredentials(actualCredentials, expectedCredentials))
                {
                    return actualCredentials;
                }
            }
            catch (InvalidOperationException ex)
            {
                Logger.LogInformation(ex.Message);
            }
        }

        return null;
    }

    private static Credentials DecodeCredentials(string encodedCredentials)
    {
        var decodedCredentials = Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));

        var separatorPosition = decodedCredentials.IndexOf(Separator);
        if (separatorPosition == -1)
            throw new InvalidOperationException($"Invalid Authorization header: Missing separator character '{Separator}'.");

        return new Credentials(decodedCredentials[..separatorPosition], decodedCredentials[(separatorPosition + 1)..]);
    }

    private static bool VerifyCredentials(Credentials actual, Credentials expected) =>
            string.Equals(actual.UserName, expected.UserName, StringComparison.InvariantCulture) &&
            string.Equals(actual.Password, expected.Password, StringComparison.InvariantCulture);

    private static ClaimsPrincipal CreatePrincipal(Credentials credentials, string? issuer)
    {
        var claims = new Claim[]
        {
            new Claim(ClaimTypes.Name, credentials.UserName, ClaimValueTypes.String, issuer)
        };
        var identity = new ClaimsIdentity(claims, BasicAuthenticationDefaults.AuthenticationScheme);
        return new ClaimsPrincipal(identity);
    }
}