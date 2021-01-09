using System;
using System.Threading.Tasks;
using LinksOrganizer.Data;
using LinksOrganizer.Models;
using LinksOrganizer.Tests.Utils;
using LinksOrganizer.ViewModels;
using Moq;
using Xunit;

namespace LinksOrganizer.Tests.ViewModelTests
{
    public class LinkItemViewModelTests
    {
        [Fact]
        public void CanSave_LinkIsNull_IsFalse()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            model.Name = "some name";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_NameIsNull_IsFalse()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            model.Link = "http://somelink.com";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_LinkIsInvalidUrl_IsFalse()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            model.Name = "some name";
            model.Link = "somelink.com";

            Assert.False(model.CanSave);
        }

        [Fact]
        public void CanSave_LinkAndNameAreValid_IsTrue()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            model.Name = "some name";
            model.Link = "http://somelink.com";

            Assert.True(model.CanSave);
        }

        [Fact]
        public void SettingName_NewValue_NotifiesPropertyChanged()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            var harness = new NotifyPropertyChangedHarness(model);

            model.Name = "some name";

            Assert.NotNull(harness.Changes);
            Assert.Single(harness.Changes);
            Assert.Equal(nameof(model.Name), harness.Changes[0]);
        }

        [Fact]
        public void SettingName_SameValue_DoesNotNotifyPropertyChanged()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            var name = "My name";
            model.Name = name;
            var harness = new NotifyPropertyChangedHarness(model);

            model.Name = name;

            Assert.Empty(harness.Changes);
        }

        [Fact]
        public void SettingLink_NewValue_NotifiesPropertyChanged()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            var harness = new NotifyPropertyChangedHarness(model);

            model.Link = "http://link.com";

            Assert.NotNull(harness.Changes);
            Assert.Single(harness.Changes);
            Assert.Equal(nameof(model.Link), harness.Changes[0]);
        }

        [Fact]
        public void SettingLink_SameValue_DoesNotNotifyPropertyChanged()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            var link = "http://link.com";
            model.Link = link;
            var harness = new NotifyPropertyChangedHarness(model);

            model.Link = link;

            Assert.Empty(harness.Changes);
        }

        [Fact]
        public void SettingInfo_NewValue_NotifiesPropertyChanged()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            var harness = new NotifyPropertyChangedHarness(model);

            model.Info = "some info";

            Assert.NotNull(harness.Changes);
            Assert.Single(harness.Changes);
            Assert.Equal(nameof(model.Info), harness.Changes[0]);
        }

        [Fact]
        public void SettingInfo_SameValue_DoesNotNotifyPropertyChanged()
        {
            var model = new LinkItemViewModel(null, null, null, null, null);
            var info = "some info";
            model.Info = info;
            var harness = new NotifyPropertyChangedHarness(model);

            model.Info = info;

            Assert.Empty(harness.Changes);
        }

        [Fact]
        public async Task InitializeAsync_DataIdIsNotZero_UpdatesDatabase()
        {
            // Arrange
            var database = new Mock<ILinkItemDatabase>();
            var model = new LinkItemViewModel(null, null, database.Object, null, null);
            var linkItem = new LinkItem
            {
                ID = 1
            };

            // Act
            await model.InitializeAsync(linkItem);

            // Assert
            database.Verify(d => d.SaveItemAsync(linkItem), Times.Once);
        }

        [Fact]
        public async Task InitializeAsync_DataIdIsZero_DoesNotUpdateDatabase  ()
        {
            // Arrange
            var database = new Mock<ILinkItemDatabase>();
            var model = new LinkItemViewModel(null, null, database.Object, null, null);
            var linkItem = new LinkItem
            {
                ID = 0
            };

            // Act
            await model.InitializeAsync(linkItem);

            // Assert
            database.Verify(d => d.SaveItemAsync(linkItem), Times.Never);
        }

        [Fact]
        public async Task InitializeAsync_DataIsValid_PropertiesAreSetFromData()
        {
            // Arrange
            var model = new LinkItemViewModel(null, null, new Mock<ILinkItemDatabase>().Object, null, null);
            var linkItem = new LinkItem
            {
                ID = 1,
                Info = "some info",
                Link = "http://link.com",
                Name = "some name",
                Rank = 6,
                LastUpdatedOn = DateTime.Now.AddDays(-5)
            };

            // Act
            await model.InitializeAsync(linkItem);

            // Assert
            Assert.Equal(linkItem.ID, model.Id);
            Assert.Equal(linkItem.Info, model.Info);
            Assert.Equal(linkItem.Link, model.Link);
            Assert.Equal(linkItem.Name, model.Name);
            Assert.Equal(linkItem.Rank, model.Rank);
            Assert.Equal(linkItem.LastUpdatedOn, model.LastUpdatedOn);
        }

        [Fact]
        public async Task InitializeAsync_DataIsNotValid_PropertiesAreNotSet()
        {
            // Arrange
            var model = new LinkItemViewModel(null, null, new Mock<ILinkItemDatabase>().Object, null, null);

            // Act
            await model.InitializeAsync(null);

            // Assert
            Assert.Equal(default(int), model.Id);
            Assert.Equal(default(string), model.Info);
            Assert.Equal(default(string), model.Link);
            Assert.Equal(default(string), model.Name);
            Assert.Equal(default(int), model.Rank);
            Assert.Equal(default(DateTime), model.LastUpdatedOn);
        }

        [Fact]
        public async Task InitializeAsync_DataIsFromDatabase_CanSaveAndCanDelete()
        {
            var model = new LinkItemViewModel(null, null, new Mock<ILinkItemDatabase>().Object, null, null);
            var linkItem = new LinkItem
            {
                ID = 1,
                Link = "http://link.com",
                Name = "some name",
            };

            await model.InitializeAsync(linkItem);

            Assert.True(model.CanSave);
            Assert.True(model.CanDelete);
        }

        [Fact]
        public async Task InitializeAsync_DataIsFromDatabase_IncrementsRank()
        {
            var model = new LinkItemViewModel(null, null, new Mock<ILinkItemDatabase>().Object, null, null);
            var rank = 5;
            var linkItem = new LinkItem
            {
                ID = 1,
                Rank = rank,
            };

            await model.InitializeAsync(linkItem);

            Assert.Equal(rank + 1, model.Rank);
        }

        [Fact]
        public async Task InitializeAsync_NewData_CannotSaveCannotDelete()
        {
            var model = new LinkItemViewModel(null, null, new Mock<ILinkItemDatabase>().Object, null, null);
            var linkItem = new LinkItem();

            await model.InitializeAsync(linkItem);

            Assert.False(model.CanSave);
            Assert.False(model.CanDelete);
        }
    }
}
