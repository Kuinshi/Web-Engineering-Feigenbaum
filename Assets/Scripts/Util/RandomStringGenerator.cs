using UnityEngine;

namespace Util
{
    public static class RandomStringGenerator
    {
        const string Glyphs= "abcdefghijklmnopqrstuvwxyz0123456789";

        public static string GetRandomString(int length)
        {
            string returnValue = "";
            for (int i = 0; i < length; i++)
            {
                returnValue += Glyphs[Random.Range(0, Glyphs.Length)];
            }

            return returnValue.ToUpper();
        }
    }
}