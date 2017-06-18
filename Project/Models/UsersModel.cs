//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Web.Http;
//using System.Web.Http.Description;
//using Project.Models;

//namespace Project.Models
//{
//    public class UsersModel
//    {
//        private ProjectContext db = new ProjectContext();

//        public User GetUser(string username)
//        {
//            return db.Users.Find(username);
//        }

//        public void PutUser(string id, User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            if (id != user.UserName)
//            {
//                return BadRequest();
//            }

//            db.Entry(user).State = EntityState.Modified;

//            try
//            {
//                await db.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!UserExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return StatusCode(HttpStatusCode.NoContent);
//        }

//        // POST: api/Users
//        [ResponseType(typeof(User))]
//        public async Task<IHttpActionResult> PostUser(User user)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            db.Users.Add(user);

//            try
//            {
//                await db.SaveChangesAsync();
//            }
//            catch (DbUpdateException)
//            {
//                if (UserExists(user.Password))
//                {
//                    return Conflict();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return CreatedAtRoute("DefaultApi", new { id = user.Password }, user);
//        }

//        [ResponseType(typeof(void))]
//        public async Task<IHttpActionResult> Register(string username, string password,
//            string email)
//        {

//            return Ok();
//        }

//        [ResponseType(typeof(void))]
//        public async Task<IHttpActionResult> Login(string username, string password,
//            string email)
//        {
//            return Ok();
//        }

//        // DELETE: api/Users/5
//        [ResponseType(typeof(User))]
//        public async Task<IHttpActionResult> DeleteUser(string id)
//        {
//            User user = await db.Users.FindAsync(id);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            db.Users.Remove(user);
//            await db.SaveChangesAsync();

//            return Ok(user);
//        }

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                db.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        private bool UserExists(string id)
//        {
//            return db.Users.Count(e => e.Password == id) > 0;
//        }
//    }
//}