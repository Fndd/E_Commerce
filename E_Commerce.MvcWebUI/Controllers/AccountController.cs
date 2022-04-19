using Microsoft.AspNetCore.Mvc;
using E_Commerce.MvcWebUI.Entity; 
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;
using System.Text.Json.Serialization;
using Firebase.Auth;
using E_Commerce.MvcWebUI.Models;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        FirebaseAuthProvider auth;
        public AccountController()
        {
            auth = new FirebaseAuthProvider(
                            new FirebaseConfig("AIzaSyCrwULS2U42COBROWYY4mq4lLokC6mGZ9c"));

        }
        public IActionResult SignIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SignIn(LoginModel userModel)
        {
            //log in the user
            var fbAuthLink = await auth
                            .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            string token = fbAuthLink.FirebaseToken;
            //saving the token in a session variable
            if (token != null)
            {
                HttpContext.Session.SetString("_UserToken", token);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        public IActionResult LogOut()
        {
            HttpContext.Session.Remove("_UserToken");
            return RedirectToAction("SignIn");
        }
        public IActionResult Register()
        { 
        return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(SignUpModel userModel)
        {
            //create the user
            await auth.CreateUserWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            //log in the new user
            var fbAuthLink = await auth
                            .SignInWithEmailAndPasswordAsync(userModel.Email, userModel.Password);
            string token = fbAuthLink.FirebaseToken;
            //saving the token in a session variable
            if (token != null)
            {
                HttpContext.Session.SetString("_UserToken", token);

                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

    }
}
