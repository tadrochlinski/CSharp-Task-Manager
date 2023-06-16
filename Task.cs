public class Task
{
    public int ID { get; set; }
    public string Name { get; set; }
    public string Desc { get; set; }
    public DateTime Date { get; set; }

    public Task()
    {
        Name = string.Empty;
        Desc = string.Empty;
    }
}