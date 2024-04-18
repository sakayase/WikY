﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikYModels.Exceptions
{
    public class GetConnectedUserException : Exception
    {
        public GetConnectedUserException(string? message = null) : base(message)
        {
        }
    }
}
