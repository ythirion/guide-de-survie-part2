using System.Collections.Generic;
using Application.Domain.Book;
using Application.Domain.Country;
using Application.Purchase;
using Application.Report;
using Application.Tests.Storage;
using FluentAssertions;
using Xunit;

namespace Application.Tests.Report
{
    public class ReportGeneratorTest
    {
        private readonly InMemoryRepository _inMemoryRepository;

        public ReportGeneratorTest()
        {
            _inMemoryRepository = new InMemoryRepository();
            MainRepository.Override(_inMemoryRepository);
        }

        [Fact]
        public void Converts_total_amount_to_usd()
        {
            // Arrange
            var reportGenerator = new ReportGenerator();
            var novel = new Novel(
                "Le Horla",
                12.99,
                new Author(
                    "Guy de Maupassant",
                    new Country("France", Currency.Euro, Language.French)
                ),
                Language.French,
                new List<Genre>
                {
                    Genre.Romance
                });

            var educationalBook = new EducationalBook(
                "Clean Code",
                29.87,
                new Author(
                    "Uncle Bob",
                    new Country("USA", Currency.UsDollar, Language.English)
                ),
                Language.English,
                Category.Computer);

            var novelPurchasedBook = new PurchasedBook(novel, 2);
            var educationalPurchasedBook = new PurchasedBook(educationalBook, 7);

            var invoice = new Invoice(
                "John Doe",
                new Country(
                    "France",
                    Currency.Euro,
                    Language.French
                )
            );

            // Act
            invoice.AddPurchasedBook(novelPurchasedBook);
            invoice.AddPurchasedBook(educationalPurchasedBook);

            _inMemoryRepository.AddInvoice(invoice);

            // Assert
            reportGenerator.GetTotalAmount().Should().Be(334.97);
            reportGenerator.GetNumberOfIssuedInvoices().Should().Be(1);
            reportGenerator.GetTotalSoldBooks().Should().Be(9);
        }

        [Fact]
        public void Converts_total_amount_to_usd_with_test_data_builders()
        {
            // Using the Test Data Builder pattern:
            // Rewrite the test above
        }
    }
}