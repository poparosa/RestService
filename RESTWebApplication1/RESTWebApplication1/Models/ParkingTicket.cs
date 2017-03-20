using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// adding comment to test git
namespace RESTWebApplication1.Models
{
	public class ParkingTicket
	{
		public Guid Identifier { get; set; }
		public DateTime TimeIn { get; set; }
	}
}