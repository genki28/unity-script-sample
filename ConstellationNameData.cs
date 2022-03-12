public class ConstellationNameData : CsvData
{
    public int Id { get; set; } // ����ID
    public string Summary { get; set; } // ����
    public string Name { get; set; } // �p�ꖼ
    public string JapaneseName { get; set; } // ���{�ꖼ

    public override void SetData(string[] data)
    {
        Id = int.Parse(data[0]);
        Summary = data[1];
        Name = data[2];
        JapaneseName = data[3];
    }
}