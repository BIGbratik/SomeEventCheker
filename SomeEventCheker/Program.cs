using System.Threading;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<SomeEvent> events = new List<SomeEvent>(); //Статическое хранилище для присланных событий
var value_sum = 0; //Переменная для сбора статистики присланных значений value за минуту

Console.WriteLine(DateTime.Now + " - Сервер запущен"); //Оповещение в консоль о дате и времени активации сервера

MyLogger(); //Запуск метода по ежеминутной отправке в консоль данных о сумме присланных значений VALUE

//Получение данных о некотором событии из присланного запроса
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    if (request.HasJsonContentType())
    {
        var message = "Некорректные данные";
        try
        {
            var some_event = await request.ReadFromJsonAsync<SomeEvent>();
            if (some_event != null)
            {
                events.Add(some_event);
                value_sum += some_event.Value;
                message = "Данные успешно сохранены";
            }
        }
        catch { }

        await response.WriteAsJsonAsync(new { text = message });
    }
    else
    {
        await response.WriteAsync("Привет, отправь мне событие");
    }
});


app.Run();

void MyLogger() //Метод запуска цикличного таймера для ведения в консоли статистики присланных занчений VALUE
{
    System.Timers.Timer timer = new System.Timers.Timer(60000);

    timer.Elapsed += CheckValuesum;
    timer.AutoReset = true;
    timer.Enabled = true;
}

//Метод отправки сообщения на консоль
void CheckValuesum(object source, ElapsedEventArgs e)
{
    Console.WriteLine(DateTime.Now + $" - {value_sum}");
    value_sum = 0;
}

//Класс события, принимаемого из запроса
public class SomeEvent
{
    public string Name { get; set; } = ""; //Поле, для хранения JSON "name"
    public int Value { get; set; } //Поле, для хранения JSON "value"
}