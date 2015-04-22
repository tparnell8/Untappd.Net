﻿using System.Collections.Generic;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using RestSharp;
using Untappd.Net.Client;
using Untappd.Net.Request;
using Untappd.Net.Responses.BeerInfo;

namespace Untappd.Net.UnitTests.Request
{
    [TestFixture]
    public class TestRepository
    {
        [Test]
        public void ConfirmRequestWorks()
        {
            var mockCreds = new Mock<IUnAuthenticatedUntappdCredentials>();
            mockCreds.Setup(a => a.ClientId).Returns("id");
            mockCreds.Setup(a => a.ClientSecret).Returns("secret");
            var bodyParam = new Dictionary<string, string> {{"key", "value"}};
            var client = new Mock<IRestClient>();
            var request = new Mock<IRestRequest>();
            request.Setup(a => a.AddParameter(It.IsAny<string>(), It.IsAny<string>()));
            request.Setup(a => a.AddParameter(It.IsAny<string>(), It.IsAny<string>()));
           
            var response = new Mock<IRestResponse>();
            var obj = JsonConvert.SerializeObject(new BeerInfo());
            response.Setup(a => a.Content).Returns(obj);
            client.Setup(a => a.Execute(It.IsAny<IRestRequest>())).Callback(() =>
            {
            }).Returns(response.Object);
            var repository = new Repository(client.Object, request.Object);
           
            repository.Get<BeerInfo>(mockCreds.Object, "awesome", bodyParam);
            request.Verify(a => a.AddParameter("client_id", mockCreds.Object.ClientId));
            request.Verify(a => a.AddParameter("client_secret", mockCreds.Object.ClientSecret));
            request.Verify(a => a.AddParameter("key", "value"));
        }

        [Test]
        public void ConfirmBasicConstructorWorks()
        {
            var constructorTest = new Repository();
            Assert.IsTrue(constructorTest.Client != null);
            Assert.IsTrue(constructorTest.Request != null);
        }
    }
}
