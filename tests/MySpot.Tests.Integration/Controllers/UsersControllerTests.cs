using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.Repositiories;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;
using Shouldly;
using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace MySpot.Tests.Integration.Controllers;

public class UsersControllerTests : ControllerTests, IDisposable
{
    [Fact]
    public async Task post_users_should_return_created_201_status_code()
    {
        var command = new SignUp(Guid.Empty, "test-user1@myspot.io", "test-user1", "secret", "John Doe", Role.User());

        var response = await Client.PostAsJsonAsync("users", command);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);
        response.Headers.Location.ShouldNotBeNull();
    }

    [Fact]
    public async Task post_sign_in_should_return_ok_200_status_code_and_jwt()
    {
        var user = await CreateUserAsync();
        var command = new SignIn(user.Email, Password);
        var response = await Client.PostAsJsonAsync("users/sign-in", command);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var jwt = await response.Content.ReadFromJsonAsync<JwtDto>();
        jwt.ShouldNotBeNull();
        jwt.AccessToken.ShouldNotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task get_users_me_should_return_ok_200_status_code_and_user()
    {
        var user = await CreateUserAsync();
        Authorize(user.Id, user.Role);

        var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");
        userDto.ShouldNotBeNull();
        userDto.Id.ShouldBe(user.Id.Value);
    }

    private async Task<User> CreateUserAsync()
    {
        var clock = new Clock();
        var passwordManager = new PasswordManager(new PasswordHasher<User>());

        var user = new User(Guid.NewGuid(), "test-user1@myspot.io", "test-user1", passwordManager.Secure(Password), "John Doe", Role.User(), clock.Current());

        await _userRepository.AddAsync(user);

        return user;
    }

    private const string Password = "secret";
    private readonly TestDatabase _testDatabase;
    private IUserRepository _userRepository;

    public UsersControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
    {
        _testDatabase = new TestDatabase(); 
    }

    public void Dispose()
    {
        _testDatabase?.Dispose();
    }

    protected override void ConfigureServices(IServiceCollection services)
    {
        _userRepository = new TestUserRepository();
        services.AddSingleton<IUserRepository>(_userRepository);
    }
}