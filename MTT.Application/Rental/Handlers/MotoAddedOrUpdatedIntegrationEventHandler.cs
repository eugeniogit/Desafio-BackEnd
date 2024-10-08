using MediatR;
using MTT.Domain.Rental.Events;
using MTT.Domain.Shared;
using System.Text.Json;

namespace MTT.Application.Rental.Handlers
{
    public class MotoAddedOrUpdatedIntegrationEventHandler : INotificationHandler<MotoAddedOrUpdatedDomainEvent>
    {
		private readonly IMessageBroker _messageBroker;
		public MotoAddedOrUpdatedIntegrationEventHandler(IMessageBroker messageBroker)
		{
			_messageBroker = messageBroker;

		}

		public Task Handle(MotoAddedOrUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            if (domainEvent.Year == 2024)
            {
				_messageBroker.SendMessage(JsonSerializer.Serialize(new MotoAddedOrUpdatedWithTag2024IntegrationEvent(domainEvent.Tag, DateTime.UtcNow)));
            }

            return Task.CompletedTask;
        }
    }
}
