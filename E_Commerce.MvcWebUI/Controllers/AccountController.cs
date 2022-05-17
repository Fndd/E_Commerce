using System; 
using System.Diagnostics;
using E_Commerce.MvcWebUI.Models; 
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;    
using Newtonsoft.Json;
using static E_Commerce.MvcWebUI.Models.FirebaseErrorModel;
using FireSharp.Response;
using E_Commerce.MvcWebUI.Entity;
using Newtonsoft.Json.Linq;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        FirebaseAuthProvider auth;
        DbContext dbcontext;
         
        public AccountController()
        {
            auth = new FirebaseAuthProvider(
                            new FirebaseConfig("AIzaSyCrwULS2U42COBROWYY4mq4lLokC6mGZ9c"));
            dbcontext = new DbContext();
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Registration(SignUpModel signUpModel)
        {
            try
            { 
                //Kullanıcının oluşturulması 
                await auth.CreateUserWithEmailAndPasswordAsync(signUpModel.Email, signUpModel.Password).ContinueWith(task =>
                {
                    if (task.IsFaulted)
                    {
                        ViewBag.Message = task.Exception.Message;
                    }
                    else
                    {
                        // Firebase user has been created.
                        Firebase.Auth.User newUser = task.Result.User;
                    }
                });

                //Kullanıcının diğer bilgierinin veri tabanına aktarılması
                PushResponse responses = dbcontext.client.Push("User/", signUpModel);
                signUpModel.Id = responses.Result.name;
                signUpModel.Role = "customer";
                SetResponse setResponse = dbcontext.client.Set("User/" + signUpModel.Id, signUpModel);

                //yeni kullanıcının logunun tutulması yani kullanıcı girişi yaparak oluşturulan tokeni alıyoruz.
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(signUpModel.Email, signUpModel.Password);
                string token = fbAuthLink.FirebaseToken;
                //token için session oluşturuluyor
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("_UserId", responses.Result.name); 
                    
                    return RedirectToAction("Index","Home");
                }
            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(signUpModel);
            }

            return View();

        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel loginModel)
        {
            try
            {
                //log in an existing user
                var fbAuthLink = await auth
                                .SignInWithEmailAndPasswordAsync(loginModel.Email, loginModel.Password);
                string token = fbAuthLink.FirebaseToken;

                FirebaseResponse response = dbcontext.client.Get("User");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Entity.User>();
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Entity.User>(((JProperty)item).Value.ToString()));
                }
                 var usr = list.Where(x => x.Email == loginModel.Email && x.Password == loginModel.Password).FirstOrDefault();

                //save the token to a session variable
                if (token != null)
                {
                    HttpContext.Session.SetString("_UserToken", token);
                    HttpContext.Session.SetString("_UserId", usr.Id);
                    HttpContext.Session.SetString("_UserName", usr.Name+" "+usr.Surname); 
                    HttpContext.Session.SetString("_UserRole", usr.Role); 

                    return RedirectToAction("Index","Home");
                }

            }
            catch (FirebaseAuthException ex)
            {
                var firebaseEx = JsonConvert.DeserializeObject<FirebaseError>(ex.ResponseData);
                ModelState.AddModelError(String.Empty, firebaseEx.error.message);
                return View(loginModel);
            }

            return View();
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("SignIn","Account");
        }
    }
}