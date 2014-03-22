using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SortServer
{
    public class Converter
    {
        #region Methods

        public static List<ulong> ToList(Stream stream)
        {
            var list = new List<ulong>();

            var b = stream.ReadByte();

            while(b != -1)
            {
                var bytes = new byte[sizeof(ulong)];

                bytes[0] = (byte)b;
                stream.Read(bytes, 1, sizeof(ulong) - 1);

                list.Add(BitConverter.ToUInt64(bytes, 0));

                b = stream.ReadByte();
            }
            
            return list;
        }

        public static byte[] ToBytes(List<ulong> list)
        {
            var bytes = new byte[list.Count * sizeof(ulong)];

            for(int i = 0; i < list.Count; ++i)
            {
                Array.Copy(
                    BitConverter.GetBytes(list[i]), 0, 
                    bytes, i * sizeof(ulong),
                    sizeof(ulong));
            }

            return bytes;
        }

        public static List<byte> ToBytes(Stream stream)
        {
            var bytes = new List<byte>();

            var b = stream.ReadByte();
            while(b != 1)
            {
                bytes.Add((byte)b);
                b = stream.ReadByte();
            }

            return bytes;
        }

        #endregion // Methods
    }
}
