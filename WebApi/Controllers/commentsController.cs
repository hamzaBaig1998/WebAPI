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
    public class commentsController : ApiController
    {
        private socialEntities db = new socialEntities();

        // GET: api/comments
        public IQueryable<comment> Getcomments()
        {
            return db.comments;
        }

        // GET: api/comments/5
        [ResponseType(typeof(comment))]
        public IHttpActionResult Getcomment(int id)
        {
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        // PUT: api/comments/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Putcomment(int id, comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != comment.Id)
            {
                return BadRequest();
            }

            db.Entry(comment).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!commentExists(id))
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

        // POST: api/comments
        [ResponseType(typeof(comment))]
        public IHttpActionResult Postcomment(comment comment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.comments.Add(comment);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (commentExists(comment.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = comment.Id }, comment);
        }

        // DELETE: api/comments/5
        [ResponseType(typeof(comment))]
        public IHttpActionResult Deletecomment(int id)
        {
            comment comment = db.comments.Find(id);
            if (comment == null)
            {
                return NotFound();
            }

            db.comments.Remove(comment);
            db.SaveChanges();

            return Ok(comment);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool commentExists(int id)
        {
            return db.comments.Count(e => e.Id == id) > 0;
        }
    }
}