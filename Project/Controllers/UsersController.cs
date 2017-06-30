using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Project.Models;
using Newtonsoft.Json.Linq;

namespace Project.Controllers
{
    /// <summary>
    /// Controller to handle users related operations(register, login...).
    /// </summary>
    public class UsersController : ApiController
    {
        private ProjectContext db = new ProjectContext();

        // GET: api/Users
        /// <summary>
        /// gets the list of users in the database.
        /// </summary>
        /// <returns> the list of users in the database. </returns>
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        /// <summary>
        /// Adds a new user to the database with the given username, password and email.
        /// The respond, if registression succeeded, is a JObject with the field "ans"
        /// that contains the link to the multiplayer HTML page ("/views/MultiPlayer.html").
        /// </summary>
        /// <param name="username"> string </param>
        /// <param name="password"> string </param>
        /// <param name="email"> string </param>
        /// <returns> an HTTP response (OK or Confilct) </returns>
        [HttpGet]
        [Route("api/Users/Register/{username}/{password}/{email}")]
        public IHttpActionResult Register(string username, string password, string email)
        {
            JObject answer;
            if (!UserExists(username))
            {
                User user = new User(username, password, email);
                db.Users.Add(user);
                try
                {
                    db.SaveChanges();
                    answer = new JObject
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
                answer = new JObject
                {
                    ["ans"] = "0"
                };
            }
            return Ok(answer);
        }

        /// <summary>
        /// The function checks if a user with the given username and password
        /// exists in the database. The respond, if login succeeded, is the link
        /// to the multiplayer HTML page ("/views/MultiPlayer.html").
        /// </summary>
        /// <param name="username"> string </param>
        /// <param name="password"> string </param>
        /// <returns> an HTTP response (OK or NotFound) </returns>
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

        /// <summary>
        /// Returns a list with the details of the top 10 players.
        /// The respond is a list of JObjects with the fields {"username" : string,
        /// "wins" : int, "loses" : int}.
        /// </summary>
        /// <returns> Returns a list with the details of the top 10 players. </returns>
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
        }

        /// <summary>
        /// Returns the database record with the id value as the argument.
        /// </summary>
        /// <param name="username"> string </param>
        /// <returns> User class object </returns>
        public User GetUser(string username)
        {
            return db.Users.Find(username);
        }

        /// <summary>
        /// async saves to the database the changes of the exsiting record user.
        /// </summary>
        /// <param name="user"> a User class object that's in the database </param>
        public async Task UpdateUserAsync(User user)
        {
            db.Entry(user).State = EntityState.Modified;
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// calls this.Dispose().
        /// </summary>
        public void TearDown()
        {
            this.Dispose();
        }

        /// <summary>
        /// Calls the protected Dispose method and if argument is True,
        /// it will also call the "db" member Dispose method.
        /// </summary>
        /// <param name="disposing"> True/False </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// returns True/False - the database has a record with the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns> True/False - the database has a record with the given id </returns>
        public bool UserExists(string id)
        {
            return db.Users.Count(e => e.UserName == id) > 0;
        }
    }
}