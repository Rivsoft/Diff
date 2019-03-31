using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Diff.API.Helpers
{
    public class Base64Helper
    {
        public static byte[] ConvertBase64String(string input)
        {
            try
            {
                return Convert.FromBase64String(input);
            }
            catch
            {
                return null;
            }
        }
    }
}
