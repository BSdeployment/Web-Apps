namespace ApiWuthHush.Authentication
{
    public class HushServices
    {
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key; // יצירת salt אקראי
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // חישוב hash
            }
        }
        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash); // בדיקה אם ה-Hash החדש תואם לישן
            }
        }
    }
}
