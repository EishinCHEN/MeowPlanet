using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace MeowPlanet.MemberManagement
{
    public class SHAUtility
    {
        public static String hashData(String source)
        {
            //轉換成byte[]
            System.Text.Encoding encoder = System.Text.Encoding.UTF8;
            Byte[] sourceBytes = encoder.GetBytes(source);
            //演算
            SHA256 sha256 = SHA256.Create();
            Byte[] hashedByte = sha256.ComputeHash(sourceBytes);
            String hashString = encoder.GetString(hashedByte);
            return hashString;
        }
    }
}
