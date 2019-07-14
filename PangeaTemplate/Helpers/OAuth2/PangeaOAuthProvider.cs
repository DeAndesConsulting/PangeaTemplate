using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace PangeaTemplate.Helpers.OAuth2
{
    public class PangeaOAuthProvider : OAuthAuthorizationServerProvider
    {
        private readonly string _publicClientId;

        //Esta deberia ser la conexión a la bd
        // private Oauth_APIEntities databaseManager = new Oauth_APIEntities();  
        private string dataBaseManager;

        public PangeaOAuthProvider(string publicClientId)
        {
            if (string.IsNullOrEmpty(publicClientId))
                throw new ArgumentNullException(nameof(publicClientId));

            _publicClientId = publicClientId;
        }

        public override async Task GrantResourceOwnerCredentials(
            OAuthGrantResourceOwnerCredentialsContext context)
        {
            //Inicialización
            string usernameVal = context.UserName;
            string passwordVal = context.Password;

            //var user = this.databaseManager.LoginByUsernamePassword(usernameVal, passwordVal).ToList();
            //var user = this.dataBaseManager.ToString();
            List<Usuario> lista = new List<Usuario>();
                lista.Add(new Usuario() { UserName = "Pepe", Password = "sarasa" });

            var user = lista;

            //Verificación
            if (user == null || user.Count() <= 0)
            {
                //Configuracion errro
                context.SetError("invalid_grant", "Usuario o password incorrecto");
                //Retorna info
                return;
            }

            //Inicialización
            var claims = new List<Claim>();
            var userInfo = user.FirstOrDefault();

            //Setting
            //claims.Add(new Claim(ClaimTypes.Name, userInfo.userName))
            claims.Add(new Claim(ClaimTypes.Name, userInfo.UserName));

            //Configuracion de identity claim para el protocolo oauth2
            ClaimsIdentity oAuthClaimIdentity = 
                new ClaimsIdentity(claims, OAuthDefaults.AuthenticationType);

            ClaimsIdentity cookiesClaimIdentity = 
                new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationType);

            //Configuración de autenticación de usuario
            AuthenticationProperties properties = CreateProperties(userInfo.UserName);
            AuthenticationTicket ticket = 
                new AuthenticationTicket(oAuthClaimIdentity, properties);

            //Grant access to authorize user
            context.Validated(ticket);
            context.Request.Context.Authentication.SignIn(cookiesClaimIdentity);
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            //Add properties 
            foreach (KeyValuePair<string, string> prop in context.Properties.Dictionary)
                context.AdditionalResponseParameters.Add(prop.Key, prop.Value);

            //Return info
            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context) {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task ValidateClientRedirectUri(OAuthValidateClientRedirectUriContext context)
        {
            //verificación
            if (context.ClientId == _publicClientId)
            {
                //inicialización
                Uri expectedRootUri = new Uri(context.Request.Uri, "/");

                //verificación
                if (expectedRootUri.AbsoluteUri == context.RedirectUri)
                    context.Validated(); //validando
            }

            //retorno información
            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName)
        {
            //Configuraciones
            IDictionary<string, string> data = 
                new Dictionary<string, string>
                {
                    { "userName", userName}
                };

            //retorna info
            return new AuthenticationProperties(data);
        }

    }

    public class Usuario
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}