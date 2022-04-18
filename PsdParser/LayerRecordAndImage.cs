namespace PsdParser
{
    public class LayerRecordAndImage
    {
        public LayerRecord Record { get; }
        public LayerImage Image { get; }
        public LayerRecordAndImage(LayerRecord record, LayerImage image)
        {
            Record = record;
            Image = image;
        }

        public void Deconstruct(out LayerRecord record, out LayerImage image) => (record, image) = (Record, Image);
    }
}