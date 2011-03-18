using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Just
{
    namespace Net
    {
        namespace Specialized
        {
            public class PalindromUdpServer : ExtendedUdpServer
            {
                public PalindromUdpServer(System.Net.IPEndPoint receiveEndPoint, Encoding encoding, int bufferSize = 512)
                    : base(receiveEndPoint, encoding, bufferSize)
                {
                }

                protected override void ProcessData(byte[] data, System.Net.EndPoint sender)
                {
                    SendTo(this._Encoding.GetBytes(IsPalindrom(this._Encoding.GetString(data)).ToString()), sender);
                }

                private bool IsPalindrom(string text)
                {
                    text = text.Replace(" ", "").ToLower();
                    char[] chars = text.ToCharArray();
                    Array.Reverse(chars);
                    if (new string(chars) == text)
                        return true;
                    else
                        return false;
                }
            }
        }

    }
}