using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{
    class Ed2kParser
    {
        private string _Link;
        private uint _Size;
        private string _FileName;
        private string _Hash;

        public Ed2kParser(string link)
        {
            _Link = link;
            Decode(link);
        }

        private void Decode(string link)
        {
            // if the link is not a ed2k file link return
            int i, j, k;

            k = "ed2k://|file|".Length ;
            i = link.IndexOf("|", k + 1 );
            j = link.IndexOf("|", i + 1);
            _FileName = link.Substring(k + 1, k - j - 1);
            _Size = uint.Parse( _Link.Substring(i + 1, j - i - 1));
            _Hash = link.Substring(j + 1, 32);// 32 is the size of md4
        }

        public string GetFileName()
        {
            return _FileName;
        }

        public uint GetSize()
        {
            return _Size;
        }

        public string GetHash()
        {
            return _Hash;
        }
    }
}
