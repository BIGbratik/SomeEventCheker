using System.Threading;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

System.Threading.Timer timer;

int interval = 1000;

bool isCheck = false;

List<SomeEvent> events = new List<SomeEvent>(); //����������� ��������� ��� ���������� �������

var value_sum = 0; //���������� ��� ����� ���������� ���������� �������� value �� ������

Console.WriteLine(DateTime.Now + " - ������ �������"); //���������� � ������� � ���� � ������� ��������� �������

//Midleware ��� ��������� ������� �� ����������� ���������� �������
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
    return "������� ������� �������";
});

app.Run();

void CheckValue() //����� ������� ���������� ������� ��� ������� � ������� ���������� ���������� �������� VALUE
{
    /*System.Timers.Timer timer = new System.Timers.Timer(60000);

    timer.Elapsed += SendValuesum;
    timer.AutoReset = true;
    timer.Enabled = true;*/
    timer = new System.Threading.Timer(new TimerCallback(SendValuesum), null, 1000, interval);
}

//����� �������� ��������� �� �������
async void SendValuesum(object obj)
{
    /*HttpContext context = (HttpContext)obj;
    Console.WriteLine(context.User);
    await context.Response.WriteAsync(DateTime.Now + $" - {value_sum}");*/
    Console.WriteLine(DateTime.Now + $" - {value_sum}");
    value_sum = 0;
}

//����� �������, ������������ �� �������
public class SomeEvent
{
    public string Name { get; set; } = ""; //����, ��� �������� JSON "name"
    public int Value { get; set; } //����, ��� �������� JSON "value"
}