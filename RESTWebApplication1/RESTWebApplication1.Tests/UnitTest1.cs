using System;
using System.Threading;
using System.Web.Http;
using System.Web.Http.Results;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RESTWebApplication1.Controllers;

namespace RESTWebApplication1.Tests
{
	[TestClass]
	public class UnitTest1
	{
		private int _lotCapacity = 200;

		[TestMethod]
		public void CheckLotCountAccuratedOneCycleSuccess()
		{
			var controller = new RESTWebApplication1.Controllers.ParkController();
			int lotVacancy = GetLotVacancy();
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(lotVacancy, _lotCapacity);

			Guid carGuid = ParkTheCar(controller);
			lotVacancy = GetLotVacancy();
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(lotVacancy, _lotCapacity-1);

			var paidCar = controller.CheckOut(carGuid);
			lotVacancy = GetLotVacancy();
			Microsoft.VisualStudio.TestTools.UnitTesting.Assert.AreEqual(lotVacancy, _lotCapacity);
		}

		[TestMethod]
		public void	CheckTwoCarsHaveDifferentTicketsSuccess()
		{
			var controller = new RESTWebApplication1.Controllers.ParkController();
			Guid carGuid1 = ParkTheCar(controller);
			Guid carGuid2 = ParkTheCar(controller);
			Assert.AreNotEqual(carGuid1, carGuid2);
		}


		[TestMethod]
		public void CheckPaymentCorrectSuccess()
		{
			var controller = new RESTWebApplication1.Controllers.ParkController();
			Guid carGuid = ParkTheCar(controller);
			int cost = GetCostInCents(carGuid);
			Assert.AreEqual(cost, 500);
			InMemoryDB.IncrementDateByMinutes(carGuid, -121);
			int cost2 = GetCostInCents(carGuid);
			Assert.AreEqual(cost2, 1000);
			InMemoryDB.IncrementDateByMinutes(carGuid, -480);
			int cost3 = GetCostInCents(carGuid);
			Assert.AreEqual(cost3, 1500);
		}

		[TestMethod]
		public void CheckLotFilledToCapacitySuccess()
		{
			InMemoryDB.FlushLot();
			InMemoryDB.SetCapacity(2);
			var controller = new RESTWebApplication1.Controllers.ParkController();
			Guid throwAwayGuid = ParkTheCar(controller);
			Guid throwAwayGuid2 = ParkTheCar(controller);
			var lotVacancy = GetLotVacancy();
			Assert.AreEqual(lotVacancy, 0);
		}

		[TestMethod]
		public void ASkForCostWithBogusCarFailure()
		{
			InMemoryDB.FlushLot();
			var controller = new RESTWebApplication1.Controllers.ParkController();
			Guid throwAwayGuid = ParkTheCar(controller);
			var result = controller.GetParkingFeeInCents(Guid.NewGuid());
			var message = result.As<BadRequestErrorMessageResult>().Message;
		}

		[TestMethod]
		public void ASkForCheckOutWithBogusCarFailure()
		{
			InMemoryDB.FlushLot();
			var controller = new RESTWebApplication1.Controllers.ParkController();
			Guid throwAwayGuid = ParkTheCar(controller);
			var result = controller.CheckOut(Guid.NewGuid());
			var message = result.As<BadRequestErrorMessageResult>().Message;
		}

		[TestMethod]
		public void CheckCannotParkWhenFullFailure()
		{
			InMemoryDB.FlushLot();
			InMemoryDB.SetCapacity(2);
			var controller = new RESTWebApplication1.Controllers.ParkController();
			Guid throwAwayGuid = ParkTheCar(controller);
			Guid throwAwayGuid2 = ParkTheCar(controller);
			var expectedFailuteResult = controller.ParkTheCar();
			var message = expectedFailuteResult.As<BadRequestErrorMessageResult>().Message;
		}


		private static int GetLotVacancy()
		{
			var controller = new RESTWebApplication1.Controllers.ParkController();
			IHttpActionResult result = controller.EmptyCount();
			OkNegotiatedContentResult<int> conNegResult = Xunit.Assert.IsType<OkNegotiatedContentResult<int>>(result);
			var lotVacancy = conNegResult.Content;
			return lotVacancy;
		}

		private static Guid ParkTheCar(ParkController controller)
		{
			var car = controller.ParkTheCar();
			OkNegotiatedContentResult<Guid> conNegResult = Xunit.Assert.IsType<OkNegotiatedContentResult<Guid>>(car);
			var carGuid = conNegResult.Content;
			return carGuid;
		}

		private static int GetCostInCents(Guid carGuid)
		{
			var controller2 = new RESTWebApplication1.Controllers.ParkController();
			var result = controller2.GetParkingFeeInCents(carGuid);
			OkNegotiatedContentResult<int> costInCents = Xunit.Assert.IsType<OkNegotiatedContentResult<int>>(result);
			var cost = costInCents.Content;
			return cost;
		}
	}
}
