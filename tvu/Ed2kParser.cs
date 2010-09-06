using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{
    public class Ed2kParser
    {
        private string _Link;
        private uint _Size;
        private string _FileName;
        private string _Hash;

        public Ed2kParser()
        {
         
        }

        public Ed2kParser(string link)
        {
            _Link = link;
            Decode(link);
        }

        private void Decode(string link)
        {

            string[] temp = link.Split('|');

            if (temp[0] == "")
            {
                // add excemption
            }
            if (temp[1] == "")
            {
                // add exception
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

        public static bool operator ==(Ed2kParser A, Ed2kParser B)
        {
            if (A._Size != B._Size)
            {
                return false;
            }

            if (A._Hash != B._Hash)
            {
                return false;
            }

            return true;
        }



        public static bool operator !=(Ed2kParser A, Ed2kParser B)
        {
            if (A._Size == B._Size)
            {
                return false;
            }

            if (A._Hash == B._Hash)
            {
                return false;
            }

            return true;
        }
        
        public override int GetHashCode()
        {
            return 0;
        }


    }
}
