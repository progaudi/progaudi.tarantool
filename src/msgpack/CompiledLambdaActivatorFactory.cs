using System;
using System.Linq.Expressions;
#if DNXCORE50
using System.Reflection;
#endif

namespace TarantoolDnx.MsgPack
{
    public class CompiledLambdaActivatorFactory
    {
        public delegate T ObjectActivator<out T>();

        public static ObjectActivator<T> GetActivator<T>(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);

            //make a NewExpression that calls the
            //ctor with the args we just created
            var newExp = Expression.New(ctor);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            var lambda =
                Expression.Lambda(typeof(ObjectActivator<T>), newExp);

            //compile it
            var compiled = (ObjectActivator<T>)lambda.Compile();
            return compiled;
        }
    }
}