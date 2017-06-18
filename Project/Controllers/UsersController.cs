using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Project.Models;
using Newtonsoft.Json.Linq;

namespace Project.Controllers
{
    public class UsersController : ApiController
    {
        private ProjectContext db = new ProjectContext();

        // GET: api/Users
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        //// GET: api/Users/5
        //[ResponseType(typeof(User))]
        //public async Task<IHttpActionResult> GetUser(string id)
        //{
        //    User user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(user);
        //}

        //// PUT: api/Users/5
        //[ResponseType(typeof(void))]
        //public async Task<IHttpActionResult> PutUser(string id, User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != user.UserName)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        // POST: api/Users
        //[ResponseType(typeof(User))]
        //public async Task<IHttpActionResult> PostUser(User user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Users.Add(user);

        //    try
        //    {
        //        await db.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (UserExists(user.UserName))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = user.UserName }, user);
        //}

        // DELETE: api/Users/5
        //[ResponseType(typeof(User))]
        //public async Task<IHttpActionResult> DeleteUser(string id)
        //{
        //    User user = await db.Users.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Users.Remove(user);
        //    await db.SaveChangesAsync();

        //    return Ok(user);
        //}

        [HttpGet]
        [ResponseType(typeof(string))]
        public IHttpActionResult Register(string username, string password, string email)
        {
            JObject solveObj;
            if (!UserExists(username))
            {
                User user = new User(username, password, email);
                db.Users.Add(user);
                try
                {
                    db.SaveChangesAsync();
                    solveObj = new JObject
                    {
                        ["ans"] = "1"
                    };
                }
                catch (DbUpdateException)
                {
                    if (UserExists(user.UserName))
                    {
                        return Conflict();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            else
            {
                solveObj = new JObject
                {
                    ["ans"] = "0"
                };
                //return Ok("0");
            }
            return Ok(solveObj);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserName == id) > 0;
        }
    }
}