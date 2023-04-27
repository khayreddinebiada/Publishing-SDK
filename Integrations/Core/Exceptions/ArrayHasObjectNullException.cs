﻿using System;

namespace Apps.Exception
{
    public class ArrayHasObjectNullException : ArgumentNullException
    {
        public ArrayHasObjectNullException(string message = "One of objects in array has null value.") : base(message)
        {

        }
    }
}
