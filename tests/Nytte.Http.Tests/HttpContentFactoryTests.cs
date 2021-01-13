using System.Security.Cryptography.Xml;
using System.Text;
using Moq.AutoMock;
using NUnit.Framework;
using Shouldly;

namespace Nytte.Http.Tests
{
    public class HttpContentFactoryTests
    {
        private AutoMocker _mocker;

        [SetUp]
        public void Setup()
        {
            _mocker = new AutoMocker();
        }

        [Test]
        public void CreateJsonContentAsync_Always_CreatesContent()
        {
            //Arrange
            var sut = CreateSut();
            var json = "json";

            //Act
            var content = sut.CreateJsonContent(json);
            
            //Assert
            var resultContent = content.ReadAsStringAsync().Result;
            resultContent.ShouldBe(json);
            content.Headers.ContentType.MediaType.ShouldBe(Constants.ApplicationJson);
            content.Headers.ContentType.CharSet.ShouldBe(Encoding.UTF8.WebName);
        }

        private IHttpContentFactory CreateSut() => _mocker.CreateInstance<HttpContentFactory>();
    }
}