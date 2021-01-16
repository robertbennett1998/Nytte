using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Moq.AutoMock;
using NUnit.Framework;
using Nytte.Http.Wrappers;
using Nytte.Testing;
using Nytte.Wrappers;
using Shouldly;

namespace Nytte.Http.Tests
{
    public class EnhancedHttpClientTests : ServiceUnderTest<IHttpClient, EnhancedHttpClient>
    {
        private Mock<IHttpClientWrapper> _client;
        private HttpResponseMessage _response;
        private string _url;
        private string _json;
        private Mock<HttpContent> _content;
        private Mock<IJson> _jsonWrapper;
        private ReturnType _returnType;

        record SendType();

        record ReturnType();

        [SetUp]
        public override void Setup()
        {
            _client = Mocker.GetMock<IHttpClientWrapper>();
            _response = new HttpResponseMessage();
            _url = "url";
            _json = "some json";
            _content = new Mock<HttpContent>();
            Mocker.GetMock<IHttpContentFactory>().Setup(o => o.CreateJsonContent(_json)).Returns(_content.Object);
            Mocker.GetMock<IJson>().Setup(o => o.SerializeAsync(It.IsAny<object>())).ReturnsAsync(_json);
            _jsonWrapper = Mocker.GetMock<IJson>();
            _returnType = new ReturnType();
            _jsonWrapper.Setup(o => o.DeserializeAsync<ReturnType>(_json)).ReturnsAsync(_returnType);
        }

        #region Post Tests

        [Test]
        public async Task PostAsyncTR_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.PostAsync(_url, _content.Object)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.PostAsync<SendType, ReturnType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Data.ShouldBeNull();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task PostAsyncTR_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            var type = new ReturnType();
            _jsonWrapper.Setup(o => o.DeserializeAsync<ReturnType>(_json)).ReturnsAsync(type);
            _client.Setup(o => o.PostAsync(_url, _content.Object)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.PostAsync<SendType, ReturnType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Data.ShouldBe(type);
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task PostAsyncTR_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            
            SetFailStatusCode();
            
            _client.Setup(o => o.PostAsync(_url, _content.Object)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.PostAsync<SendType, ReturnType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Data.ShouldBeNull();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task PostAsyncT_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.PostAsync(_url, _content.Object)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.PostAsync<SendType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task PostAsyncT_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.PostAsync(_url, _content.Object)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.PostAsync<SendType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task PostAsyncT_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            
            SetFailStatusCode();
            
            _client.Setup(o => o.PostAsync(_url, _content.Object)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.PostAsync<SendType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }

        #endregion

        #region Get Tests

        [Test]
        public async Task GetAsyncT_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.GetAsync(_url)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.GetAsync<ReturnType>(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task GetAsyncT_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.GetAsync(_url)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.GetAsync<ReturnType>(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task GetAsyncT_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            SetFailStatusCode();
            
            
            _client.Setup(o => o.GetAsync(_url)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.GetAsync<ReturnType>(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task GetAsync_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.GetAsync(_url)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.GetAsync(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task GetAsync_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.GetAsync(_url)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.GetAsync(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task GetAsync_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            SetFailStatusCode();
            
            _client.Setup(o => o.GetAsync(_url)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.GetAsync(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }

        #endregion

        #region Delete Tests

        [Test]
        public async Task DeleteAsyncT_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.DeleteAsync(_url)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.DeleteAsync<ReturnType>(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task DeleteAsyncT_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.DeleteAsync(_url)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.DeleteAsync<ReturnType>(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task DeleteAsyncT_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            SetFailStatusCode();
            
            _client.Setup(o => o.DeleteAsync(_url)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.DeleteAsync<ReturnType>(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task DeleteAsync_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.DeleteAsync(_url)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.DeleteAsync(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task DeleteAsync_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.DeleteAsync(_url)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.DeleteAsync(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task DeleteAsync_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            SetFailStatusCode();
            
            _client.Setup(o => o.DeleteAsync(_url)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.DeleteAsync(_url);
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }

        #endregion
        
        #region Put Tests

        [Test]
        public async Task PutAsyncTR_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.PutAsync(_url, _content.Object)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.PutAsync<SendType, ReturnType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Data.ShouldBeNull();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task PutAsyncTR_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            var type = new ReturnType();
            _jsonWrapper.Setup(o => o.DeserializeAsync<ReturnType>(_json)).ReturnsAsync(type);
            _client.Setup(o => o.PutAsync(_url, _content.Object)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.PutAsync<SendType, ReturnType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Data.ShouldBe(type);
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task PutAsyncTR_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            
            SetFailStatusCode();
            
            _client.Setup(o => o.PutAsync(_url, _content.Object)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.PutAsync<SendType, ReturnType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Data.ShouldBeNull();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task PutAsyncT_RefusedConnection_ReturnsRefusedResponse()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.PutAsync(_url, _content.Object)).Throws(new HttpRequestException());
            
            //Act
            var response = await sut.PutAsync<SendType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeTrue();
            response.Message.ShouldBeNull();
        }
        
        [Test]
        public async Task PutAsyncT_SuccessStatusCode_ReturnsType()
        {
            //Arrange
            var sut = CreateSut();
            _client.Setup(o => o.PutAsync(_url, _content.Object)).ReturnsAsync(_response);
            SetSuccessStatusCode();
            SetContent();
            //Act
            var response = await sut.PutAsync<SendType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }
        
        [Test]
        public async Task PutAsyncT_NonSuccessStatsCode_ReturnsMessage()
        {
            //Arrange
            var sut = CreateSut();
            
            SetFailStatusCode();
            
            _client.Setup(o => o.PutAsync(_url, _content.Object)).ReturnsAsync(_response);
            
            //Act
            var response = await sut.PutAsync<SendType>(_url, new SendType());
            
            //Assert
            response.RefusedConnection.ShouldBeFalse();
            response.Message.ShouldBe(_response);
        }

        #endregion

        private void SetContent() =>
            _response.Content = new StringContent(_json, Encoding.UTF8, Constants.ApplicationJson);

        private void SetSuccessStatusCode() => _response.StatusCode = HttpStatusCode.OK;

        private void SetFailStatusCode() => _response.StatusCode = HttpStatusCode.BadRequest;
    }
}