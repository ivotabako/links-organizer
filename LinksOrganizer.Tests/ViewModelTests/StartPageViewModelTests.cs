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
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null);

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
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null);
            
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
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null);

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
            var model = new StartPageViewModel(clipboard.Object, navigationService.Object, null);
            
            model.AddLinkItemCommand.Execute(null);

            Assert.NotNull(link);
            Assert.True(string.IsNullOrEmpty(link.Link));
        }
    }
}
