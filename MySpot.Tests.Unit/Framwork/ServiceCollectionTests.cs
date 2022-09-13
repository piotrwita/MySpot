using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using Xunit;

namespace MySpot.Tests.Unit.Framwork;

public class ServiceCollectionTests
{
    [Fact]
    public void given_two_objects_like_as_transient_should_be_different()
    {
        var serviceCollection = new ServiceCollection();

        //za kazdym razem gdy poprosimy nasz framework o przekazanie/wstrzykniecie nowego obiektu (jako abstrakcja/implementacja)
        //za kazdym razem tworzy nowy obiekt

        serviceCollection.AddTransient<IMessenger, Messenger>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        //GetRequiredService zwraca exception gdy nie znajdzie zarejestrowanego obiektu (getsevice zwraca null)
        var messenger = serviceProvider.GetRequiredService<IMessenger>();  

        var messenger2 = serviceProvider.GetRequiredService<IMessenger>(); 

        messenger.ShouldNotBeNull();
        messenger2.ShouldNotBeNull();

        messenger.ShouldNotBe(messenger2);
    }

    [Fact]
    public void given_two_objects_like_as_singleton_should_be_the_same()
    {
        var serviceCollection = new ServiceCollection();

        //przeciwienstwo trasient
        //istnieje jedna instancja obiektu reuzywalna globalnie i za kazdym razem ta sama wstrzykiwania
        //warto uzywac jesli nie mamy zmian stanu obiektu (np same obliczenia w klasie)
        serviceCollection.AddSingleton<IMessenger, Messenger>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var messenger = serviceProvider.GetRequiredService<IMessenger>();  

        var messenger2 = serviceProvider.GetRequiredService<IMessenger>(); 

        messenger.ShouldNotBeNull();
        messenger2.ShouldNotBeNull();

        messenger.ShouldBe(messenger2);
    }

    [Fact]
    public void given_two_objects_like_as_scoped_in_the_same_scope_should_be_the_same()
    {
        var serviceCollection = new ServiceCollection();

        //hybryda dwoch
        //pojedyncza instancja ale w ramach danego zakresu (scope'a) (jakby w obudowanej bańce)
        //np od cale przetworzenie od otrzymania zadania do zwrocenia odpowiedzi
        serviceCollection.AddScoped<IMessenger, Messenger>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        IMessenger messenger;
        IMessenger messenger2; 

        using (var scope = serviceProvider.CreateScope())
        {
            messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();

            messenger2 = scope.ServiceProvider.GetRequiredService<IMessenger>();
        } 

        messenger.ShouldNotBeNull();
        messenger2.ShouldNotBeNull(); 

        messenger.ShouldBe(messenger2); 
    }

    [Fact]
    public void given_two_objects_like_as_scoped_in_different_scopes_should_be_different()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<IMessenger, Messenger>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        IMessenger messenger;
        IMessenger messenger2;

        using (var scope = serviceProvider.CreateScope())
        {
            messenger = scope.ServiceProvider.GetRequiredService<IMessenger>();
        }

        using (var scope = serviceProvider.CreateScope())
        {
            messenger2 = scope.ServiceProvider.GetRequiredService<IMessenger>();
        }

        messenger.ShouldNotBeNull();
        messenger2.ShouldNotBeNull();

        messenger.ShouldNotBe(messenger2);
    }

    [Fact]
    public void given_two_objects_like_as_scoped_without_scope_should_be_the_same()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<IMessenger, Messenger>();

        var serviceProvider = serviceCollection.BuildServiceProvider();

        //nadany jest niejawny zakres
        IMessenger messenger = serviceProvider.GetRequiredService<IMessenger>();
        IMessenger messenger2 = serviceProvider.GetRequiredService<IMessenger>(); 

        messenger.ShouldNotBeNull();
        messenger2.ShouldNotBeNull();

        messenger.ShouldBe(messenger2);
    }

    private interface IMessenger
    {
        void Send();
    }

    private class Messenger : IMessenger
    {
        private readonly Guid _id = Guid.NewGuid();

        public void Send()
        {
            Console.WriteLine($"Sending a message... [{_id}]");
        }
    }
}
