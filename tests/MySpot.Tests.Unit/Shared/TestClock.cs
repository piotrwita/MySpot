using MySpot.Core.Abstractions;
using System;

namespace MySpot.Tests.Unit.Shared;

internal class TestClock : IClock
{
    public DateTime Current() => new(2022, 08, 11, 12, 0, 0);
}
