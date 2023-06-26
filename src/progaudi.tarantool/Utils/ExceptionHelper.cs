using System;
using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Model.Responses;
using System.Reflection;
using System.Runtime.Serialization;

namespace ProGaudi.Tarantool.Client.Utils
{
    internal static class ExceptionHelper
    {
        public static Exception UnexpectedGreetingBytesCount(int readCount)
        {
            return new ArgumentException($"Invalid greetings response length. 128 is expected, but got {readCount}.");
        }

        public static Exception InvalidMapLength(uint? actual, params uint [] expected)
        {
            return new ArgumentException($"Invalid map length: {string.Join(", ", expected)} is expected, but got {actual}.");
        }

        public static Exception UnexpectedKey(Key actual, params Key[] expected)
        {
            return new ArgumentException($"Unexpected key: {string.Join(", ", expected)} is expected, but got {actual}.");
        }

        public static Exception InvalidArrayLength(uint expected, uint? actual)
        {
            return new ArgumentException($"Invalid array length: {expected} is expected, but got {actual}.");
        }

        public static Exception UnexpectedDataType(DataTypes expected, DataTypes actual)
        {
            return new ArgumentException($"Unexpected data type: {expected} is expected, but got {actual}.");
        }

        public static Exception UnexpectedDataType(byte actualCode, params byte[] expectedCodes)
        {
            return new ArgumentException($"Unexpected data type: {String.Join(", ", expectedCodes)} is expected, but got {actualCode}.");
        }

        public static Exception UnexpectedMsgPackHeader(byte actual, byte expected)
        {
            return new ArgumentException($"Unexpected msgpack header: {expected} is expected, but got {actual}.");
        }

        public static Exception NotConnected()
        {
            return new InvalidOperationException("Can't perform operation. Looks like we are not connected to tarantool. Call 'Connect' method before calling any other operations.");
        }

        public static ArgumentException TarantoolError(ResponseHeader header, ErrorResponse errorResponse)
        {
            var detailedMessage = GetDetailedTarantoolMessage(header.Code);
            return new ArgumentException($"Tarantool returns an error for request with id: {header.RequestId}, code: 0x{header.Code:X}  and message: {errorResponse.ErrorMessage}. {detailedMessage}");
        }

        public static ArgumentOutOfRangeException WrongRequestId(RequestId requestId)
        {
            return new ArgumentOutOfRangeException($"Can't find pending request with id = {requestId}");
        }

        public static ArgumentException InvalidSpaceName(string name)
        {
            return new ArgumentException($"Space with name '{name}' was not found!");
        }

        public static ArgumentException InvalidSpaceId(uint id)
        {
            return new ArgumentException($"Space with id '{id}' was not found!");
        }

        public static ArgumentException InvalidIndexName(string indexName, string space)
        {
            return new ArgumentException($"Index with name '{indexName}' was not found in space {space}!");
        }

        public static ArgumentException InvalidIndexId(uint indexId, string space)
        {
            return new ArgumentException($"Index with id '{indexId}' was not found in space {space}!");
        }

        public static Exception PropertyUnspecified(string propertyName)
        {
            return new ArgumentException($"Property '{propertyName}' is not specified!");
        }

        public static InvalidOperationException EnumExpected(Type type)
        {
            return new InvalidOperationException($"Enum expected, but got {type}.");
        }

        public static InvalidOperationException EnumExpected(TypeInfo type)
        {
            return new InvalidOperationException($"Enum expected, but got {type}.");
        }

        public static InvalidOperationException UnexpectedEnumUnderlyingType(Type enumUnderlyingType)
        {
            return new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
        }

        public static NotSupportedException WrongIndexType(string indexType, string operation)
        {
            return new NotSupportedException($"Only {indexType} indicies support {operation} operation.");
        }

        public static Exception RequestWithSuchIdAlreadySent(RequestId requestId)
        {
            return new ArgumentException($"Task with id {requestId} already sent.");
        }

        private static string GetDetailedTarantoolMessage(CommandCode code)
        {
            switch ((uint)code)
            {
                case 0x8012:
                    return "If index part type is NUM, unsigned int should be used.";
            }

            return null;
        }

        public static Exception VersionCantBeEmpty()
        {
            return new ArgumentNullException("version", "TarantoolVersion should be not null.");
        }

        public static Exception CantParseBoxInfoResponse()
        {
            return new SerializationException("Box info response is malformed");
        }

        public static Exception CantCompareBuilds(TarantoolVersion left, TarantoolVersion right)
        {
            return new InvalidOperationException($"Versions '{left}' and '{right}' differs only by commit hash, can't compare them.");
        }

        public static Exception SqlIsNotAvailable(TarantoolVersion version)
        {
            return new InvalidOperationException($"Can't use sql on '{version}' of tarantool. Upgrade to 1.8 (prefer latest one).");
        }

        public static Exception NoDataInDataResponse()
        {
            return new InvalidOperationException("No data in data response");
        }
    }
}