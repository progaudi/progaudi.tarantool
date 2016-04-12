using System;
using System.Linq.Expressions;
// ReSharper disable once RedundantUsingDirective
using System.Reflection;

namespace TarantoolDnx.MsgPack
{
    public class CompiledLambdaActivatorFactory
    {
        public delegate object ObjectActivator();

        public static ObjectActivator GetActivator(Type type)
        {
            var ctor = type.GetConstructor(Type.EmptyTypes);

            //make a NewExpression that calls the
            //ctor with the args we just created
            var newExp = Expression.New(ctor);

            //create a lambda with the New
            //Expression as body and our param object[] as arg
            var lambda =
                Expression.Lambda(typeof(ObjectActivator), newExp);
                
            //compile it
            var compiled = (ObjectActivator)lambda.Compile();
            return compiled;
        }
    }
}