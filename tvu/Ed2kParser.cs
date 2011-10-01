using System;
using System.Collections.Generic;
using System.Text;

namespace tvu
{
    public class Ed2kParser
    {
        public string Ed2kLink { get; private set; }
        public string FileName {  get; private set; }
        public string HashMD4 {  get; private set; }
        public string HashSHA1 {  get; private set; }
        public ulong FileSize {  get; private set; }

        public Ed2kParser(string link)
        {
            Ed2kLink = link;
            Decode(link);
        }

        private void Decode(string link)
        {
            if (link.Substring(0,13) != "ed2k://|file|")
            {
                throw (new System.ApplicationException("Link error (missing ed2k://|file|)"));
            }


            string[] temp = link.Split('|');

            if (temp.Length < 5)
            {
                throw (new System.ApplicationException("Link error (not valid link)"));
            }

            FileName = temp[2];
            FileSize = uint.Parse(temp[3]);
            HashMD4 = temp[4];

        }



    /*    public void SetLink(string link)
        {

            this.Decode(link);
        }*/


        public string GetLink()
        {
            return Ed2kLink;
        }
        
        public string GetFileName()
        {
            return FileName;
        }

        public ulong GetFileSize()
        {
            return FileSize;
        }

        public string GetHash()
        {
            return HashMD4;
        }

        public static bool operator ==(Ed2kParser A, Ed2kParser B)
        {
            if (A.FileSize != B.FileSize)
            {
                return false;
            }

            if (A.HashMD4 != B.HashMD4)
            {
                return false;
            }

            return true;
        }

         public override bool Equals(object o)
         {
             Ed2kParser A = (Ed2kParser)o;

             if (this.FileSize != A.FileSize)
             {
                 return false;
             }

             if (this.HashMD4 != A.HashMD4)
             {
                 return false;
             }

             return true;
         }


        public  bool Equals(Ed2kParser A)
        {
            if (this.FileSize != A.FileSize)
            {
                return false;
            }

            if (this.HashMD4 != A.HashMD4)
            {
                return false;
            }

            return true;
        }

        public static bool operator !=(Ed2kParser A, Ed2kParser B)
        {
            if (A.FileSize == B.FileSize)
            {
                return false;
            }

            if (A.HashMD4 == B.HashMD4)
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
