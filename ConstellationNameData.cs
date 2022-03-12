public class ConstellationNameData : CsvData
{
    public int Id { get; set; } // ¯ÀID
    public string Summary { get; set; } // —ªÌ
    public string Name { get; set; } // ‰pŒê–¼
    public string JapaneseName { get; set; } // “ú–{Œê–¼

    public override void SetData(string[] data)
    {
        Id = int.Parse(data[0]);
        Summary = data[1];
        Name = data[2];
        JapaneseName = data[3];
    }
}