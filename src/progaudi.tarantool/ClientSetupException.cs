using System;

namespace ProGaudi.Tarantool.Client
{
    public class ClientSetupException : Exception {
        public ClientSetupException(string msg) : base(msg)
        {
            
        }
    }
}