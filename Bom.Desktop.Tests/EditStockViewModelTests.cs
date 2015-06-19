﻿using Bom.Client.Contracts;
using Bom.Client.Entities;
using Bom.Desktop.ViewModels;
using Core.Common.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Bom.Desktop.Tests
{
    [TestClass]
    public class EditStockViewModelTests
    {
        [TestMethod]
        public void TestViewModelConstruction()
        {
            StockItemData stock = TestHelper.GetTestStockItemData();

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditStockViewModel viewModel = new EditStockViewModel(mockServiceFactory.Object, stock);

            Assert.IsTrue(viewModel.Stock != null);
            Assert.IsTrue(viewModel.Stock.Id == stock.StockId && viewModel.Stock.Notes == stock.Notes);
        }

        [TestMethod]
        public void TestSaveCommand()
        {
            StockItemData stock = TestHelper.GetTestStockItemData();

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditStockViewModel viewModel = new EditStockViewModel(mockServiceFactory.Object, stock);

            mockServiceFactory.Setup(mock => mock.CreateClient<IStockService>().UpdateStock(It.IsAny<Stock>())).Returns(viewModel.Stock);

            viewModel.Stock.Notes = "Black";

            bool stockUpdated = false;
            string color = string.Empty;
            viewModel.StockUpdated += (s, e) =>
            {
                stockUpdated = true;
                color = e.Stock.Notes;
            };

            viewModel.SaveCommand.Execute(null);

            Assert.IsTrue(stockUpdated);
            Assert.IsTrue(color == "Black");
        }

        [TestMethod]
        public void TestCanSaveCommand()
        {
            StockItemData stock = TestHelper.GetTestStockItemData();

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditStockViewModel viewModel = new EditStockViewModel(mockServiceFactory.Object, stock);

            Assert.IsFalse(viewModel.SaveCommand.CanExecute(null));

            viewModel.Stock.Notes = "Black";

            Assert.IsTrue(viewModel.SaveCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestStockIsValid()
        {
            StockItemData stock = TestHelper.GetTestStockItemData();

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditStockViewModel viewModel = new EditStockViewModel(mockServiceFactory.Object, stock);

            viewModel.Stock.PartId = 0;
            Assert.IsFalse(viewModel.Stock.IsValid);

            viewModel.Stock.PartId = 1;
            Assert.IsTrue(viewModel.Stock.IsValid);
        }

        [TestMethod]
        public void TestCancelCommand()
        {
            StockItemData stock = new StockItemData { StockId = 1, Notes = "White" };

            Mock<IServiceFactory> mockServiceFactory = new Mock<IServiceFactory>();

            EditStockViewModel viewModel = new EditStockViewModel(mockServiceFactory.Object, stock);

            bool canceled = false;
            viewModel.CancelEditStock += (s, e) => canceled = true;

            Assert.IsTrue(!canceled);

            viewModel.CancelCommand.Execute(null);

            Assert.IsTrue(viewModel.CancelCommand.CanExecute(null));

            Assert.IsTrue(canceled);
        }
    }
}
