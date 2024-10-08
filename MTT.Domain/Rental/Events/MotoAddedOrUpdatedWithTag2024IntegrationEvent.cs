using MediatR;

namespace MTT.Domain.Rental.Events
{
    public record MotoAddedOrUpdatedWithTag2024IntegrationEvent(string Tag, DateTime OccurredOn) : INotification
    {
    }
}
