using System;
using System.Collections.Generic;
using System.Linq;
using RESTWebApplication1.Models;

namespace RESTWebApplication1
{
	public static class InMemoryDB
	{
		static List<ParkingTicket> roster = new List<ParkingTicket>();
		static int _capacity = 200;
		public static DateTime BadDate = new DateTime(1900, 1, 1, 0, 0, 0);
		public static Guid Park()
		{
			if (roster.Count >= _capacity)
			{ throw new Exception("lot is full"); }
			ParkingTicket ticket = new ParkingTicket()
			{
				Identifier = Guid.NewGuid(),
				TimeIn = DateTime.Now
			};
			roster.Add(ticket);
			return ticket.Identifier;
		}

		public static int GetOpenSpots()
		{
			return _capacity - roster.Count();
		}

		public static DateTime GetTimeParked(Guid Identifier)
		{
			DateTime answer = BadDate;
			var car = roster.Where(t => t.Identifier == Identifier);
			if (car.Count() > 0)
			{
				answer = car.Single().TimeIn;
			}
			return answer;
		}

		public static bool CheckoutSuccess(Guid Identifier)
		{
			bool success = false;
			var car = roster.Where(t => t.Identifier == Identifier);
			if (car.Count() > 0)
			{
				roster.Remove(car.Single());
				success = true;
			}
			return success;
		}

		public static bool IncrementDateByMinutes(Guid Identifier, int minutes)
		{
			bool success = false;
			var car = roster.Where(t => t.Identifier == Identifier);
			if (car.Count() > 0)
			{
				car.Single().TimeIn = car.Single().TimeIn.AddMinutes(minutes);
				success = true;
			}
			return success;
		}

		public static void SetCapacity(int Capacity)
		{
			_capacity = Capacity;
		}

		public static void FlushLot()
		{
			roster = new List<ParkingTicket>();
		}
	}


}