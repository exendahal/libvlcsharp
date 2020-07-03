using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace VideoPlayervlc
{
    public interface IFileService
    {
        void SaveFile(string name, Stream data);
        bool checkFile(string name);

    }
}
