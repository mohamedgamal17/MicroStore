using FluentAssertions;
using MicroStore.Inventory.Application.Common;
using MicroStore.Inventory.Events.Contracts;

namespace MicroStore.Inventory.Application.Tests
{
    [TestFixture]
    public abstract class AggregateRootTest<TAggregate> 
        where TAggregate : IAggregateRoot
    {


        private readonly TAggregate _aggregate;

        public AggregateRootTest(TAggregate aggregate)
        {
            _aggregate = aggregate;
        }


        protected void Given(params IEvent[] events) 
        {
            foreach (var evnt in events)
            {
                _aggregate.Recive(evnt);
            }
        }


        protected void When(Action<TAggregate> action)
        {
            action(_aggregate);
        }

        protected void Then<TEvent>(Action<TEvent>? condition)
        {
            var events = _aggregate.GetUncomittedEvents();

            events.Count.Should().Be(1);

            var evnt =  events.First();

            evnt.Should().BeOfType<TEvent>();

            condition?.Invoke((TEvent)evnt);
        }

        protected void Throws<TException>(Action<TAggregate> action , Action<TException>? condition)
            where TException : Exception
        {
           
            var exceptionAssert = FluentActions.Invoking(() => action(_aggregate)).Should().Throw<TException>();

            condition?.Invoke(exceptionAssert.Subject.First());
        }

    }
}
