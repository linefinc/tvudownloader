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

            string[] temp = link.Split('|');

            if (temp[0] == "")
            {

            }
            if (temp[1] == "")
            {

            }

            _FileName = temp[2];
            _Size = uint.Parse(temp[3]);
            _Hash = temp[4];

        }

        public string GetLink()
        {
            return _Link;
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

        public override bool Equals(Ed2kParser temp)
        {
            if(temp._Size != this._Size)
            {
                return false;
            }

            if(temp._Hash != this._Hash)
            {
                return false;
            }

            return true;
        }

    }
}
