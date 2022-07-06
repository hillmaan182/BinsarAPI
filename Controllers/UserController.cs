using Microsoft.AspNetCore.Mvc;
using BinsarAPI.Models;
using Newtonsoft.Json;
using System.Text;

namespace BinsarAPI.Controllers
{
    public class UserController : Controller
    {
        private string endPointCreate = "https://gorest.co.in/public/v1/users";
        private string endPointRead = "https://gorest.co.in/public/v1/users?page=1";
        private string endPointDel = "https://gorest.co.in/public/v1/users/";
        private string endPointUpd = "https://gorest.co.in/public/v1/users/";
        private string accessToken = "ff204d63332ec73ee6b9256320e985acae910bb57263977debf581bfd96b3169";


        public class DataDTO
        {
            public int id { get; set; }
            public string name { get; set; }
            public string email { get; set; }
            public string gender { get; set; }
            public string status { get; set; }

           
        }


        public class UserDTO
        {
            public object meta { get; set; }
            public DataDTO data { get; set; }
            
        }
       
        public class RespDTO
        {
            public object meta { get; set; }
            public  List<MsgDTO> data  { get; set; }
        }

        public class MsgDTO
        {
            public string field { get; set; }
            public string message { get; set; }
        }

        public async Task<IActionResult> Index()
        {
            
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<User> usrList = new List<User>();
            var response = await httpClient.GetAsync(endPointRead);
            string resp = await response.Content.ReadAsStringAsync();
           
            User obj = JsonConvert.DeserializeObject<User>(resp);
          
            return View(obj);
        }

        [HttpPost]
        public async Task<IActionResult> Next(string next)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<User> usrList = new List<User>();
            var response = await httpClient.GetAsync(next);
            string resp = await response.Content.ReadAsStringAsync();

            User obj = JsonConvert.DeserializeObject<User>(resp);

            return View("Index",obj);

        }


        [HttpPost]
        public async Task<IActionResult> Prev(string prev)
        {

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            List<User> usrList = new List<User>();
            var response = await httpClient.GetAsync(prev);
            string resp = await response.Content.ReadAsStringAsync();

            User obj = JsonConvert.DeserializeObject<User>(resp);

            return View("Index", obj);

        }

        [HttpPost]
        public async Task<IActionResult> AddUser(Data data)
        {
            try
            {

                User objUser = new User();
                data.status = "active";
                var httpClient = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var req = await httpClient.PostAsync(endPointCreate, content);
                var msg = "";
                if (req.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    string apiReq = await req.Content.ReadAsStringAsync();
                    RespDTO reqObj = JsonConvert.DeserializeObject<RespDTO>(apiReq);

                    foreach(var x in reqObj.data)
                    {
                        var error = x.field.ToString() + ": " + x.message.ToString() + " , ";
                        msg += error;
                    }
                }
                else
                {
                    msg = "Success Add User";
                   
                }

                var response = await httpClient.GetAsync(endPointCreate);
                string resp = await response.Content.ReadAsStringAsync();

                ViewBag.Result = msg;

                User obj = JsonConvert.DeserializeObject<User>(resp);
                return View("Index", obj);

            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> DelUser(int id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                using (var response = await httpClient.DeleteAsync(endPointDel + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                }
            }

            return RedirectToAction("Index");
        }

        #region "Update User"
        public async Task<IActionResult> Update(int id)
        {
            Data data = new Data();
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                using (var response = await httpClient.GetAsync( endPointUpd + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    UserDTO usrDTO = JsonConvert.DeserializeObject<UserDTO>(apiResponse);
                    data.id = id;
                    data.name = usrDTO.data.name;
                    data.email = usrDTO.data.email;
                    data.status = usrDTO.data.status;
                }
            }
            return View("Update", data);
        }

        //Update
        [HttpPost]
        public async Task<IActionResult> Update(Data data)
        {
            Data datas = new Data();
            using (var httpClient = new HttpClient())
            {
                //token bearer
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
                var content = new MultipartFormDataContent();
                content.Add(new StringContent(data.name), "name");
                content.Add(new StringContent(data.email), "email");
                content.Add(new StringContent(data.status), "status");

                using (var response = await httpClient.PutAsync(endPointUpd + data.id, content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    ViewBag.Result = "Success";
                    UserDTO updUsr = JsonConvert.DeserializeObject<UserDTO>(apiResponse);
                    datas.id = updUsr.data.id;
                    datas.name = updUsr.data.name;
                    datas.email = updUsr.data.email;
                    datas.status = updUsr.data.status;
                }
            }
            return View(datas);
        }

        #endregion
    }
}
