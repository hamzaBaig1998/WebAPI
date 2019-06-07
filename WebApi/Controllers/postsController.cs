using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class postsController : ApiController
    {
        private socialEntities db = new socialEntities();

        // GET: api/posts
        public IQueryable<post> Getposts()
        {
            return db.posts;
        }

        // GET: api/posts/5
        [ResponseType(typeof(post))]
        public IHttpActionResult Getpost(int id)
        {
            post post = db.posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post);
        }

        // PUT: api/posts/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putpost(int id, post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != post.postNumber)
            {
                return BadRequest();
            }

            db.Entry(post).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!postExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/posts
        [ResponseType(typeof(post))]
        public IHttpActionResult Postpost(post post)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.posts.Add(post);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = post.postNumber }, post);
        }

        // DELETE: api/posts/5
        [ResponseType(typeof(post))]
        public IHttpActionResult Deletepost(int id)
        {
            post post = db.posts.Find(id);
            if (post == null)
            {
                return NotFound();
            }

            db.posts.Remove(post);
            db.SaveChanges();

            return Ok(post);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool postExists(int id)
        {
            return db.posts.Count(e => e.postNumber == id) > 0;
        }
    }
}