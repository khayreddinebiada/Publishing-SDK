﻿namespace Apps.Exception
{
    public class ReinitializedException : System.Exception
    {
        public ReinitializedException(string message = "The Object is already intialized.") : base(message)
        {

        }
    }
}
