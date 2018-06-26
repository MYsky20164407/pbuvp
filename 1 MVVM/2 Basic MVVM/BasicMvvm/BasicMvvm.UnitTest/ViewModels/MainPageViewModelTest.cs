using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicMvvm.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BasicMvvm.Services;
using BasicMvvm.ViewModels;

namespace BasicMvvm.UnitTest.ViewModels {
    [TestClass]
    public class MainPageViewModelTest {
        [TestMethod]
        public void TestListCommand() {
            var contacts = new Contact[] {
                new Contact {FirstName = "Elmer", LastName = "Stella"},
                new Contact {FirstName = "Hugo", LastName = "Vincent"},
                new Contact {FirstName = "Rachel", LastName = "Walker"}
            };
            var stubIContactService =
                new StubIContactService().ListAsync(async () => contacts);

            var viewModel = new MainPageViewModel(stubIContactService);
            viewModel.ListCommand.Execute(null);

            Assert.AreEqual(contacts.Length, viewModel.ContactCollection.Count);
            for (int i = 0; i < contacts.Length; i++) {
                Assert.AreSame(contacts[i], viewModel.ContactCollection[i]);
            }
        }

        [TestMethod]
        public void TestUpdateCommand() {
            Contact savedContact = null;
            var contact = new Contact
                {FirstName = "Elmer", LastName = "Stella"};

            var stubIContactService =
                new StubIContactService().UpdateAsync(async (c) =>
                    savedContact = c);

            var viewModel = new MainPageViewModel(stubIContactService);
            viewModel.UpdateCommand.Execute(contact);

            Assert.AreSame(contact, savedContact);
        }

        [TestMethod]
        public void TestSelectedContactChangedEvent() {
            var eventFired = false;

            var viewModel = new MainPageViewModel(null);
            viewModel.PropertyChanged += (sender, args) => eventFired = true;

            viewModel.SelectedContact = new Contact();
            Assert.IsTrue(eventFired);
        }
    }
}