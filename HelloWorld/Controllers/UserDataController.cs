using Microsoft.AspNetCore.Mvc;
using LiteDB;
using HelloWorld.Models;

namespace HelloWorld.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDataController : Controller
    {
        private const string databasePath = "UserData.db";
        private const string userCollection = "users";
        private List<UserData> _usersDetails = new List<UserData> { new UserData { UserName="mouna" , DateOfBirth = new DateTime(1990,07, 30) }  };


        // Create/update user details : PUT /hello/<username> { “dateOfBirth”: “YYYY-MM-DD” }

        [HttpPut("{username}")]
        public IActionResult PostUserDetails(string userName, [FromBody] UserData userData)
        {
            if (userData.DateOfBirth > DateTime.Today)
            {
                return BadRequest("Date of birth is greater than today");
            }

            using (var db = new LiteDatabase(databasePath))
            {
                var userdbCollection = db.GetCollection<UserData>(userCollection);
                if(userdbCollection.FindOne(x=>x.UserName == userName) == null)
                {
                    return NotFound("User" +userName+ " not found in the database");
                }
                userdbCollection.Upsert(userData);
            }

            //userData.UserName = userName.ToLower();
            //userData.DateOfBirth = userData.DateOfBirth;
            //_usersDetails.Add(userData);

            return NoContent();
        }

        // Return hello birthday for a given user details : GET /hello/<username>

        [HttpGet("{username}")]
        public IActionResult GetUserDetails(string userName)
        {
            //UserData userData = _usersDetails.Find(x => x.UserName == userName.ToLower());

            using (var db = new LiteDatabase(databasePath))
            {
                var userdbCollection = db.GetCollection<UserData>(userCollection);
                var userData = userdbCollection.FindOne(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
                if (userData == null)
                {
                    return NotFound();
                }

                if ((userData.DateOfBirth.Date.Month == DateTime.Today.Date.Month) && (userData.DateOfBirth.Date.Day == DateTime.Today.Date.Day))
                {
                    return Ok(string.Format("Hello," + userData.UserName + "! Happy Birthday"));
                }
                else
                {
                    return Ok(string.Format("Hello," + userData.UserName + " !Your birthday is in " + GetDaysRemainingForBirthday(userData.DateOfBirth) + " day(s)"));
                }
            }

        }


        public int GetDaysRemainingForBirthday(DateTime birthday)
        {
            DateTime today = DateTime.Today;
            DateTime nextBirthday = birthday.AddYears(today.Year - birthday.Year);

            if (nextBirthday < today)
                nextBirthday = nextBirthday.AddYears(1);

            return (nextBirthday - today).Days;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
