namespace HonsMusicMaker.Entities
{
    public class CustomMelody
    {
        public CustomNote[] Melody { get; set; }

        public CustomMelody(CustomNote[] melody)
        {
            this.Melody = melody;
        }

        public CustomMelody()
        {
            
        }
    }
}