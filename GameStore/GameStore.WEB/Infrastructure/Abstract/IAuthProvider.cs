﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameStore.WEB.Infrastructure.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string  userName, string password);
    }
}
