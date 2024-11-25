using System.Text;

namespace ApiBase64.ConverterServices
{
    public class ConverToBase64Serviece
    {
       public static string EncodeToBase64(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }

        // פונקציה להמרת Base64 חזרה לטקסט רגיל
        public static string DecodeFromBase64(string base64EncodedData)
        {
            byte[] base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
            return Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
