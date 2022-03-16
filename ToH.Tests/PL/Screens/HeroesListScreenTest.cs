using System;
using System.Collections.Generic;
using Moq;
using ToH.Data;
using ToH.PL;
using ToH.PL.Screens;
using Xunit;

namespace ToH.Tests.Screens;


public class HeroesListScreenTest
{
    private HeroesListScreen uut;
    private Mock<IDatabase> _db;
    private Mock<IPrinter> _printer;
    private Mock<IUi> _ui;

    public HeroesListScreenTest()
    {
        _db = new Mock<IDatabase>();
        _printer = new Mock<IPrinter>(MockBehavior.Strict);
        _ui = new Mock<IUi>();
        uut = new HeroesListScreen(_db.Object, _printer.Object);
    }

    [Fact]
    public void ShouldPrintTowLines_WithNoActionAndOneHero()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
        });
        _printer.Setup(p => p.Clear());
        _printer.Setup(p => p.PrintLine(It.IsAny<string>()));

        // Act
        uut.None(_ui.Object);

        // Assert
        _printer.Verify(printer => printer.PrintLine(
                It.IsAny<string>()),
            Times.Exactly(2));
    }

    [Fact]
    public void ShouldAddCursorOnFirstHero_WhenNoActionIsGiven()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));

        // Act
        uut.None(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Once);
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Once);
    }
    
    [Fact]
    public void ShouldKeepCursorOnFirstHero_WhenDownActionIsGivenOnLastHero()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));

        // Act
        uut.Down(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Once);
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Once);
    }
    
    [Fact]
    public void ShouldKeepCursorOnLastHero_WhenDownActionIsGivenOnFirstHero()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));

        // Act
        uut.Up(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Once);
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Once);
    }
    
    [Fact]
    public void ShouldAddCursorOnFirstHero_WhenNoActionIsGivenAndTwoHeroes()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
            new () { Id = 2, Name = "TestHero2"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));

        // Act
        uut.None(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Exactly(2));
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Once);
    }

    [Fact]
    public void ShouldShowHeroNameInUppercase()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
        });
        _printer.Setup(p => p.Clear());
        _printer.Setup(p => p.PrintLine(It.IsAny<string>()));

        // Act
        uut.None(_ui.Object);
        
        // Assert
        _printer.Verify(printer => printer.PrintLine(
                It.Is<string>(s => s.Contains("TESTHERO1"))), 
            Times.Exactly(1));   
    }

    [Fact]
    public void ShouldAddCursorSecondFirstHero_WhenDowActionIsGivenWithTwoHeroes()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
            new () { Id = 2, Name = "TestHero2"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));

        // Act
        uut.Down(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Exactly(2));
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Once);
    }
    
    [Fact]
    public void ShouldKeepCursorSecondFirstHero_WhenDowActionIsGivenTwiceWithTwoHeroes()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
            new () { Id = 2, Name = "TestHero2"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));

        // Act
        uut.Down(_ui.Object);
        uut.Down(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Exactly(4));
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Exactly(2));
    }
    
    [Fact]
    public void ShouldMoveCursorToSecondAndBackToFirstHero_WhenDowAndUpActionsIsGivenWithTwoHeroes()
    {
        // Arrange
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            new () { Id = 1, Name = "TestHero1"},
            new () { Id = 2, Name = "TestHero2"},
        });
        var seq = new MockSequence();
        _printer.Setup(p => p.Clear());
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))));
        _printer.InSequence(seq).Setup(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))));

        // Act
        uut.Down(_ui.Object);
        uut.Up(_ui.Object);

        // Assert
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => !s.Contains("*"))), Times.Exactly(4));
        _printer.Verify(p => p.PrintLine(It.Is<string>(s => s.Contains("*"))), Times.Exactly(2));
    }

    [Fact]
    public void ShouldSetScreenOnUiToHeroScreen_WhenEnterActionIsGiven()
    {
        // Arrange
        var hero = new Hero() { Id = 1, Name = "TestHero1" };
        var heroScreen = new HeroScreen(hero, new Mock<IPrinter>().Object);
        _db.Setup(db => db.GetAllHeroes()).Returns(new List<Hero>()
        {
            hero,
        });
        var screenFactory = new Mock<IScreenFactory>();
        screenFactory
            .Setup(sf => sf.CreateScreen(
                    It.Is<Type>(t => t == typeof(HeroScreen)), 
                    It.IsAny<Hero>()))
            .Returns(heroScreen);
        _ui.Setup(ui => ui.ScreenFactory).Returns(screenFactory.Object);
        
        // Act
        uut.Enter(_ui.Object);
        
        // Assert
        _ui.VerifySet(ui => ui.Screen=It.Is<HeroScreen>(hs => hs == heroScreen));
    }

}