using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PawaoxOCRWPF.GUI.GUIModels
{
    public interface BoundingBox
    {
        event BoundingBoxSizeChangedHandler SizeChangedEvent;
        event BoundingBoxPositionChangedHandler PositionChangedEvent;

        int GetTopPosition();
        int GetLeftPosition();
        int GetWidth();
        int GetHeight();
        bool GetIgnoresInput();
        double GetOpacity();

        void SetTopPosition(int value);
        void SetLeftPosition(int value);
        void SetWidth(int value);
        void SetHeight(int value);
        void SetIgnoresInput(bool ignoreInput);
        void SetOpacity(double opacity);

        void Show();
        void Hide();
    }

    public delegate void BoundingBoxPositionChangedHandler(BoundingBox sender, BoundingBoxPositionChanges changes);
    public delegate void BoundingBoxSizeChangedHandler(BoundingBox sender, BoundingBoxSizeChanges changes);

    public class BoundingBoxPositionChanges
    {
        public int OldTop { get; set; }
        public int NewTop { get; set; }
        public int OldLeft { get; set; }
        public int NewLeft { get; set; }

        public int TopChange { get; set; }
        public int LeftChange { get; set; }
    }
    public class BoundingBoxSizeChanges
    {
        public int OldWidth { get; set; }
        public int NewWidth { get; set; }
        public int OldHeight { get; set; }
        public int NewHeight { get; set; }

        public int WidthChange { get; set; }
        public int HeightChange { get; set; }
    }
}
