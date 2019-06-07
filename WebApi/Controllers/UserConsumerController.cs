using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class UserConsumerController : Controller
    {
        // GET: UserConsumer
        public ActionResult Index()
        {
            IEnumerable<user> users = null;
            using(var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var responseTalk = client.GetAsync("users");
                responseTalk.Wait();

                var result = responseTalk.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<user>>();
                    readTask.Wait();

                    users = readTask.Result;
                }
                else
                {
                    users = Enumerable.Empty<user>();
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(users);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(user u)
        {
            using(var client =new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var postTask = client.PostAsJsonAsync<user>("users", u);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return View(u);
        }
        public ActionResult Edit(int id)
        {
            user u = null;
            using(var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var responseTalk = client.GetAsync("users/" + id);
                responseTalk.Wait();

                var result = responseTalk.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<user>();
                    readTask.Wait();

                    u = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(u);
        }
        [HttpPost]
        public ActionResult Edit(user u)
        {
            using(var client=new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:520122/api/");
                var putTask = client.PutAsJsonAsync<user>("users/" + u.userId, u);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return View(u);
        }
        public ActionResult Delete(int id)
        {
            using(var client=new HttpClient())
            {
                client.BaseAddress=new Uri("http://localhost:520122/api/");
                var deleteTask = client.DeleteAsync("users/" + id);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return RedirectToAction("Index");
        }
    }
}