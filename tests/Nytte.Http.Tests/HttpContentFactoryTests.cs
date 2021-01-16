using System.Security.Cryptography.Xml;
using System.Text;
using Moq.AutoMock;
using NUnit.Framework;
using Nytte.Testing;
using Shouldly;

namespace Nytte.Http.Tests
{
    public class HttpContentFactoryTests : ServiceUnderTest<IHttpContentFactory, HttpContentFactory>
    {
        private AutoMocker Mocker;
        
        public override void Setup()
        {
            
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

        
    }
}