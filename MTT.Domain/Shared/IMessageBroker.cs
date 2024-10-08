using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTT.Domain.Shared
{
	public interface IMessageBroker
	{
		void SendMessage(string message);
	}
}
