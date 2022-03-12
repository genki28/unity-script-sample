public class ConstellationPositionData : CsvData
{
    public int Id { get; set; } // êØç¿ID
    public float RightAscension { get; set; } // ê‘åo
    public float Declination { get; set; } // ê‘à‹

    public override void SetData(string[] data)
    {
        Id = int.Parse(data[0]);
        RightAscension = RightAscensionToDegree(int.Parse(data[1]), int.Parse(data[2]));
        Declination = DeclinationToDegree(int.Parse(data[3]));
    }
}