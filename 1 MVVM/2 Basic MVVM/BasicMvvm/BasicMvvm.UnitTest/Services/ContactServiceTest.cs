using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasicMvvm.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicMvvm.UnitTest.Services {
    [TestClass]
    public class ContectServiceTest {
        [TestMethod]
        public async Task TestListAsync() {
            var contactService = new ContactService();

            var contacts = (await contactService.ListAsync()).ToArray();
            Assert.AreEqual(contacts.Length, 12);

            var firstContact = contacts[0];
            Assert.AreEqual("Theodore", firstContact.FirstName);
            Assert.AreEqual("Humphrey", firstContact.LastName);
            Assert.AreEqual(new DateTime(1984, 1, 5), firstContact.Birthday);
            Assert.AreEqual("http://facebook.com/th", firstContact.Link);
            Assert.AreEqual("http://localhost:54652/images/1.jpg", firstContact.Avatar);
            Assert.AreEqual(
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit.",
                firstContact.Message);
        }

        [TestMethod]
        public async Task TestUpdateAsync() {
            var contactService = new ContactService();

            var firstContact = (await contactService.ListAsync()).First();
            Assert.AreEqual(firstContact.FirstName, "Theodore");

            firstContact.FirstName = "Bill";
            await contactService.UpdateAsync(firstContact);
            Assert.AreEqual(
                (await contactService.ListAsync()).First().FirstName, "Bill");
        }
    }
}