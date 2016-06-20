﻿namespace Tarantool.Client.IProto.Data
{
    public abstract class HeaderBase
    {
        protected HeaderBase(CommandCode code, RequestId requestId)
        {
            Code = code;
            RequestId = requestId;
        }

        public CommandCode Code { get; }

        public RequestId RequestId { get; set; }
    }
}