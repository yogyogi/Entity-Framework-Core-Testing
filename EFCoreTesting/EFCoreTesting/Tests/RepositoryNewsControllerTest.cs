using EFCoreTesting.Controllers;
using EFCoreTesting.Models;
using Moq;
using Xunit;

namespace EFCoreTesting.Tests
{
    public class RepositoryNewsControllerTest
    {
        [Fact]
        public async Task GetNews()
        {
            // Arrange
            var repositoryMock = new Mock<INewsRepository>();
            repositoryMock
                .Setup(r => r.GetNewsByNameAsync("Elon Musk worth soared to $1 trillion"))
                .Returns(Task.FromResult(new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" }));

            var controller = new NewsControllerWithRepository(repositoryMock.Object);

            // Act
            var news = await controller.GetNews("Elon Musk worth soared to $1 trillion");

            // Assert
            repositoryMock.Verify(r => r.GetNewsByNameAsync("Elon Musk worth soared to $1 trillion"));
            Assert.Equal("https://newsabc.com/elon-musk-trillionaire/", news.Url);
        }

        [Fact]
        public async Task GetAllNews()
        {
            // Arrange
            var repositoryMock = new Mock<INewsRepository>();
            repositoryMock
                .Setup(r => r.GetAllNewsAsync())
                .Returns(new[]
                {
                new News { Name = "Donald Trump wins 2025 USA Presidential Election", Url = "https://newsabc.com/usa-2025-donald-trump/" },
                new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" }
                }.ToAsyncEnumerable());

            var controller = new NewsControllerWithRepository(repositoryMock.Object);

            // Act
            var news = await controller.GetAllNews().ToListAsync();

            // Assert
            repositoryMock.Verify(r => r.GetAllNewsAsync());
            Assert.Equal("https://newsabc.com/usa-2025-donald-trump/", news[0].Url);
            Assert.Equal("https://newsabc.com/elon-musk-trillionaire/", news[1].Url);
        }

        [Fact]
        public async Task AddNews()
        {
            // Arrange
            var repositoryMock = new Mock<INewsRepository>();
            var controller = new NewsControllerWithRepository(repositoryMock.Object);

            // Act
            await controller.AddNews("Elon Musk worth soared to $1 trillion", "https://newsabc.com/elon-musk-trillionaire/");

            // Assert
            repositoryMock.Verify(r => r.AddNews(It.IsAny<News>()));
            repositoryMock.Verify(r => r.SaveChangesAsync());
        }

        [Fact]
        public async Task UpdateNewsUrl()
        {
            var news = new News { Name = "Elon Musk worth soared to $1 trillion", Url = "https://newsabc.com/elon-musk-trillionaire/" };

            // Arrange
            var repositoryMock = new Mock<INewsRepository>();
            repositoryMock
                .Setup(r => r.GetNewsByNameAsync("Elon Musk worth soared to $1 trillion"))
                .Returns(Task.FromResult(news));

            var controller = new NewsControllerWithRepository(repositoryMock.Object);

            // Act
            await controller.UpdateNewsUrl("Elon Musk worth soared to $1 trillion", "https://newsabc.com/elon-musk-trillionaire-news-updated/");

            // Assert
            repositoryMock.Verify(r => r.GetNewsByNameAsync("Elon Musk worth soared to $1 trillion"));
            repositoryMock.Verify(r => r.SaveChangesAsync());
            Assert.Equal("https://newsabc.com/elon-musk-trillionaire-news-updated/", news.Url);
        }
    }
}
