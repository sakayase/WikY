﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WikYModels.Exceptions
{
    public class SignInException : Exception
    {
        public SignInException(string? message = null) : base(message)
        {
        }
    }
}
