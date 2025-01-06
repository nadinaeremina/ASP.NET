namespace ChemicalElementsDictionary
{
    // описывает сущность химического элемента таблицы Менделеева
    public class Element
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // наименование 
        public string Code { get; set; } = string.Empty; // код 
        public int Group { get; set; } // группа 
        public int Period { get; set; } // период 
        public int ProtonsNumber { get; set; } // кол-во протонов в ядре эл-та
        public Element() {}
    }
}
