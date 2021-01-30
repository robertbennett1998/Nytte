using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;
using Nytte.Testing;
using Shouldly;

namespace Nytte.PubSub.Tests
{
    public class EventManagerTests : ServiceUnderTest<IEventManager, EventManager>
    {
        private Mock<IAsyncEventHandlerRegister> _asyncEventHandlerRegister;
        private Mock<IEventHandlerRegister> _eventHandlerRegister;

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
        
        private class AsyncTestPubSubEvent : IAsyncPubSubEvent
        {
            
        }
        
        private class AsyncTestPubSubEventWithArgs : IAsyncPubSubEvent<TestPubSubEventWithArgsArgs>
        {
            public AsyncTestPubSubEventWithArgs(TestPubSubEventWithArgsArgs args)
            {
                Args = args;
            }
            
            public TestPubSubEventWithArgsArgs Args { get; }
        }
        
        public override void Setup()
        {
            _eventHandlerRegister = Mocker.GetMock<IEventHandlerRegister>();
            _asyncEventHandlerRegister = Mocker.GetMock<IAsyncEventHandlerRegister>();
        }
        
        public void Publish_EventNoArgs_TriesToCallHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandlerCalled = false;
            var eventHandler = new Action<TestPubSubEvent>((e) =>
            {
                eventHandlerCalled = true;
            });
            var @event = new TestPubSubEvent();

            _eventHandlerRegister
                .Setup(register => register.GetEventHandlers<TestPubSubEvent>())
                .Returns(new List<Action<TestPubSubEvent>> {eventHandler}.ToImmutableList());
            
            // Act
            sut.Publish(@event);
            
            // Assert
            eventHandlerCalled.ShouldBeTrue();
        }

        public void Publish_EventWithArgs_TriesToCallHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandlerCalled = false;
            var eventHandlerArg = 0;
            var eventHandler = new Action<TestPubSubEventWithArgs>((e) =>
            {
                eventHandlerArg = e.Args.I;
                eventHandlerCalled = true;
            });
            var @event = new TestPubSubEventWithArgs(new TestPubSubEventWithArgsArgs(19));

            _eventHandlerRegister
                .Setup(register => register.GetEventHandlers<TestPubSubEventWithArgs>())
                .Returns(new List<Action<TestPubSubEventWithArgs>> {eventHandler}.ToImmutableList());
            
            // Act
            sut.Publish(@event);
            
            // Assert
            eventHandlerCalled.ShouldBeTrue();
            eventHandlerArg.ShouldBe(19);
        }
                
        public void Publish_Null_ThrowsNullException()
        {
            // Arrange
            var sut = CreateSut();
            
            // Act + Assert
            Should.Throw<NullReferenceException>(() => sut.Publish<TestPubSubEvent>(null));
        }
        
        public async Task PublishAsync_EventNoArgs_TriesToCallHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandlerCalled = false;
            var eventHandler = new Func<AsyncTestPubSubEvent, Task>(async (e) =>
            {
                await Task.CompletedTask;
                eventHandlerCalled = true;
            });
            var @event = new AsyncTestPubSubEvent();

            _asyncEventHandlerRegister
                .Setup(register => register.GetAsyncEventHandlers<AsyncTestPubSubEvent>())
                .Returns(new List<Func<AsyncTestPubSubEvent, Task>> {eventHandler}.ToImmutableList());
            
            // Act
            await sut.PublishAsync(@event);
            
            // Assert
            eventHandlerCalled.ShouldBeTrue();
        }
        
        public async Task PublishAsync_EventWithArgs_TriesToCallHandler()
        {

            // Arrange
            var sut = CreateSut();
            var eventHandlerCalled = false;            
            var eventHandlerArg = 0;
            var eventHandler = new Func<AsyncTestPubSubEvent, Task>(async (e) =>
            {
                await Task.CompletedTask;
                eventHandlerCalled = true;
            });
            var @event = new AsyncTestPubSubEventWithArgs(new TestPubSubEventWithArgsArgs(10));

            _asyncEventHandlerRegister
                .Setup(register => register.GetAsyncEventHandlers<AsyncTestPubSubEvent>())
                .Returns(new List<Func<AsyncTestPubSubEvent, Task>> {eventHandler}.ToImmutableList());
            
            // Act
            await sut.PublishAsync(@event);
            
            // Assert
            eventHandlerCalled.ShouldBeTrue();
            eventHandlerArg.ShouldBe(10);
        }
        
        public async Task PublishAsync_Null_ThrowsNullException()
        {
            // Arrange
            var sut = CreateSut();
            
            // Act + Assert
            await Should.ThrowAsync<NullReferenceException>(async () => await sut.PublishAsync<AsyncTestPubSubEvent>(null));
        }

        public void Subscribe_EventHandlerNoArgs_RegistersEventHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandler = new Action<TestPubSubEvent>((e) => { });
            
            // Act
            sut.Subscribe(eventHandler);
            
            // Assert
            _eventHandlerRegister.Verify(register => register.RegisterEventHandler(eventHandler), Times.Once);
        }
        
        public void Subscribe_EventHandlerWithArgs_RegistersEventHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandler = new Action<TestPubSubEventWithArgs>((e) => { });
            
            // Act
            sut.Subscribe(eventHandler);
            
            // Assert
            _eventHandlerRegister.Verify(register => register.RegisterEventHandler(eventHandler), Times.Once);
        }
        
        public void Subscribe_Null_ThrowsNullException()
        {
            // Arrange
            var sut = CreateSut();
            
            // Act + Assert
            Should.Throw<NullReferenceException>(() => sut.Subscribe<TestPubSubEvent>(null));
        }
        
        public void Subscribe_Async_EventHandlerNoArgs_RegistersEventHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandler = new Func<AsyncTestPubSubEvent, Task>(async (e) => { await Task.CompletedTask; });
            
            // Act
            sut.Subscribe<AsyncTestPubSubEvent>(null);
            
            // Assert
            _asyncEventHandlerRegister.Verify(register => register.RegisterAsyncEventHandler(eventHandler), Times.Once);
        }
        
        public void Subscribe_Async_EventHandlerWithArgs_RegistersEventHandler()
        {
            // Arrange
            var sut = CreateSut();
            var eventHandler = new Func<AsyncTestPubSubEventWithArgs, Task>(async (e) => { await Task.CompletedTask; });

            // Act
            sut.Subscribe(eventHandler);
            
            // Assert
            _asyncEventHandlerRegister.Verify(register => register.RegisterAsyncEventHandler(eventHandler), Times.Once);
        }
        
        public void Subscribe_Async_Null_ThrowsNullException()
        {
            // Arrange
            var sut = CreateSut();
            
            // Act + Assert
            Should.Throw<NullReferenceException>(() => sut.Subscribe<AsyncTestPubSubEvent>(null));
        }
    }
}