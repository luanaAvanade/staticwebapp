using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Gicaf.API.Authentication
{
    namespace HttpClientSample
    {
        public class UserRegister
        {
            public string UserName { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string LinkConfirmacao { get; set; }
        }

        public static class AuthenticatorSettings
        {
            public static string UrlBase { get; set; }
        }
        
        public class AuthenticatorClient
        {
            HttpClient _client = new HttpClient();

            public async Task<object> RegisterUserAsync(UserRegister user)
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync(
                    "api/register", user);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<object>();
            }

            public async Task<object> LoginAsync(Login user)
            {
                HttpResponseMessage response = null;
                try
                {
                    response = await _client.PostAsJsonAsync(
                    "api/login", user);
                    response.EnsureSuccessStatusCode();
                }
                catch(Exception e)
                {
                    throw new Exception(string.Format("{0}: {1}", _client.BaseAddress+"api/login", e.Message));
                }                

                return await response.Content.ReadAsAsync<object>();
            }

            public async Task<object> ResetPassword(Login user)
            {

                HttpResponseMessage response = await _client.PostAsJsonAsync(
                    "api/Password/Reset", user);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<object>();
            }

            public async Task<object> ChangePassword(ResetPassword user, HttpRequest request)
            {

                HttpResponseMessage response = await _client.PostAsJsonAsync(
                    "api/Password/Change", user);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<object>();
            }

            public async Task<object> EmailConfirmationValidation(EmailConfirmation user, HttpRequest request)
            {

                HttpResponseMessage response = await _client.PostAsJsonAsync(
                    "api/User/EmailConfirmationValidation", user);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsAsync<object>();
            }

            public AuthenticatorClient(string baseUrl)
            {
                // Update port # in the following line.
                _client.BaseAddress = new Uri(baseUrl);
                _client.DefaultRequestHeaders.Accept.Clear();
                _client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }

            private void SetAuthentication(HttpRequest request)
            {
                var token = request.Headers["Authorization"].ToString();
                _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token);
            }

            public async Task<List<string>> GetUsersInRole(string roleName)
            {
                HttpResponseMessage response = await _client.GetAsync(../../"api/User/GetUsersInRole/{roleName}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsAsync<List<string>>();
            }
        }
    }
}
