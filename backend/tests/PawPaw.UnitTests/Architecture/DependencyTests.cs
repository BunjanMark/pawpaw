using FluentAssertions;
using NetArchTest.Rules;
using PawPaw.Core.Common;
using PawPaw.UseCases.Files;
using Xunit;

namespace PawPaw.UnitTests.Architecture;

public sealed class DependencyTests
{
  [Fact]
  public void Core_Should_Not_Depend_On_Outer_Layers()
  {
    var result = Types
      .InAssembly(typeof(EntityBase).Assembly)
      .ShouldNot()
      .HaveDependencyOnAny("PawPaw.UseCases", "PawPaw.Infrastructure", "PawPaw.Web")
      .GetResult();

    result.IsSuccessful.Should().BeTrue();
  }

  [Fact]
  public void UseCases_Should_Not_Depend_On_Infrastructure_Or_Web()
  {
    var result = Types
      .InAssembly(typeof(IFileStorage).Assembly)
      .ShouldNot()
      .HaveDependencyOnAny("PawPaw.Infrastructure", "PawPaw.Web")
      .GetResult();

    result.IsSuccessful.Should().BeTrue();
  }
}
