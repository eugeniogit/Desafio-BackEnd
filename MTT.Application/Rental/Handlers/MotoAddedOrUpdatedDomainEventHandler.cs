using MediatR;
using MTT.Domain.Rental.Events;

namespace MTT.Application.Rental.Handlers
{
    public class MotoAddedOrUpdatedDomainEventHandler : INotificationHandler<MotoAddedOrUpdatedDomainEvent>
    {
        public Task Handle(MotoAddedOrUpdatedDomainEvent domainEvent, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
