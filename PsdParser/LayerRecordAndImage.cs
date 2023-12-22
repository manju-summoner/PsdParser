namespace PsdParser
{
    public class LayerRecordAndImage(LayerRecord record, LayerImage image)
    {
        public LayerRecord Record { get; } = record;
        public LayerImage Image { get; } = image;

        public void Deconstruct(out LayerRecord record, out LayerImage image) => (record, image) = (Record, Image);
    }
}