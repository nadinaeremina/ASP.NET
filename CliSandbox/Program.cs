using System.IO;

namespace CliSandbox
{
    internal class Program
    {
        // это просто класс адреса - будет выступать в качестве сервиса
        class Adress
        {
            private string city = string.Empty;
            private string street = string.Empty;
            private int buildingNumber;
            public Adress(string city, string street, int buildingNumber)
            {
                this.city = city;
                this.street = street;       
                this.buildingNumber= buildingNumber;
            }
            public override string ToString()
            {
                return $"{city} - {street} - {buildingNumber}";
            }
        }
        // класс организации
        class Company
        {
            private string name = string.Empty;
            private Adress adress;
            public Company(string name, Adress adress)
            {
                this.name = name;
                this.adress = adress;
            }
            public override string ToString()
            {
                return $"{name} - {adress}";
            }
        }
        class AddressFactory()
        {
            // здесь создаем адрес
            public Adress CreateAddress()
            {
                return new Adress("Барнаул", "Пушкина", 1);
            }
        }
        // создадим фабрику
        class CompanyFactory
        {
            private AddressFactory addressFactory;
            public CompanyFactory(AddressFactory addressFactory)
            {
                this.addressFactory = addressFactory;
            }
            // здесь создаем компанию
            public Company CreateCompany()
            {
                // создаем всегда одну и ту же компанию, но ее адрес может меняться
                return new Company("ООО Рога и копыта", addressFactory.CreateAddress());
            }
        }
        // создание специального компонента для управления (не создания) зависимостями приложения
        // статический класс
        class DependencyContainer
        {
            // словарь хранит внутри себя ключ ввиде типа зависимости, а в качестве значения - саму зависимость
            public static Dictionary<Type, object> deps = new Dictionary<Type, object>();
            public static void AddDependency<T>(T dependency)
            {
                // устанавливаем тип у этой зависимости и добавляем ее в список зависимостей
                deps[dependency.GetType()] = dependency;
            }
            // достаем зависимость по типу
            public static T GetDependency<T>()
            {
                return (T)deps[typeof(T)];
            }
        }
        static void RunApp()
        {
            // здесь ничего не создается, все было создано до "Run"
            // мы дали приложению все необходимые зависимости внутри контейнера, а оно дальше с этим работате
            // адрес достаем из контейнера
            Adress adress = DependencyContainer.GetDependency<Adress>();
            Console.WriteLine(adress);

            // компанию достаем из контейнера
            Company company = DependencyContainer.GetDependency<Company>();
            Console.WriteLine(company);
        }
        static void ConfigureApp()
        {
            // создаем конфиги // конфигурируем
            AddressFactory addressFactory = new AddressFactory();
            Adress adress = addressFactory.CreateAddress();      
            CompanyFactory companyFactory = new CompanyFactory(addressFactory);
            Company company = companyFactory.CreateCompany();
            // добавляем сервисы в наш контейнер
            DependencyContainer.AddDependency(company);
            DependencyContainer.AddDependency(adress);
        }
        static void Main(string[] args)
        {
            //////////////////////////////////////////////// 1 ////////////////////////////////////////////////////////
            //Adress adress = new("Барнаул", "Пушкина", 1);
            //Company company = new Company("ООО Рога и копыта", adress);
            //Console.WriteLine(company);

            // если нельзя создавать компоненты в других компонентах - то, что будет, если мы создадим компонент
            // для создания других компонентов?
            // можно ли в нем будет создавать? - да, можно - порождающие паттерны (фабрика, строитель)

            ///////////////////////////////////////////////// 2 //////////////// с использованием паттерна //////////////
            //AddressFactory addressFactory = new AddressFactory();
            //CompanyFactory companyFactory = new CompanyFactory(addressFactory);
            //Company company = companyFactory.CreateCompany();
            //Console.WriteLine(company);

            // если в адресе чтото меняется - меняем только в этом компоненте ('AddressFactory' и 'Adress')

            ////////////////////////////////////////////// 3 /////////// запуск приложения в 2 этапа: ///////////////////
            // 1 // конфигурация контейнера зависимостей
            ConfigureApp();
            // 2 // запуск приложения
            RunApp();
        }

        // СЕРВИС - это класс, решающий какую-либо задачу, как правило, не являющийся статическим
        // ЗАВИСИМОСТЬ - класс, который используется другим классом или кодом в качестве сервиса
        // и требуется используещему классу для работы

        ///////////////////// Правила работы с сервисами и зависимостями: ///////////////////////////////////////

        // 1 // компоненты не должны нести ответственность за создание компонентов зависимостей,
        // все зависимости должны передаваться через параметры методов или конструкторов
        // ни один компонент не должен создаваться внутри другого компонента
        // один компонент не должен влиять на создание другого компонента
        // все зависимости должны передаваться через параметры (параметры методов или конструткоров)
        // нарушение правила ведет к сильной связанности компонентов приложения

        // 2 // для создания сложных компонентов использовать паттерн фабрики или строителя (порождающие паттерны)
        // а именно создавать и использовать компоненты для создания сложных компонентов

        // Строгое соблюдение пункта 2 крупного приложения ведет к излишнему усложнению проекта и сильной
        // привязке к логике фабрик - решение - создать специальный компонент для управления зависимостями приложения
        // нужен глобальный инструмент, который будет отвечать за зависимости и выдавать эти зависимости по требованию
        // управлять жизненным циклом зависимостей (как создаются сервисы друг из друга и тд)
        // некоторый контейнер, внктри которого будут наши сервисы, зависимости
        // любой компонент нашего приложения сможет обратиться к этому контейнеру для получения какого-либо сервиса
        // 'required service'
        // запуск приложения будет состоять из двух этапов - инициализация контейнера и запуск приложения
        // нужно создать специальный компонент для управления (не создания) зависимостями приложения 'DependencyContainer'

        // 3 // для крупного приложения, которыми являются веб-приложения, следует использовать 
        // 'dependency injection' (DI) - "внедрение зависимостей" и 'IoC-контейнер' (Inversion of control)
        // 'DI' - компоненты и сервисы не создаются в приложении, а внедряются в это приложение через
        // различные инструменты, вчастности через различные контейнеры
        // 'IoC-контейнер' (механизм) - управление зависимостями, делает инверсию управления, один из компонентов
        // управляет нашими компонентами, управляет создаениями компонентов, уровнями доступа компонентов
        // областями видимости и временем жизнями компонентов
        // 'DI' - возможно только при соблюдении первого правила: все зависимости должны передаваться через параметры
        // DI-концепция (механизм) предлагает реализовывать и использовать IoC-контейнер с зависимостями для того,
        // чтобы работать с сервисами в больших приложениях
        // эти два механизма реализованы в ASP
    }
}
