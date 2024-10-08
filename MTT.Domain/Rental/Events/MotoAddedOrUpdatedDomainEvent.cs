using MediatR;
using MTT.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTT.Domain.Rental.Events
{
    public record MotoAddedOrUpdatedDomainEvent(string Tag, int Year) : IDomainEvent, INotification
    {
    }
}
