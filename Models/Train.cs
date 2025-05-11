namespace CFRApp.Models
{
    public class Train
    {
        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public string? Name { get; set; }  // optional
        public string Type { get; set; } = "R";  // R, IR, RE etc.
        public string Operator { get; set; } = "CFR Calatori";
    }
}
