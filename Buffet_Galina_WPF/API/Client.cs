using Buffet_Galina_WPF.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using static System.Net.Mime.MediaTypeNames;

namespace Buffet_Galina_WPF.API
{
    class Client
    {
        HttpClient httpClient = new HttpClient();
        public Client()
        {
            httpClient.BaseAddress = new Uri(@"https://localhost:7054/");
        }

        public async Task<AdminDTO> LoginAdmin (AdminDTO admin, string password)
        {

            using (var client = new HttpClient())
            {
                var loginAdmin = new LoginAdmin
                {
                    
                    Password = password
                };
                var jsonContent = JsonConvert.SerializeObject(loginAdmin);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("Admin/LoginAdmin", httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Неправильный пароль");
                }
                var answerUser = JsonConvert.DeserializeObject<AdminDTO>(await response.Content.ReadAsStringAsync());
                return answerUser;
            }
        }
        public async Task<List<DishDTO>> GetDish()
        
        {
            try
            {
                var response = await httpClient.GetAsync("Admin/GetDish");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<DishDTO>>(content);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine(ex.Message);
                return null;
            }            
        }

        public async Task<List<CategoryDTO>> GetCategories()
        {
            try
            {
                var response = await httpClient.GetAsync("Admin/GetCategories");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CategoryDTO>>(content);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<ProductDTO>> GetProducts()
        {
            try
            {
                var response = await httpClient.GetAsync("Admin/GetProducts");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<ProductDTO>>(content);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine(ex.Message);
                return null;
            }
        }


        public async Task<AdminDTO> GetAdmin(int id)
        {
            try
            {
                var response = await httpClient.GetAsync("Admin/GetAdmin");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<AdminDTO>(content);
                }
                else
                {
                    throw new Exception($"Error: {response.ReasonPhrase}");
                }
            }
            catch (Exception ex)
            {
                // Обработка исключений
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        //public async Task<List<CategoryDTO>> GetCategory()
        //{
        //    try
        //    {
        //        var response = await httpClient.GetAsync("ZType/GetZTypes");
        //        if (response.IsSuccessStatusCode)
        //        {
        //            var content = await response.Content.ReadAsStringAsync();
        //            return JsonConvert.DeserializeObject<List<CategoryDTO>>(content);
        //        }
        //        else
        //        {
        //            throw new Exception($"Error: {response.ReasonPhrase}");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Обработка исключений
        //        Console.WriteLine(ex.Message);
        //        return null;
        //    }
        //}

        public async Task AddDishAsync(DishDTO dish)
        {
            using (var client = new HttpClient())
            {
                var jsonContent = JsonConvert.SerializeObject(dish);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync("Admin/AddDish", httpContent);
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Не удалось добавить блюдо.");
                }
            }
        }


        public async Task<DishDTO> EditDish(DishDTO dish, int id)
        {
            using StringContent jsonContent = new(
                   System.Text.Json.JsonSerializer.Serialize(dish),
                   Encoding.UTF8,
                   "application/json");
            using HttpResponseMessage response = await httpClient.PutAsync("Admin/" + dish.Id, jsonContent);
            response.EnsureSuccessStatusCode();
            // MessageBox.Show(response.StatusCode.ToString());
            return dish;
        }
            

        public async Task DeleteDish(int id)
        {
            using HttpResponseMessage response = await httpClient.DeleteAsync("Admin/" + id);
            response.EnsureSuccessStatusCode();
           

        }




        static Client instance = new();
        public static Client Instance
        {
            get
            {
                if (instance == null)
                    instance = new Client();
                return instance;
            }
        }
    }
}
