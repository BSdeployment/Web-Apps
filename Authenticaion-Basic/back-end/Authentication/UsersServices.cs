using AuthApiApplication;
using Microsoft.EntityFrameworkCore;

namespace ApiWuthHush.Authentication
{
    public class UsersServices
    {
        private readonly UsersContext _context;

        public UsersServices(UsersContext usersContext)
        {
            this._context = usersContext;
        }

        public async Task<User> Register(string username,string password)
        {
            HushServices.CreatePasswordHash(password,out byte[] passwordhush,out byte[] salt);

            var user = new User
            {
                Username = username,
                PasswordHash = passwordhush,
                PasswordSalt = salt
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }
        public async Task<User> Login(string username,string password)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(a=>a.Username ==username);

            if(currentUser is null)
            {
                return null;
            }

            if (!HushServices.VerifyPasswordHash(password, currentUser.PasswordHash, currentUser.PasswordSalt))
            {
                return null;
            }
            return currentUser;
        }
        public async Task<List<User>> GetUsers()
        {
            var result = await  _context.Users.ToListAsync();

            return result;
        }

        public async Task<bool> DeleteUsers(string username)
        {
            var currentUser = _context.Users.FirstOrDefault(a=>a.Username == username);
            if (currentUser is null)
                return await Task.FromResult(false);
            else
            {
                _context.Users.Remove(currentUser);
                await _context.SaveChangesAsync();
                return await Task.FromResult(true);
            }
          
        }
        public async Task<List<UserModel>> GetUsersNames()
        {
            var result = await _context.Users.ToListAsync();
            var newlist = new List<UserModel>();

            foreach (var user in result)
            {
                newlist.Add(new UserModel { username = user.Username, password = "secret" });
            }
            return newlist;
        }
    }
}
