using System.Threading;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

System.Threading.Timer timer;

int interval = 60000; //�������� ������������ ����� ���������� � 1 ������

List<SomeEvent> events = new List<SomeEvent>(); //��������� ��� ���������� �������
List<Statistic> stat = new List<Statistic>(); //���������� ���������� ���������� value-��������

var value_sum = 0; //���������� ��� ����� ���������� ���������� �������� value �� ������

Console.WriteLine(DateTime.Now + " - ������ �������"); //���������� � ������� � ���� � ������� ��������� �������
CheckValue();

//Midleware ��� ��������� ������� �� ����������� ���������� �������
app.MapPost("/events", (SomeEvent someEvent)=>
{
    events.Add(someEvent);
    value_sum += someEvent.Value;
    var message = value_sum;
    return message;
});

//Midleware ��� �������� �� ������� ���������� ���������� value
app.MapGet("/check", () =>
{
    return stat;
});

app.Run();

void CheckValue() //����� ������� ���������� ������� ��� ������� � ������� ���������� ���������� �������� VALUE
{
    timer = new System.Threading.Timer(new TimerCallback(SendValuesum), null, 0, interval);
}

//����� ���������� ������� ���������� ����� ���������� �������� value
async void SendValuesum(object obj)
{
    stat.Add(new Statistic { Date = DateTime.Now, Sum = value_sum });
    value_sum = 0;
}

//����� �������, ������������ �� �������
public class SomeEvent
{
    public string Name { get; set; } = ""; //����, ��� �������� JSON "name"
    public int Value { get; set; } //����, ��� �������� JSON "value"
}

//����� �������� ����������� ���������� �������� �� ������ value-��������
public class Statistic
{
    public DateTime Date { get; set; }
    public int Sum { get; set; }   
}