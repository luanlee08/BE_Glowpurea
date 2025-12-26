using System.Text;

namespace BE_Glowpurea.Helpers
{
    public class SkuGenerator
    {
        public static string Generate()
        {
            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string numbers = "0123456789";
            var random = new Random();
            var sb = new StringBuilder();

            // 4 chữ
            for (int i = 0; i < 4; i++)
                sb.Append(letters[random.Next(letters.Length)]);

            // 3 số
            for (int i = 0; i < 3; i++)
                sb.Append(numbers[random.Next(numbers.Length)]);

            return sb.ToString();
        }
    }
}
