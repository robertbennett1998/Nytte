using System;
using Moq.AutoMock;
using NUnit.Framework;

namespace Nytte.Testing
{
    public abstract class ServiceUnderTestBase
    {
        [SetUp]
        public virtual void NUnitSetup()
        {
            Mocker = new AutoMocker();
        }

        protected AutoMocker Mocker;

        public abstract void Setup();

    }
    
    public abstract class ServiceUnderTest<TAbstraction, TImplementation> : ServiceUnderTestBase where TImplementation : class, TAbstraction
    {
        protected TAbstraction CreateSut() => Mocker.CreateInstance<TImplementation>();
    }

    public abstract class ServiceUnderTest<TImplementation> : ServiceUnderTestBase where TImplementation : class
    {
        protected TImplementation CreateSut() => Mocker.CreateInstance<TImplementation>();
    }
}