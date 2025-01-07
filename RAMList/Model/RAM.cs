namespace RAMList.Model
{
    // описывает сущность оперативной памяти
    public class RAM
    {
        public int Id { get; set; }
        public string Country { get; set; } = string.Empty; // страна-производитель
        public string Model { get; set; } = string.Empty; // модель
        public string MemoryType { get; set; } = string.Empty; // тип памяти 
        public string FormFactor { get; set; } = string.Empty; // форм-фактор памяти
        public int Volume { get; set; } // обьем памяти
        public double ClockFrequency { get; set; } // тактовая частота
        
        public RAM() { }
    }
}
