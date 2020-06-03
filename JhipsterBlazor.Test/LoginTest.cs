using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Bunit;
using Bunit.Mocking.JSInterop;
using JhipsterBlazor.Models;
using JhipsterBlazor.Pages;
using JhipsterBlazor.Services;
using JhipsterBlazor.Test.Helpers;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using static Bunit.ComponentParameterFactory;

namespace JhipsterBlazor.Test
{
    /// <summary>
    /// These tests are written entirely in C#.
    /// Learn more at https://bunit.egilhansen.com/docs/
    /// </summary>
    public class LoginTest : TestContext
    {
        [Fact]
        public void Should_CallSignInMethod_When_LoginFormIsSubmit()
        {
            // Arrange
            var loginModel = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };
            var authenticationService = new Mock<MockAuthenticationService>();
            authenticationService.Setup(authenticationService =>
                authenticationService.SignIn(It.IsAny<LoginModel>()));
            Services.AddSingleton<AuthenticationStateProvider>(authenticationService.Object);

            var login = RenderComponent<Login>();
            login.Instance.LoginModel = loginModel;

            // Act
            login.Find("form").Submit();

            // Assert
            authenticationService.Verify(mock => mock.SignIn(It.Is<LoginModel>(model =>
                    model.Username.Equals(loginModel.Username) && model.Password.Equals(loginModel.Password))),
                Times.Once());
        }

        [Fact]
        public void Should_DisplayFailed_When_CredentialIsInvalid()
        {
            // Arrange
            var loginModel = new LoginModel()
            {
                Username = "test",
                Password = "test"
            };
            var authenticationService = new Mock<MockAuthenticationService>();
            authenticationService.Setup(authenticationService =>
                authenticationService.SignIn(It.IsAny<LoginModel>())).Returns(Task.FromResult(false));
            Services.AddSingleton<AuthenticationStateProvider>(authenticationService.Object);

            var login = RenderComponent<Login>();
            login.Instance.LoginModel = loginModel;

            // Act
            login.Find("form").Submit();

            // Assert
            login.Find("div").Children.First().MarkupMatches(
                @"<div class=""col-md-8"">
                    <div class=""alert alert-danger"">
                        <strong>Failed to sign in!</strong> Please check your credentials and try again.
                    </div>
                </div>");
        }
    }
}