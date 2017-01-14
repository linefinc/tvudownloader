using System;

namespace TvUndergroundDownloader
{
    public class Ed2kfile
    {
        public Ed2kfile(string link)
        {
            HashSHA1 = null;

            if (link == null)
            {
                throw (new System.ApplicationException("Link error (missing ed2k://|file|)"));
            }

            if (link.IndexOf("ed2k://|file|", 0) != 0)
            {
                throw (new System.ApplicationException("Link error (missing ed2k://|file|)"));
            }

            if (CountChar(link, '|') < 5)
            {
                throw (new System.ApplicationException("Link error (not valid link)"));
            }

            Ed2kLink = link;
            //ed2k://|file|ubuntu-5.10-install-i386.iso|647129088|901E6AA2A6ACDC43A83AE3FC211120B0|h=3BX7GGEHOYSPPV4RMGQUAEKUMQ8HOMDE|/
            //       1    2                            3         4                                5                                  6
            int start = -1;
            int stop = -1;

            start = link.IndexOf('|') + 1;
            start = link.IndexOf('|', start) + 1;   // skip first vertical bar and go directly to second bar
            stop = link.IndexOf('|', start);    // position of 3th vertical bar

            FileName = link.Substring(start, stop - start);

            start = stop + 1;
            stop = link.IndexOf('|', start);    // position of 4th vertical bar
            string tempo = link.Substring(start, stop - start);
            FileSize = UInt64.Parse(tempo);

            start = stop + 1;
            stop = link.IndexOf('|', start);    // position of 5th vertical bar
            HashMD4 = link.Substring(start, stop - start);

            start = link.IndexOf("|h=");
            if (start == -1)    //check if exist AICH
            {
                return;
            }
            start += ("|h=").Length;
            stop = link.IndexOf('|', start);

            HashSHA1 = link.Substring(start, stop - start);
            return;
        }

        public Ed2kfile(Ed2kfile file)
        {
            this.Ed2kLink = file.Ed2kLink;
            this.FileName = file.FileName;
            this.HashMD4 = file.HashMD4;
            this.HashSHA1 = file.HashSHA1;
            this.FileSize = file.FileSize;
        }

        public string Ed2kLink { get; private set; }
        public string FileName { get; private set; }
        public ulong FileSize { get; private set; }
        public string HashMD4 { get; private set; }
        public string HashSHA1 { get; private set; }

        public override bool Equals(Object obj)
        {
            //
            //  https://msdn.microsoft.com/it-it/library/w4hkze5k(v=vs.110).aspx
            //
            if (obj == null || !(obj is Ed2kfile))
                return false;
            else
            {
                if (this.FileSize != ((Ed2kfile)obj).FileSize)
                {
                    return false;
                }

                if (this.HashMD4 != ((Ed2kfile)obj).HashMD4)
                {
                    return false;
                }

                return true;
            }
        }

        public string GetEscapedLink()
        {
            return Uri.EscapeDataString(Ed2kLink);
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

        public override int GetHashCode()
        {
            //
            //  https://msdn.microsoft.com/it-it/library/system.object.gethashcode(v=vs.110).aspx
            //
            return this.HashMD4.GetHashCode();
        }

        public string GetLink()
        {
            return Ed2kLink;
        }

        private int CountChar(string str, char c)
        {
            int count = 0;
            foreach (char t in str)
            {
                if (t == c)
                {
                    count++;
                }
            }
            return count;
        }
    }
}