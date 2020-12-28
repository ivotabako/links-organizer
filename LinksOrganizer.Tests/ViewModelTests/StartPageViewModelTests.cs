using System.Collections.Generic;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Services.Navigation;
using LinksOrganizer.Utils.ClipboardInfo;
using LinksOrganizer.ViewModels;
using Moq;
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
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null);

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
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null);
            
            model.AddLinkItemCommand.Execute(null);

            Assert.NotNull(link);
            Assert.True(string.IsNullOrEmpty(link.Link));
        }

        [Fact]
        public void AddLinkItemCommand_ClipboardHasValidUrl_CreatesLinkWithUrl()
        {
            var url = "http://www.test.com";
            var clipboard = new Mock<IClipboardInfo>();
            clipboard.Setup(c => c.HasText).Returns(true);
            clipboard.Setup(c => c.GetTextAsync()).ReturnsAsync(url);
            var navigationService = new Mock<INavigationService>();
            LinkItem link = null;
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(It.IsAny<LinkItem>()))
                .Callback<object>(l => link = (LinkItem)l);
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null);

            model.AddLinkItemCommand.Execute(null);

            Assert.NotNull(link);
            Assert.Equal(url, link.Link);
        }

        [Fact]
        public void AddLinkItemCommand_ClipboardHasInvalidUrl_CreatesEmptyLink()
        {
            var url = "www.test.com";
            var clipboard = new Mock<IClipboardInfo>();
            clipboard.Setup(c => c.HasText).Returns(true);
            clipboard.Setup(c => c.GetTextAsync()).ReturnsAsync(url);
            var navigationService = new Mock<INavigationService>();
            LinkItem link = null;
            navigationService.Setup(ns => ns.NavigateToAsync<LinkItemViewModel>(It.IsAny<LinkItem>()))
                .Callback<object>(l => link = (LinkItem)l);
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null, null);
            
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
            var model = new StartPageViewModel(null, navigationService.Object, null, null);

            model.LoadLinkItemCommand.Execute(link);

            navigationService.Verify(ns => ns.NavigateToAsync<LinkItemViewModel>(link), Times.Once());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("     ")]
        public void SetSearchedLinkItemsCommand_SearchedTextIsNullOrWhitespace_SearchedLlinksIsNull(string searchResult)
        {
            var model = new StartPageViewModel(null, null, null, null);

            model.SetSearchedLinkItemsCommand.Execute(searchResult);

            Assert.Null(model.SearchedLinks);
        }

        [Fact]
        public void SetSearchedLinkItemsCommand_WithSearchedText_SearchedLlinksIsCorrect()
        {
            var database = new Mock<ILinkItemDatabase>();
            database.Setup(d => d.GetItemsAsync()).ReturnsAsync(new List<LinkItem>
            { 
                new LinkItem{ ID = 0, Name = "Valid", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 1, Name = "valid name", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 2, Name = "Invalid name", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 3, Name = "something else", Info = "", Link ="http://test.com" },
                new LinkItem{ ID = 4, Name = "test", Info = "valid", Link ="http://test.com" },
                new LinkItem{ ID = 5, Name = "test test", Info = "", Link ="http://test.com" },
            });

            var model = new StartPageViewModel(null, null, null, database.Object);
            var expectedIds = new List<int> { 0, 1, 2, 4 };

            model.SetSearchedLinkItemsCommand.Execute("valid");

            Assert.Equal(4, model.SearchedLinks.Count);
            foreach (var id in expectedIds)
            {
                Assert.Contains(model.SearchedLinks, l => l.ID == id);
            }
        }
    }
}
