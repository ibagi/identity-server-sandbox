using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityProvider.Entities;
using IdentityProvider.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityProvider.Pages.Login;

[SecurityHeaders]
[AllowAnonymous]
public class Index : PageModel
{
    private readonly IIdentityServerInteractionService _interaction;
    private readonly IUserService _userService;

    public ViewModel View { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public Index(IIdentityServerInteractionService interaction, IUserService userService)
    {
        _interaction = interaction;
        _userService = userService;
    }

    public async Task<IActionResult> OnGet(string returnUrl)
    {
        return await PageResult(returnUrl);
    }

    public async Task<IActionResult> OnPost()
    {
        // check if we are in the context of an authorization request
        var context = await _interaction.GetAuthorizationContextAsync(Input.ReturnUrl);

        // the user clicked the "cancel" button
        if (Input.Button != "login")
        {
            if (context != null)
            {
                // if the user cancels, send a result back into IdentityServer as if they 
                // denied the consent (even if this client does not require consent).
                // this will send back an access denied OIDC error response to the client.
                await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);

                // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                if (context.IsNativeClient())
                {
                    // The client is native, so this change in how to
                    // return the response is for better UX for the end user.
                    return this.LoadingPage(Input.ReturnUrl);
                }

                return Redirect(Input.ReturnUrl);
            }
            else
            {
                // since we don't have a valid context, then we just go back to the home page
                return Redirect("~/");
            }
        }

        if (!ModelState.IsValid)
        {
            // something went wrong, show form with error
            return await PageResult(Input.ReturnUrl);
        }

        var credentialsAreValid = await _userService.ValidateCredentialsAsync(Input.Username, Input.Password);

        // validate username/password against in-memory store
        if (!credentialsAreValid)
        {
            ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
            return await PageResult(Input.ReturnUrl);
        }

        var user = await _userService.GetByUsernameOrEmail(Input.Username);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, LoginOptions.InvalidCredentialsErrorMessage);
            return await PageResult(Input.ReturnUrl);
        }

        // only set explicit expiration here if user chooses "remember me". 
        // otherwise we rely upon expiration configured in cookie middleware.
        AuthenticationProperties props = null;

        if (LoginOptions.AllowRememberLogin && Input.RememberLogin)
        {
            props = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(LoginOptions.RememberMeLoginDuration)
            };
        };

        // issue authentication cookie with subject ID and username
        var isuser = new IdentityServerUser(user.Subject)
        {
            DisplayName = user.UserName
        };

        await HttpContext.SignInAsync(isuser, props);

        if (context != null)
        {
            if (context.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(Input.ReturnUrl);
            }

            // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
            return Redirect(Input.ReturnUrl);
        }

        // request for a local page
        if (Url.IsLocalUrl(Input.ReturnUrl))
        {
            return Redirect(Input.ReturnUrl);
        }
        else if (string.IsNullOrEmpty(Input.ReturnUrl))
        {
            return Redirect("~/");
        }
        else
        {
            // user might have clicked on a malicious link - should be logged
            throw new Exception("invalid return URL");
        }
    }

    private async Task<IActionResult> PageResult(string returnUrl)
    {
        Input = new InputModel
        {
            ReturnUrl = returnUrl
        };

        View = new ViewModel
        {
            AllowRememberLogin = LoginOptions.AllowRememberLogin,
        };

        return Page();
    }
}