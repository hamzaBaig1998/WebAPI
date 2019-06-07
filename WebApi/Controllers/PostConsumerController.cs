using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using WebApi.Models;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class PostConsumerController : Controller
    {
        // GET: PostConsumer
        public ActionResult Index()
        {
            IEnumerable<post> posts = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var responseTalk = client.GetAsync("posts");
                responseTalk.Wait();

                var result = responseTalk.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<post>>();
                    readTask.Wait();

                   posts = readTask.Result;
                }
                else
                {
                    posts = Enumerable.Empty<post>();
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(posts);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(post p)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var postTask = client.PostAsJsonAsync<post>("posts", p);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return View(p);
        }
        public ActionResult Edit(int id)
        {
            post p = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var responseTalk = client.GetAsync("posts/" + id);
                responseTalk.Wait();

                var result = responseTalk.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<post>();
                    readTask.Wait();

                    p = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(p);
        }
        [HttpPost]
        public ActionResult Edit(post p)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:520122/api/");
                var putTask = client.PutAsJsonAsync<post>("posts/" + p.postNumber, p);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return View(p);
        }
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:520122/api/");
                var deleteTask = client.DeleteAsync("posts/" + id);
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