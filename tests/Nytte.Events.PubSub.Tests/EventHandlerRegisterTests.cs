using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Nytte.Testing;
using Shouldly;

namespace Nytte.PubSub.Tests
{
    [TestFixture]
    public class EventHandlerRegisterTests : ServiceUnderTest<IEventHandlerRegister, EventHandlerRegister>
    {
        private class TestPubSubEvent : IPubSubEvent
        {
            
        }
        private record TestPubSubEventWithArgsArgs(int I);
        private class TestPubSubEventWithArgs : IPubSubEvent<TestPubSubEventWithArgsArgs>
        {
            public TestPubSubEventWithArgs(TestPubSubEventWithArgsArgs args)
            {
                Args = args;
            }
            
            public TestPubSubEventWithArgsArgs Args { get; }
        }
        
        public override void Setup()
        {
        }

        [Test]
        public void RegisterHandler_ValidNoArgsFunctionHandler_RegistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Action<TestPubSubEvent> eventHandler = (e) => { };

            // Act
            sut.RegisterEventHandler(eventHandler);

            // Assert
            sut.GetEventHandlers<TestPubSubEvent>().Count.ShouldBe(1);
            sut.GetEventHandlers<TestPubSubEvent>()[0].ShouldBe(eventHandler);
        }
        
        [Test]
        public void RegisterHandler_ValidArgsFunctionHandler_RegistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Action<TestPubSubEventWithArgs> eventHandler = (e) => { };

            // Act
            sut.RegisterEventHandler(eventHandler);

            // Assert
            sut.GetEventHandlers<TestPubSubEventWithArgs>().Count.ShouldBe(1);
            sut.GetEventHandlers<TestPubSubEventWithArgs>()[0].ShouldBe(eventHandler);
        }

        [Test]
        public void DeregisterHandler_ValidNoArgsFunctionHandler_RegistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Action<TestPubSubEvent> eventHandler = (e) => { };

            // Act
            sut.RegisterEventHandler(eventHandler);
            sut.DeregisterEventHandler(eventHandler);

            // Assert
            sut.GetEventHandlers<TestPubSubEvent>().Count.ShouldBe(0);
        }
        
        [Test]
        public void DeregisterHandler_ValidArgsFunctionHandler_RegistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Action<TestPubSubEventWithArgs> eventHandler = (e) => { };

            // Act
            sut.RegisterEventHandler(eventHandler);
            sut.DeregisterEventHandler(eventHandler);

            // Assert
            sut.GetEventHandlers<TestPubSubEventWithArgs>().Count.ShouldBe(0);
        }
    }
}