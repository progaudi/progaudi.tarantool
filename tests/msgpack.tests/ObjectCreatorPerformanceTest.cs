using System;

using Shouldly;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests
{
    public class ObjectCreatorPerformanceTest
    {
        private const int TestsCount = 10000000;

        [Fact]
        public void TestActivator()
        {
            for (var i = 0; i < TestsCount; i++)
            {
                var instance = Activator.CreateInstance(typeof (TestReflectionConverter));
                instance.ShouldNotBeNull();
            }
        }

        [Fact]
        public void TestCompiledLambdaActivator()
        {
            var objectActivator = CompiledLambdaActivatorFactory.GetActivator<IMsgPackConverter>(typeof(TestReflectionConverter));

            for (var i = 0; i < TestsCount; i++)
            {
                var instance = objectActivator();
                instance.ShouldNotBeNull();
            }
        }
    }
}