using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            //new User()
            //{
            //    Id = Guid.NewGuid(),
            //    FirstName = "David",
            //    LastName = "Silva",
            //    Email = "david_goncalvessilva@hotmail.com",
            //    Username = "davidsilva",
            //    Password = "davidsilva",
            //    Roles = new List<string> { "reader", "writer" }
            //},
            //new User()
            //{
            //    Id = Guid.NewGuid(),
            //    FirstName = "Helena",
            //    LastName = "Gonçalves",
            //    Email = "helena_goncalves@hotmail.com",
            //    Username = "helenagoncalves",
            //    Password = "helenagoncalves",
            //    Roles = new List<string> { "reader" }
            //}
        };
         
        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = Users.Find(
                x => x.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase) &&
                x.Password.Equals(password));

            return user;
        }
    }
}
