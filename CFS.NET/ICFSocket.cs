﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFS.Net
{
    public interface ICFSocket
    {
        void Send(string data);
        string Receive();
    }
}
