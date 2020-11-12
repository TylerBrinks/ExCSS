//namespace ExCSS
//{
//    public class RenderDevice
//    {
//        public enum Kind : byte
//        {
//            Screen,
//            Printer,
//            Speech,
//            Other
//        }

//        public RenderDevice(int width, int height)
//        {
//            DeviceWidth = width;
//            DeviceHeight = height;
//            ViewPortWidth = width;
//            ViewPortHeight = height;
//            ColorBits = 32;
//            MonochromeBits = 0;
//            Resolution = 96;
//            DeviceType = Kind.Screen;
//            IsInterlaced = false;
//            IsGrid = false;
//            Frequency = 60;
//        }

//        public int ViewPortWidth { get; set; }
//        public int ViewPortHeight { get; set; }
//        public bool IsInterlaced { get; set; }
//        public bool IsGrid { get; set; }
//        public int DeviceWidth { get; private set; }
//        public int DeviceHeight { get; private set; }
//        public int Resolution { get; set; }
//        public int Frequency { get; set; }
//        public int ColorBits { get; set; }
//        public int MonochromeBits { get; set; }
//        public Kind DeviceType { get; set; }
//    }
//}