using Xunit;

namespace TodoApi.Tests
{
    public class TestToDoController
    {
        [Fact]
        public void PassingTest()
        {
            Assert.Equal(5,5);
        }

        [Fact]
        public void FailingTest()
        {
            Assert.NotEqual(4,5);
        }

        [Theory]
        [InlineData(3)]
        [InlineData(5)]
        [InlineData(6)] // <- this is not odd it will fail
        public void TheoryTest(int value)
        {
            // only true for particualr set of data
            bool isOdd = value % 2 == 1;
            Assert.True(isOdd);
        }
    }
}
// in order to create this:
// dotnet add package Microsoft.NET.Test.Sdk --version 16.9.4
// dotnet add package xunit --version 2.4.1
// dotnet add package xunit.runner.visualstudio --version 2.4.3

// this will not run, since this is a second entry point for the preogram and program will get confused
// we have to change TodoApi.csproj
//   <PropertyGroup>
//     <TargetFramework>net5.0</TargetFramework>
//     <GenerateProgramFile>false</GenerateProgramFile> <======this line
//   </PropertyGroup>

// to run: dotnet test

// https://www.youtube.com/watch?v=HQmbAdjuB88