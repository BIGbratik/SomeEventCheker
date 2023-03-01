using System.Threading;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

System.Threading.Timer timer;

int interval = 1000;

bool isCheck = false;

List<SomeEvent> events = new List<SomeEvent>(); //Статическое хранилище для присланных событий

var value_sum = 0; //Переменная для сбора статистики присланных значений value за минуту

Console.WriteLine(DateTime.Now + " - Сервер запущен"); //Оповещение в консоль о дате и времени активации сервера

//Midleware для обработки запроса на регистрацию некоторого события
app.MapPost("/events", (SomeEvent someEvent)=>
{
    events.Add(someEvent);
    if (isCheck)
        value_sum += someEvent.Value;
    var message = value_sum;
    return message;
});

app.MapGet("/checkvalue", () =>
{
    isCheck = true;
    CheckValue();
    return "Счётчик событий запущен";
});

app.Run();

void CheckValue() //Метод запуска цикличного таймера для ведения в консоли статистики присланных занчений VALUE
{
    /*System.Timers.Timer timer = new System.Timers.Timer(60000);

    timer.Elapsed += SendValuesum;
    timer.AutoReset = true;
    timer.Enabled = true;*/
    timer = new System.Threading.Timer(new TimerCallback(SendValuesum), null, 1000, interval);
}

//Метод отправки сообщения на консоль
async void SendValuesum(object obj)
{
    /*HttpContext context = (HttpContext)obj;
    Console.WriteLine(context.User);
    await context.Response.WriteAsync(DateTime.Now + $" - {value_sum}");*/
    Console.WriteLine(DateTime.Now + $" - {value_sum}");
    value_sum = 0;
}

//Класс события, принимаемого из запроса
public class SomeEvent
{
    public string Name { get; set; } = ""; //Поле, для хранения JSON "name"
    public int Value { get; set; } //Поле, для хранения JSON "value"
}