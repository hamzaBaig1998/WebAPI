using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using WebApi.Models;
using System.Web.Mvc;

namespace WebApi.Controllers
{
    public class CommentConsumerController : Controller
    {
        // GET: CommentConsumer
        public ActionResult Index()
        {
            IEnumerable<comment> comments = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var responseTalk = client.GetAsync("comments");
                responseTalk.Wait();

                var result = responseTalk.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<comment>>();
                    readTask.Wait();

                    comments = readTask.Result;
                }
                else
                {
                    comments = Enumerable.Empty<comment>();
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(comments);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(comment c)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var postTask = client.PostAsJsonAsync<comment>("comments", c);
                postTask.Wait();

                var result = postTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return View(c);
        }
        public ActionResult Edit(int id)
        {
            comment c = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:50122/api/");
                var responseTalk = client.GetAsync("comments/" + id);
                responseTalk.Wait();

                var result = responseTalk.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<comment>();
                    readTask.Wait();

                    c = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error");
                }
            }
            return View(c);
        }
        [HttpPost]
        public ActionResult Edit(comment c)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:520122/api/");
                var putTask = client.PutAsJsonAsync<comment>("comments/" + c.Id, c);
                putTask.Wait();

                var result = putTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
            }
            ModelState.AddModelError(string.Empty, "Error");
            return View(c);
        }
        public ActionResult Delete(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:520122/api/");
                var deleteTask = client.DeleteAsync("comments/" + id);
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