using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace MotorDepot.WEB.Extensions
{
    public static class StreamExtensions
    {
        public static byte[] ReadBytes(this Stream input, int length)
        {
            byte[] buffer;
            using (var reader = new BinaryReader(input))
            {
                buffer = reader.ReadBytes(length);
            }
            return buffer;

        }
    }
}