using System.Threading;
using System.Timers;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<SomeEvent> events = new List<SomeEvent>(); //����������� ��������� ��� ���������� �������
var value_sum = 0; //���������� ��� ����� ���������� ���������� �������� value �� ������

Console.WriteLine(DateTime.Now + " - ������ �������"); //���������� � ������� � ���� � ������� ��������� �������

MyLogger(); //������ ������ �� ����������� �������� � ������� ������ � ����� ���������� �������� VALUE

//��������� ������ � ��������� ������� �� ����������� �������
app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    if (request.HasJsonContentType())
    {
        var message = "������������ ������";
        try
        {
            var some_event = await request.ReadFromJsonAsync<SomeEvent>();
            if (some_event != null)
            {
                events.Add(some_event);
                value_sum += some_event.Value;
                message = "������ ������� ���������";
            }
        }
        catch { }

        await response.WriteAsJsonAsync(new { text = message });
    }
    else
    {
        await response.WriteAsync("������, ������� ��� �������");
    }
});


app.Run();

void MyLogger() //����� ������� ���������� ������� ��� ������� � ������� ���������� ���������� �������� VALUE
{
    System.Timers.Timer timer = new System.Timers.Timer(60000);

    timer.Elapsed += CheckValuesum;
    timer.AutoReset = true;
    timer.Enabled = true;
}

//����� �������� ��������� �� �������
void CheckValuesum(object source, ElapsedEventArgs e)
{
    Console.WriteLine(DateTime.Now + $" - {value_sum}");
    value_sum = 0;
}

//����� �������, ������������ �� �������
public class SomeEvent
{
    public string Name { get; set; } = ""; //����, ��� �������� JSON "name"
    public int Value { get; set; } //����, ��� �������� JSON "value"
}