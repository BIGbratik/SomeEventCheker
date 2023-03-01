using System.Threading;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

System.Threading.Timer timer;

int interval = 60000; //ИНтервал срабатывания сбора статистики в 1 минуту

List<SomeEvent> events = new List<SomeEvent>(); //Хранилище для присланных событий
List<Statistic> stat = new List<Statistic>(); //Хранеилище статистики присланных value-значений

var value_sum = 0; //Переменная для сбора статистики присланных значений value за минуту

Console.WriteLine(DateTime.Now + " - Сервер запущен"); //Оповещение в консоль о дате и времени активации сервера
CheckValue();

//Midleware для обработки запроса на регистрацию некоторого события
app.MapPost("/events", (SomeEvent someEvent)=>
{
    events.Add(someEvent);
    value_sum += someEvent.Value;
    var message = value_sum;
    return message;
});

//Midleware для отправки по запросу статистики присланных value
app.MapGet("/check", () =>
{
    return stat;
});

app.Run();

void CheckValue() //Метод запуска цикличного таймера для ведения в консоли статистики присланных занчений VALUE
{
    timer = new System.Threading.Timer(new TimerCallback(SendValuesum), null, 0, interval);
}

//Метод сохранения текущей статистики суммы присланных значений value
async void SendValuesum(object obj)
{
    stat.Add(new Statistic { Date = DateTime.Now, Sum = value_sum });
    value_sum = 0;
}

//Класс события, принимаемого из запроса
public class SomeEvent
{
    public string Name { get; set; } = ""; //Поле, для хранения JSON "name"
    public int Value { get; set; } //Поле, для хранения JSON "value"
}

//Класс хранения ежеминутной статистики отправки на сервер value-значений
public class Statistic
{
    public DateTime Date { get; set; }
    public int Sum { get; set; }   
}