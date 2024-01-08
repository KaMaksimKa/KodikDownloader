

using System.Text;

namespace KodikUploader.Utils
{
    public static class StringUtils
    {
        public static string ProcessingGviSrc(string link)
        {
            var newLink = "";
            foreach (var ch in link)
            {
                if (ch >= 65 && ch <= 90 || ch >= 97 && ch <= 122)
                {
                    var e = ch;
                    newLink += ((char)((e <= 'Z' ? 90 : 122) >= (e = (char)(e + 13)) ? e : e - 26));
                }
                else
                {
                    newLink += ch;
                }
            }
            
            return newLink;
        }

        public static string Atob(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }
    }
}
