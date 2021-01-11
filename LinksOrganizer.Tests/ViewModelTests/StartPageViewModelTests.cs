using System;
using System.Collections.Generic;
using System.Linq;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Tests.Utils;
using LinksOrganizer.Themes;
using LinksOrganizer.Utils;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.Utils.ResourcesProvider;
using LinksOrganizer.ViewModels;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Xamarin.Forms;
using Xunit;

namespace LinksOrganizer.Tests.ViewModelTests
{
    public class StartPageViewModelTests
    {
        [Fact]
        public void AddLinkItemCommand_NavigatesToLinkViewItem()
        {
            var clipboard = new Mock<IClipboardInfo>();
            var navigationService = new Mock<INavigationService>();
            LinkItem link = null;
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(It.IsAny<LinkItem>()))
                .Callback<object>(l => link = (LinkItem)l);
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null, null);

            model.AddLinkItemCommand.Execute(null);

            navigationService.Verify(ns => ns.NavigateToAsync<LinkItemViewModel>(link), Times.Once());
        }

        [Fact]
        public void AddLinkItemCommand_EmptyClipboard_CreatesEmptyLink()
        {
            var clipboard = new Mock<IClipboardInfo>();
            var navigationService = new Mock<INavigationService>();
            LinkItem link = null;
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(It.IsAny<LinkItem>()))
                .Callback<object>(l => link = (LinkItem)l);
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null, null);
            
            model.AddLinkItemCommand.Execute(null);

            Assert.NotNull(link);
            Assert.True(string.IsNullOrEmpty(link.Link));
        }

        [Theory]
        [InlineData("http://www.test.com")]
        [InlineData("http://www.test.com/")]
        [InlineData("https://www.test.com")]
        [InlineData("https://www.test.com/")]
        public void AddLinkItemCommand_ClipboardHasValidUrl_CreatesLinkWithUrl(string url)
        {
            var clipboard = new Mock<IClipboardInfo>();
            clipboard.Setup(c => c.HasText).Returns(true);
            clipboard.Setup(c => c.GetTextAsync()).ReturnsAsync(url);
            var navigationService = new Mock<INavigationService>();
            LinkItem link = null;
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(It.IsAny<LinkItem>()))
                .Callback<object>(l => link = (LinkItem)l);
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null, null);

            model.AddLinkItemCommand.Execute(null);

            Assert.NotNull(link);
            Assert.Equal(url, link.Link);
        }

        [Theory]
        [InlineData("http://")]
        [InlineData("https://")]
        [InlineData("test.com")]
        [InlineData("www.test.com")]
        public void AddLinkItemCommand_ClipboardHasInvalidUrl_CreatesEmptyLink(string url)
        {
            var clipboard = new Mock<IClipboardInfo>();
            clipboard.Setup(c => c.HasText).Returns(true);
            clipboard.Setup(c => c.GetTextAsync()).ReturnsAsync(url);
            var navigationService = new Mock<INavigationService>();
            LinkItem link = null;
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(It.IsAny<LinkItem>()))
                .Callback<object>(l => link = (LinkItem)l);
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null, null);
            
            model.AddLinkItemCommand.Execute(null);

            Assert.NotNull(link);
            Assert.True(string.IsNullOrEmpty(link.Link));
        }

        [Fact]
        public void LoadLinkItemCommand_NavigatesToLinkViewItem()
        {
            LinkItem link = new LinkItem();
            var navigationService = new Mock<INavigationService>();
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(link));
            var model = new StartPageViewModel(null, navigationService.Object, null, null, null);

            model.LoadLinkItemCommand.Execute(link);

            navigationService.Verify(ns => ns.NavigateToAsync<LinkItemViewModel>(link), Times.Once());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void SetFavoriteLinksItemsCommand_SearchedTextIsNullOrWhitespace_FavoriteLinksIsNotFiltered(string searchTerm)
        {
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(new List<LinkItem>
            {
                new LinkItem{ ID = 0, Name = "Valid", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 1, Name = "valid name", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 2, Name = "Invalid name", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 3, Name = "something else", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 4, Name = "test", Info = "valid", Link ="http://test.com" },
                new LinkItem{ ID = 5, Name = "test test", Info = "", Link ="http://test.com" },
            });

            var model = new StartPageViewModel(null, null, null, database.Object, null);
            var expectedIds = new List<int> { 0, 1, 2, 3, 4, 5 };

            model.SetFavoriteLinksItemsCommand.Execute(searchTerm);

            Assert.Equal(6, model.FavoriteLinks.Count);
            foreach (var id in expectedIds)
            {
                Assert.Contains(model.FavoriteLinks, l => l.ID == id);
            }
        }

        [Fact]
        public void SetFavoriteLinksItemsCommand_WithSearchedText_FavoriteLinksIsCorrect()
        {
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(new List<LinkItem>
            { 
                new LinkItem{ ID = 0, Name = "Valid", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 1, Name = "valid name", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 2, Name = "Invalid name", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 3, Name = "something else", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 4, Name = "test", Info = "valid", Link ="http://test.com" },
                new LinkItem{ ID = 5, Name = "test test", Info = "", Link ="http://test.com" },
            });

            var model = new StartPageViewModel(null, null, null, database.Object, null);
            var expectedIds = new List<int> { 0, 1, 2, 4 };

            model.SetFavoriteLinksItemsCommand.Execute("valid");

            Assert.Equal(4, model.FavoriteLinks.Count);
            foreach (var id in expectedIds)
            {
                Assert.Contains(model.FavoriteLinks, l => l.ID == id);
            }
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        [InlineData("test")]
        public void SetFavoriteLinksItemsCommand_ChangesFavoriteLinksProperty(string searchTerm)
        {
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(new List<LinkItem>());
            var model = new StartPageViewModel(null, null, null, database.Object, null);
            var harness = new NotifyPropertyChangedHarness(model);

            model.SetFavoriteLinksItemsCommand.Execute(searchTerm);

            Assert.NotEmpty(harness.Changes);
            Assert.Single(harness.Changes);
            Assert.Equal(nameof(model.FavoriteLinks), harness.Changes[0]);
        }

        [Fact]
        public async void InitializeAsync_NavigationDataIsNullAndNoCache_OrdersFavouritesByLastUpdate()
        {
            // Arrange
            var links = new List<LinkItem>
            {
                new LinkItem{ ID = 0, Name = "A", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-5), Rank = 3 },
                new LinkItem{ ID = 1, Name = "B", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-4), Rank = 2 },
                new LinkItem{ ID = 2, Name = "C", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-3), Rank = 1 },
                new LinkItem{ ID = 3, Name = "D", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-2), Rank = 4 },
                new LinkItem{ ID = 4, Name = "E", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-1), Rank = 6 },
                new LinkItem{ ID = 5, Name = "F", Link ="http://test.com", LastUpdatedOn = DateTime.Now, Rank = 5 },
            };
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(links);

            var cache = new Mock<IMemoryCache>();
            cache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(new Mock<ICacheEntry>().Object);
            object isOrderedByRank = false;
            cache.Setup(c => c.TryGetValue(It.IsAny<object>(), out isOrderedByRank))
                .Returns(true);

            var resourcesProvider = new Mock<IResourcesProvider>();
            resourcesProvider.Setup(rp => rp.Resources)
                .Returns(new ResourceDictionary());

            var model = new StartPageViewModel(null, null, cache.Object, database.Object, resourcesProvider.Object);
            var harness = new NotifyPropertyChangedHarness(model);
            var expectedLinks = links.OrderByDescending(l => l.LastUpdatedOn);

            // Act
            await model.InitializeAsync(null);

            // Assert
            Assert.NotEmpty(harness.Changes);
            Assert.Contains(nameof(model.IsOrderedByRank), harness.Changes);
            Assert.Contains(nameof(model.FavoriteLinks), harness.Changes);
            Assert.Equal(expectedLinks, model.FavoriteLinks);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void InitializeAsync_NavigationDataIsNullAndOrderIsReadFromCache_OrdersFavouritesByCacheProperty(object isOrderedByRank)
        {
            // Arrange
            var links = new List<LinkItem>
            {
                new LinkItem{ ID = 0, Name = "A", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-5), Rank = 3 },
                new LinkItem{ ID = 1, Name = "B", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-4), Rank = 2 },
                new LinkItem{ ID = 2, Name = "C", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-3), Rank = 1 },
                new LinkItem{ ID = 3, Name = "D", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-2), Rank = 4 },
                new LinkItem{ ID = 4, Name = "E", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-1), Rank = 6 },
                new LinkItem{ ID = 5, Name = "F", Link ="http://test.com", LastUpdatedOn = DateTime.Now, Rank = 5 },
            };
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(links);

            var cache = new Mock<IMemoryCache>();
            cache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(new Mock<ICacheEntry>().Object);
            cache.Setup(c => c.TryGetValue(It.IsAny<object>(), out isOrderedByRank))
                .Returns(true);

            var resourcesProvider = new Mock<IResourcesProvider>();
            resourcesProvider.Setup(rp => rp.Resources)
                .Returns(new ResourceDictionary());

            var model = new StartPageViewModel(null, null, cache.Object, database.Object, resourcesProvider.Object);
            var harness = new NotifyPropertyChangedHarness(model);
            var expectedLinks = (bool)isOrderedByRank
                ? links.OrderByDescending(l => l.Rank)
                : links.OrderByDescending(l => l.LastUpdatedOn);

            // Act
            await model.InitializeAsync(null);

            // Assert
            Assert.NotEmpty(harness.Changes);
            Assert.Contains(nameof(model.IsOrderedByRank), harness.Changes);
            Assert.Contains(nameof(model.FavoriteLinks), harness.Changes);
            Assert.Equal(expectedLinks, model.FavoriteLinks);
        }

        [Fact]
        public async void InitializeAsyncFromOrderChangeEvent_IsNotOrderedByRank_OrdersFavouritesByLastUpdate()
        {
            // Arrange
            var links = new List<LinkItem>
            {
                new LinkItem{ ID = 0, Name = "A", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-5), Rank = 3 },
                new LinkItem{ ID = 1, Name = "B", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-4), Rank = 2 },
                new LinkItem{ ID = 2, Name = "C", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-3), Rank = 1 },
                new LinkItem{ ID = 3, Name = "D", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-2), Rank = 4 },
                new LinkItem{ ID = 4, Name = "E", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-1), Rank = 6 },
                new LinkItem{ ID = 5, Name = "F", Link ="http://test.com", LastUpdatedOn = DateTime.Now, Rank = 5 },
            };
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(links);

            var cache = new Mock<IMemoryCache>();
            cache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(new Mock<ICacheEntry>().Object);
            object isOrderedByRank = false;
            cache.Setup(c => c.TryGetValue(It.IsAny<object>(), out isOrderedByRank))
                .Returns(true);

            var model = new StartPageViewModel(null, null, cache.Object, database.Object, null);
            var harness = new NotifyPropertyChangedHarness(model);
            var expectedLinks = links.OrderByDescending(l => l.LastUpdatedOn);

            // Act
            await model.InitializeAsync((false, ChangeEvents.OrderChanged));

            // Assert
            Assert.NotEmpty(harness.Changes);
            Assert.Equal(2, harness.Changes.Count);
            Assert.Equal(nameof(model.IsOrderedByRank), harness.Changes[0]);
            Assert.Equal(nameof(model.FavoriteLinks), harness.Changes[1]);
            Assert.Equal(expectedLinks, model.FavoriteLinks);
        }

        [Fact]
        public async void InitializeAsyncFromOrderChangeEvent_IsOrderedByRank_OrdersFavouritesByRank()
        {
            // Arrange
            var links = new List<LinkItem>
            {
                new LinkItem{ ID = 0, Name = "A", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-5), Rank = 3 },
                new LinkItem{ ID = 1, Name = "B", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-4), Rank = 2 },
                new LinkItem{ ID = 2, Name = "C", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-3), Rank = 1 },
                new LinkItem{ ID = 3, Name = "D", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-2), Rank = 4 },
                new LinkItem{ ID = 4, Name = "E", Link ="http://test.com", LastUpdatedOn = DateTime.Now.AddDays(-1), Rank = 6 },
                new LinkItem{ ID = 5, Name = "F", Link ="http://test.com", LastUpdatedOn = DateTime.Now, Rank = 5 },
            };
            var database = new Mock<ILinkItemRepository>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(links);

            var cache = new Mock<IMemoryCache>();
            cache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(new Mock<ICacheEntry>().Object);
            object isOrderedByRank = true;
            cache.Setup(c => c.TryGetValue(It.IsAny<object>(), out isOrderedByRank))
                .Returns(true);

            var model = new StartPageViewModel(null, null, cache.Object, database.Object, null);
            var harness = new NotifyPropertyChangedHarness(model);
            var expectedLinks = links.OrderByDescending(l => l.Rank);

            // Act
            await model.InitializeAsync((true, ChangeEvents.OrderChanged));

            // Assert
            Assert.NotEmpty(harness.Changes);
            Assert.Equal(2, harness.Changes.Count);
            Assert.Equal(nameof(model.IsOrderedByRank), harness.Changes[0]);
            Assert.Equal(nameof(model.FavoriteLinks), harness.Changes[1]);
            Assert.Equal(expectedLinks, model.FavoriteLinks);
        }

        [Theory]
        [InlineData(Theme.LightTheme)]
        [InlineData(Theme.DarkTheme)]
        public async void InitializeAsyncFromThemeChangeEvent_RaisesUpdateTheme(object theme)
        {
            // Arrange
            var cache = new Mock<IMemoryCache>();
            cache.Setup(c => c.CreateEntry(It.IsAny<object>()))
                .Returns(new Mock<ICacheEntry>().Object);
            cache.Setup(c => c.TryGetValue(It.IsAny<object>(), out theme))
                .Returns(true);

            var resourcesProvider = new Mock<IResourcesProvider>();
            resourcesProvider.Setup(rp => rp.Resources)
                .Returns(new ResourceDictionary());

            var model = new StartPageViewModel(null, null, cache.Object, null, resourcesProvider.Object);
            var harness = new NotifyPropertyChangedHarness(model);

            // Act
            await model.InitializeAsync(((Theme)theme, ChangeEvents.ThemeChanged));

            // Assert
            Assert.NotEmpty(harness.Changes);
            Assert.Single(harness.Changes);
            Assert.Equal(nameof(model.Theme), harness.Changes[0]);
            Assert.Equal(theme, model.Theme);
        }
    }
}
