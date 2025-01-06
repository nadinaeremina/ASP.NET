namespace JsonAPI
{
    // OutputData - класс, описывающий выходные данные
    public class OutputData
    {
        public string Message { get; set; } = string.Empty;
        public int NumberCode { get; set; }
        public InputData? OutputForInput { get; set; }
        public OutputData(){}
    }
}
