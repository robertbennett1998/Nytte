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
    public class AsyncEventHandlerRegisterTests : ServiceUnderTest<IAsyncEventHandlerRegister, AsyncEventHandlerRegister>
    {
        private class TestPubSubEvent : IAsyncPubSubEvent
        {
            
        }
        private record TestPubSubEventWithArgsArgs(int I);
        private class TestPubSubEventWithArgs : IAsyncPubSubEvent<TestPubSubEventWithArgsArgs>
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
            Func<TestPubSubEvent, Task> eventHandler = async (e) => await Task.CompletedTask;

            // Act
            sut.RegisterAsyncEventHandler(eventHandler);
            
            // Assert
            sut.GetAsyncEventHandlers<TestPubSubEvent>().Count.ShouldBe(1);
            sut.GetAsyncEventHandlers<TestPubSubEvent>()[0].ShouldBe(eventHandler);
        }
        
        [Test]
        public void RegisterHandler_ValidArgsFunctionHandler_RegistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Func<TestPubSubEventWithArgs, Task> eventHandler = async (e) => await Task.CompletedTask;

            // Act
            sut.RegisterAsyncEventHandler(eventHandler);
            
            // Assert
            sut.GetAsyncEventHandlers<TestPubSubEventWithArgs>().Count.ShouldBe(1);
            sut.GetAsyncEventHandlers<TestPubSubEventWithArgs>()[0].ShouldBe(eventHandler);
        }

        [Test]
        public void DeregisterHandler_ValidNoArgsFunctionHandler_DeregistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Func<TestPubSubEvent, Task> eventHandler = async (e) => await Task.CompletedTask;

            // Act
            sut.RegisterAsyncEventHandler(eventHandler);
            sut.DeregisterAsyncEventHandler(eventHandler);
            
            // Assert
            sut.GetAsyncEventHandlers<TestPubSubEvent>().Count.ShouldBe(0);
        }
        
        [Test]
        public void DeregisterHandler_ValidArgsFunctionHandler_DeregistersListener()
        {
            // Arrange
            var sut = CreateSut();
            Func<TestPubSubEventWithArgs, Task> eventHandler = async (e) => await Task.CompletedTask;

            // Act
            sut.RegisterAsyncEventHandler(eventHandler);
            sut.DeregisterAsyncEventHandler(eventHandler);
            
            // Assert
            sut.GetAsyncEventHandlers<TestPubSubEventWithArgs>().Count.ShouldBe(0);
        }
    }
}