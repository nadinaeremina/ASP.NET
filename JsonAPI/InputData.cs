namespace JsonAPI
{
    // InputData - класс, описывающий входные данные
    public class InputData
    {
        public int IntField { get; set; }
        public string StringField { get; set; } = string.Empty;
        public bool? NullableBoolField { get; set; }
        public InputData() { }
        public override string ToString()
        {
            string nbf = "null";
            if(NullableBoolField!=null)
            {
                nbf = NullableBoolField.Value ? "true" : "false";
            }
            return $"{IntField} - {StringField} - {NullableBoolField}";
        }
    }
}
