using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace RESTWebApplication1.Controllers
{
	public class ParkController : ApiController
	{

		/// <summary>
		/// Get the empty spaces
		/// returns: number of open spaces
		/// </summary>
		[HttpGet]
		[Route("EmptyCount")]
		public IHttpActionResult EmptyCount()
		{
			IHttpActionResult result;
			int openSpaces = InMemoryDB.GetOpenSpots();
			result = Ok(openSpaces);
			return result;
		}

		/// <summary>
		/// Park the Car
		/// returns: unique identifier (parking ticket)
		/// </summary>
		[HttpPost]
		[Route("ParkTheCar")]
		public IHttpActionResult ParkTheCar()
		{
			IHttpActionResult result;
			if (InMemoryDB.GetOpenSpots() <= 0)
			{ result = BadRequest("no open spaces"); }
			else
			{
				Guid ticket = InMemoryDB.Park();
				result = Ok(ticket);
			}
			return result;
		}

		/// <summary>
		/// Get the parking Fee
		/// returns: cost in cents
		/// </summary>
		[HttpGet]
		[Route("GetParkingFeeInCents")]
		public IHttpActionResult GetParkingFeeInCents(Guid Id)
		{
			IHttpActionResult result;
			if (InMemoryDB.GetTimeParked(Id) == InMemoryDB.BadDate)
			{ result = BadRequest("invalid parking ticket id"); }
			else
			{
				TimeSpan timeSpan = DateTime.Now - InMemoryDB.GetTimeParked(Id);
				int costInCents = Business.GetTodaysrPricing.GetCostInCents(timeSpan);
				result = Ok(costInCents);
			}
			return result;
		}

		/// <summary>
		/// check out
		/// </summary>
		[HttpPut]
		[Route("CheckOut")]
		public IHttpActionResult CheckOut(Guid id)
		{
			IHttpActionResult result;
			if (InMemoryDB.CheckoutSuccess(id))
			{ result = Ok("thank you for your business"); }
			else
			{ result = BadRequest("Invalid ticket"); }
			return result;
		}
	}
}
