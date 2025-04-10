using Newtonsoft.Json;
using Project.BLL.Dtos.Authorization;
using System.Net.Http.Headers;
using System.Text;

namespace Project.MVC.Services
{
    public class RolesServices
    {
        private readonly HttpClient _httpClient;
        public RolesServices()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7276/Api/v1/Authorization/")
            };
        }

        public async Task<List<GetAllRoles>> GetRolesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync("GetAllRoles");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<GetAllRoles>>(json);
        }
        public async Task<string> GetRoleByIdAsync(EditRoleDto model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(model);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync("/UpdateRole", content);
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<string>(json);
        }

        public async Task<bool> AddRoleAsync(AddRoleDto addroleDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var json = JsonConvert.SerializeObject(addroleDto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("AddRole", content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Error: {response.StatusCode}, Response: {error}");

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        Console.WriteLine("Validation failed. Check the request payload.");
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    {
                        Console.WriteLine("Server error occurred. Please try again later.");
                    }

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


        public async Task<ManageUserRoles> MangeRole(int userId, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"ManageUserRole?userId={userId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ManageUserRoles>(json);

        }
        public async Task<UpdateUserRole> UpdateUserRole(UpdateUserRole model, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("UpdateUserRole", jsonContent);

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UpdateUserRole>(json);

        }
        public async Task<string> RemoveRole(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync($"DeleteRole?id={id}");

            response.EnsureSuccessStatusCode();

            return "Success";

        }



    }
}
