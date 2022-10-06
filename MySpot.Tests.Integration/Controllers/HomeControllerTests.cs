﻿using Shouldly;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MySpot.Tests.Integration.Controllers;

public class HomeControllerTests : ControllerTests
{
    [Fact]
    public async Task get_base_endpoint_should_return_200_ok_status_code_and_api_name()
    {
        var response = await Client.GetAsync("/");
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.ShouldBe("MySpot API [test]");
    }

    public HomeControllerTests(OptionsProvider optionsProvider) : base(optionsProvider)
    {
    }
}
