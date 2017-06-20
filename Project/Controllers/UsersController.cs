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
        [Route("api/Users/Register/{username}/{password}/{email}")]
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
                        ["ans"] = "/views/MultiPlayer.html"
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
            }
            return Ok(solveObj);
        }

        [HttpGet]
        [Route("api/Users/Login/{username}/{password}")]
        public IHttpActionResult Login(string username, string password)
        {
            if (UserExists(username) && db.Users.Find(username).IsPassword(password))
            {
                return Ok("/views/MultiPlayer.html");
            }
            return NotFound();//"Wrong username or password"
        }

        [HttpGet]
        [Route("api/Users/GetTop10")]
        public IHttpActionResult GetTop10()
        {
            var users = db.Users.OrderByDescending(a => (a.Wins - a.Loses)).Take(10);
            List<JObject> lst2 = new List<JObject>(users.Count());
            users.Reverse();
            foreach (User user in users)
            {
                lst2.Add(new JObject
                {
                    ["username"] = user.UserName,
                    ["wins"] = user.Wins,
                    ["loses"] = user.Loses
                });
            }
            return Ok(lst2);
            //List<User> lst = new List<User>(10);
            //// initial lst as an desending list of top 10 users
            //int n = (10 <= users.Count()) ? 10 : users.Count();
            //for (int i = 0; i < n; ++i)
            //{
            //    lst.Add(users.ElementAt(i));
            //}
            //lst.Sort((a, b) => {
            //    int diffA = (a.Wins - a.Loses), diffB = (b.Wins - b.Loses);
            //    int retVal = 0;
            //    if (diffA < diffB)
            //    {
            //        retVal = 1;
            //    }
            //    else if(diffA > diffB)
            //    {
            //        retVal = -1;
            //    }
            //    return retVal;
            //});
            //if (n > 10)
            //{
            //    // lst is an desending list of top 10 users
            //    int i;
            //    User lastUser = lst.ElementAt(0);
            //    foreach (User user in db.Users.Skip(10))
            //    {
            //        if ((user.Wins - user.Loses) > (lastUser.Wins - lastUser.Loses))
            //        {
            //            i = 0;
            //            foreach (User otherUser in lst)
            //            {
            //                if ((user.Wins - user.Loses) > (otherUser.Wins - otherUser.Loses))
            //                {
            //                    ++i;
            //                }
            //                else
            //                {
            //                    break;
            //                }
            //            }
            //            lst.Insert(i, user);
            //            lst.RemoveAt(0);
            //        }
            //    }
            //}

            //// turn to JObject list
            //List<JObject> lst2 = new List<JObject>(lst.Count);
            //foreach (User user in lst)
            //{
            //    lst2.Add(new JObject
            //    {
            //        ["username"] = user.UserName,
            //        ["wins"] = user.Wins,
            //        ["loses"] = user.Loses
            //    });
            //}
            //return Ok(lst2);
        }

        public User GetUser(string username)
        {
            return db.Users.Find(username);
        }

        public async Task UpdateUserAsync(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        public void TearDown()
        {
            this.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserName == id) > 0;
        }
    }
}