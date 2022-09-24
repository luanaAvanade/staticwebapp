using Gicaf.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public static class KeyStore
{
    public static string Key =>
    @"{""Modulus"":""v5yix9AdUeKVUhDwT2+L0lBWNURuNrEZ8gLbgLPV3uioQRvmri1v4fKZfsyjMBYlTyzfB6D87EVXljVrti/i42Jsa2gxireV6qluaqzZhJL5inigd7CKeS+2wV+wzmbLvguVoIgXsNOhlEMUmKakUm0HUm1TEMdskeHCSd25xgAMY9a/xWQXYVe6s68+FVY7hrJdZtUgSFuul5387kRRKYz6Gxs2PERBYBmilt5ZB/xeRko8tXHX5kt1SGp4bEfamuTm2gxHYWDcc48/FccoQ4ksEQWHI3k4Hx3Hq4MWtpiYz2k84rwRDZj9+lAX/kz852GuKNp0wvRVqpjG03w5ZQ=="",""Exponent"":""AQAB"",""P"":""5M665LwapzagEM9gn3CNpn9Klj/krbV8oC390wK2E/LmVy1zFaJ8Maxp9zVQVMaHmUNOTuSYg/SyaaxGL7RdYQ6ICOAivblpu0QYAHr4QLBTW8qfcEpcF1RK6i2D47EWZ3DWrD/ZiruAsvF0nJmgYZsqFUsbZlKCiminlgDajyc="",""Q"":""1mJCeJGyTL0RIDFN9NQF/yNzwU26EI4S4eDd3rYp20t+D6W75GaD1kEVUFcjUsuRhPd6cshtzA9tAnDm5GHwmKH5zSK4ykH0t+WVYrhcWLuOSj7jlFdGct8KsyJXydxI23SnL405go+17bgOPffm1y8SXOgrmO8bT8icW0dHipM="",""DP"":""aYUti+90G0mF6Dq7RMyThwWNUF2HCNV59CVBud62Odz7fZjoSkH1JLNu6yMbOP2V64iuxJOIAtMGtNVOSRHVPr2E3xL+8qBfso1kxFPxyWOIDf0UKqUgc2HcasMy3/77ZhkT57xKh0CRyfSw4se2v0zfy5InwLism0hIgVVDdmE="",""DQ"":""WEKJxSOOeUNZWpyR0Jq9gUWyMUHzuT8UjQPLtBXg85SH6J891JFT85BF2xaUx1Bhr5FHSxwy/9DV+yxKEnoz2FLPCeIim7o9qrwNHWBzPGCy+uQbQ4YOzdn/iLI53DQhlp/W+J+2robFyoF884ws8ChD8hXvZP4Di3w2yul0jZs="",""InverseQ"":""XHHfSX3piY4y6veJyNkZojUgH/tjd0HZpq2vFgoqQapYPVvTFmb5v08pJYveKVAbq5DUS3HGscNdvwhqj6OPbFaUqQ6YhIjk6eGcFmogDtt27WfPeI1bLA84x71smo4gSoP8xJGZC+kGAD5FS1zjCjsE9wS6Q5dKh7KMlTFRMXM="",""D"":""CTNxUZz5u3Pg7bbogzf9A9VdvmMR08h/UXK3rZ91FqYfZjj7w8NkyvnGSyNydp2pnvqtnwUpAAN4aDsad/4mq/qO8D1pm5qjNUh7h93S+B3Co9aVBgpRK+6RnQmGSPMAt5iAz9uZH6GZ8a+i7o2Z+GdJ4bhkM7sPyJWJqdhe0lYa+hEcPFfimKJVnGXcwayGXBLcsQh+fnJ7dPm3eNHgLdWd9OcwINoW1Bz7YC2tZdbKvYKzML1RIhgxPIVPKx27oiWtqlep6TEWczjIQUB1AYAG3Jd0wt1XLqpG9m28jAE30eq+l7as+omUp4ujdnSsrSWbknhJPTO07CeZtX5XZQ==""}";
}

namespace Gicaf.API
{
    public class Login
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string LinkConfirmacao { get; set; }
        public ICollection<string> Roles { get; set; }
    }

    public class ResetPassword
    {
        public string Id { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string Token { get; set; }
    }

    public class EmailConfirmation
    {
        public string UserName { get; set; }
        public string Token { get; set; }
    }


    public static class Roles
    {
        public const string ROLE_API_GICAF = "Acesso-API-Gicaf";
    }

    public class TokenConfigurations
    {
        public string Audience { get; set; }
        public string Issuer { get; set; }
        public int Seconds { get; set; }
    }

    public class SigningConfigurations
    {
        public SecurityKey Key { get; }
        public SigningCredentials SigningCredentials { get; }
        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                provider.FromJsonString(KeyStore.Key);
                Key = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(
                Key, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}

internal class RSAParametersJson
{
    //Public key Modulus
    public string Modulus { get; set; }
    //Public key Exponent
    public string Exponent { get; set; }

    public string P { get; set; }

    public string Q { get; set; }

    public string DP { get; set; }

    public string DQ { get; set; }

    public string InverseQ { get; set; }

    public string D { get; set; }
}

internal static class RSAKeyExtensions
{
    #region JSON
    internal static void FromJsonString(this RSA rsa, string jsonString)
    {
        //Check.Argument.IsNotEmpty(jsonString, nameof(jsonString));
        try
        {
            var paramsJson = JsonConvert.DeserializeObject<RSAParametersJson>(jsonString);

            RSAParameters parameters = new RSAParameters();

            parameters.Modulus = paramsJson.Modulus != null ? Convert.FromBase64String(paramsJson.Modulus) : null;
            parameters.Exponent = paramsJson.Exponent != null ? Convert.FromBase64String(paramsJson.Exponent) : null;
            parameters.P = paramsJson.P != null ? Convert.FromBase64String(paramsJson.P) : null;
            parameters.Q = paramsJson.Q != null ? Convert.FromBase64String(paramsJson.Q) : null;
            parameters.DP = paramsJson.DP != null ? Convert.FromBase64String(paramsJson.DP) : null;
            parameters.DQ = paramsJson.DQ != null ? Convert.FromBase64String(paramsJson.DQ) : null;
            parameters.InverseQ = paramsJson.InverseQ != null ? Convert.FromBase64String(paramsJson.InverseQ) : null;
            parameters.D = paramsJson.D != null ? Convert.FromBase64String(paramsJson.D) : null;
            rsa.ImportParameters(parameters);
        }
        catch
        {
            throw new Exception("Invalid JSON RSA key.");
        }
    }

    internal static string ToJsonString(this RSA rsa, bool includePrivateParameters)
    {
        RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        var parasJson = new RSAParametersJson()
        {
            Modulus = parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
            Exponent = parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
            P = parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
            Q = parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
            DP = parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
            DQ = parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
            InverseQ = parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
            D = parameters.D != null ? Convert.ToBase64String(parameters.D) : null
        };

        return JsonConvert.SerializeObject(parasJson);
    }
    #endregion

    #region XML

    public static void FromCoreXmlString(this RSA rsa, string xmlString)
    {
        RSAParameters parameters = new RSAParameters();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlString);

        if (xmlDoc.DocumentElement.Name.Equals("RSAKeyValue"))
        {
            foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
            {
                switch (node.Name)
                {
                    case "Modulus": parameters.Modulus = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "Exponent": parameters.Exponent = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "P": parameters.P = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "Q": parameters.Q = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "DP": parameters.DP = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "DQ": parameters.DQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "InverseQ": parameters.InverseQ = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                    case "D": parameters.D = (string.IsNullOrEmpty(node.InnerText) ? null : Convert.FromBase64String(node.InnerText)); break;
                }
            }
        }
        else
        {
            throw new Exception("Invalid XML RSA key.");
        }

        rsa.ImportParameters(parameters);
    }

    public static string ToCoreXmlString(this RSA rsa, bool includePrivateParameters)
    {
        RSAParameters parameters = rsa.ExportParameters(includePrivateParameters);

        return string.Format("<RSAKeyValue><Modulus>{0}</Modulus><Exponent>{1}</Exponent><P>{2}</P><Q>{3}</Q><DP>{4}</DP><DQ>{5}</DQ><InverseQ>{6}</InverseQ><D>{7}</D></RSAKeyValue>",
              parameters.Modulus != null ? Convert.ToBase64String(parameters.Modulus) : null,
              parameters.Exponent != null ? Convert.ToBase64String(parameters.Exponent) : null,
              parameters.P != null ? Convert.ToBase64String(parameters.P) : null,
              parameters.Q != null ? Convert.ToBase64String(parameters.Q) : null,
              parameters.DP != null ? Convert.ToBase64String(parameters.DP) : null,
              parameters.DQ != null ? Convert.ToBase64String(parameters.DQ) : null,
              parameters.InverseQ != null ? Convert.ToBase64String(parameters.InverseQ) : null,
              parameters.D != null ? Convert.ToBase64String(parameters.D) : null);
    }

    #endregion
}



public class JwtAuthorizationPolicyBuilder : AuthorizationPolicyBuilder
{
    public JwtAuthorizationPolicyBuilder()
    {
    }

    public AuthorizationPolicy Configure()
    {
        return AddRequirements(new CheckPermission())
               .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
               .RequireAuthenticatedUser().Build();
    }
}

public class ModulePermission
{
    public Guid Id { get; set; }
    public bool C { get; set; }
    public bool R { get; set; }
    public bool U { get; set; }
    public bool D { get; set; }
}


public class CheckPermission : IAuthorizationHandler, IAuthorizationRequirement
{
    public Task HandleAsync(AuthorizationHandlerContext context)
    {
        var mvcContext = context.Resource as Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext;

        //var memoryStream = new MemoryStream();
        //mvcContext.HttpContext.Request.Body.CopyTo(memoryStream);
        //memoryStream.Seek(0, SeekOrigin.Begin);
        ////using (var reader = new StreamReader(mvcContext.HttpContext.Request.Body))
        //using (var reader = new StreamReader(memoryStream))
        //{
        //    var body = reader.ReadToEnd();
        //    var gql = JsonConvert.DeserializeObject<GraphQLQuery>(body);
        //    memoryStream.Seek(0, SeekOrigin.Begin);
        //    memoryStream.CopyTo(mvcContext.HttpContext.Request.Body);
        //}

        if (!context.User.HasClaim(c => c.Type == "perm"))
        {
            return Task.CompletedTask;
        }
        var perm =  JsonConvert.DeserializeObject<List<ModulePermission>>(context.User.FindFirst(c => c.Type == "perm").Value);
        context.Succeed(this);
        return Task.CompletedTask;
    }
}