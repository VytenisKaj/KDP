using FluentAssertions;
using Moq;
using System.Text.RegularExpressions;

namespace KursinisTests;

public class CsvServiceTests
{
    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithSelect()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,20", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,20\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"20\"},{\"name\":\"Alice\",\"age\":\"20\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithSelectAndStar()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,20", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,20\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT * FROM users.csv");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"20\"},{\"name\":\"Alice\",\"age\":\"20\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithSelectAndNotAllColumns()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,20", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,20\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"},{\"name\":\"Alice\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithFrom()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines("users.csv"))
            .Returns(new[] { "name,age", "John,20", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile("users.csv"))
            .Returns("name,age\nJohn,20\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"20\"},{\"name\":\"Alice\",\"age\":\"20\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithLessThan()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,19", "Alice,30" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,19\nAlice,30");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE age < 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"19\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithLessOrEqualThan()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,20", "Alice,19", "Tom,30" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,20\nAlice,19\nTom,30");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE age <= 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"20\"},{\"name\":\"Alice\",\"age\":\"19\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithGreaterThan()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,50", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,50\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE age > 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"50\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithGreaterOrEqualThan()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,50", "Alice,20", "Tom,19" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,50\nAlice,20\nTom,19");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE age >= 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"50\"},{\"name\":\"Alice\",\"age\":\"20\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithEqual()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,20", "Alice,21", "Tom,19" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,20\nAlice,21\nTom,19");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE age = 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"20\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithNotEqual()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE age != 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\",\"age\":\"21\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithLikeNoWildcard()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv WHERE name LIKE 'John'");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithLikeWithWildcardAtStart()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv WHERE name LIKE '%ohn'");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithLikeWithWildcardAtEnd()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv WHERE name LIKE 'Joh%'");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithLikeWithWildcardInMiddle()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv WHERE name LIKE 'J%n'");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithAnd()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE name LIKE 'J%n' AND age > 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithOr()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,21", "Alice,20", "Tom,35" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,21\nAlice,20\nTom,35");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name, age FROM users.csv WHERE name LIKE 'J%n' OR age < 21");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"},{\"name\":\"Alice\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithAs()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "John,25", "Alice,30" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nJohn,25\nAlice,30");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name AS Name FROM users.csv WHERE age > 20");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"Name\":\"John\"},{\"Name\":\"Alice\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithOrderByAsc()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "Tom, 35", "John,25", "Alice,30" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nTom,35\nJohn,25\nAlice,30");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv ORDER BY age ASC");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"John\"},{\"name\":\"Alice\"},{\"name\":\"Tom\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryWithOrderByDesc()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name,age", "Tom, 35", "John,25", "Alice,30" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name,age\nTom,35\nJohn,25\nAlice,30");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name FROM users.csv ORDER BY age DESC");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"Tom\"},{\"name\":\"Alice\"},{\"name\":\"John\"}]");
    }

    [Fact]
    public void Query_ShouldReturnCorrectJson_WhenValidQueryAndFileWithDifferentSeparator()
    {
        // Arrange
        var fileServiceMock = new Mock<IFileService>();
        fileServiceMock.Setup(x => x.ReadFileLines(It.IsAny<string>()))
            .Returns(new[] { "name;age", "Tom; 35", "John;25", "Alice;30" });
        fileServiceMock.Setup(x => x.ReadFile(It.IsAny<string>()))
            .Returns("name;age\nTom;35\nJohn;25\nAlice;30");

        var csvService = new CSVService(fileServiceMock.Object);

        // Act
        var result = csvService.Query("SELECT name,age FROM users.csv");
        result = result.Replace(" ", "");
        result = Regex.Replace(result, @"\t|\n|\r", "");

        // Assert
        result.Should().Be("[{\"name\":\"Tom\"},{\"name\":\"John\"},{\"name\":\"Alice\"}]");
    }
}
