using Newtonsoft.Json;
using Project.BLL.Dtos.Account;
using System.Net.Http.Headers;
using System.Text;

namespace Project.MVC.Services
{
    public class UserApiServices
    {
        private readonly HttpClient _httpClient;

        public UserApiServices()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7276/Api/v1/Account/users/")
            };
        }

        public async Task<List<GetAllUsersDto>> GetUsersAsync()
        {
            var response = await _httpClient.GetAsync("List");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GetAllUsersDto>>(json);
        }

        public async Task<string> AddUserAsync(RegisterDto registerDto)
        {
            var json = JsonConvert.SerializeObject(registerDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");


            var response = await _httpClient.PostAsync("Register?flag=true", content);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync();

                return errorContent;
            }
            else
            {

                return "Success";

            }



        }



        public async Task<bool> ChangePassword([FromBody] ChangePasswordDto model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("changePassword", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HttpRequestException: {httpEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }
    }
}

