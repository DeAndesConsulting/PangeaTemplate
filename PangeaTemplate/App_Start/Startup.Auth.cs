using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using Owin;
using PangeaTemplate.Helpers.OAuth2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace PangeaTemplate
{
	public partial class Startup
	{
		public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        //private static string PUBLIC_CLIENT_ID = "self";
        public static string PublicClientId { get; private set; }

        public void ConfigureAuth(IAppBuilder app)
        {
            //Habilito a la app para usar las cookies y almacenar la información firmada por el usuario
            //y uso la cookie temporalmente 
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                LogoutPath = new PathString("/Account/LogOff"),
                ExpireTimeSpan = TimeSpan.FromMinutes(5.0),
            });
            PublicClientId = "self";
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            OAuthOptions = new OAuthAuthorizationServerOptions()
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new PangeaOAuthProvider(PublicClientId),
                AuthorizeEndpointPath = new PathString("/Account/ExternalLogin"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(5),
                AllowInsecureHttp = true //Don't do this in production ONLY FOR DEVELOPING: ALLOW INSECURE HTTP!  
            };

            // Enable the application to use bearer tokens to authenticate users  
            app.UseOAuthBearerTokens(OAuthOptions);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.  
            app.UseTwoFactorSignInCookie(
                DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.  
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.  
            // This is similar to the RememberMe option when you log in.  
            app.UseTwoFactorRememberBrowserCookie(
                DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers  
            //app.UseMicrosoftAccountAuthentication(  
            //    clientId: "",  
            //    clientSecret: "");  

            //app.UseTwitterAuthentication(  
            //   consumerKey: "",  
            //   consumerSecret: "");  

            //app.UseFacebookAuthentication(  
            //   appId: "",  
            //   appSecret: "");  

            //app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()  
            //{  
            //    ClientId = "",  
            //    ClientSecret = ""  
            //});  

        }


    }
}