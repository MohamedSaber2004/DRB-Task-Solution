public class HistoryRecordDto
{
    public string StartLocation { get; set; } = null!;
    public string EndLocation { get; set; } = null!;
    public DateTime CompletedOn { get; set; }
    public double Distance { get; set; }
}