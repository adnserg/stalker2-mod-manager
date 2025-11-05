using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace Stalker2ModManager.Helpers
{
    public class InsertionLineAdorner : Adorner
    {
        private double _insertionY;

        public InsertionLineAdorner(UIElement adornedElement)
            : base(adornedElement)
        {
            IsHitTestVisible = false; // Adorner не должен перехватывать события мыши
            _insertionY = 0;
        }

        public void UpdatePosition(double y)
        {
            _insertionY = y;
            InvalidateVisual();
        }

        protected override Size MeasureOverride(Size constraint)
        {
            return AdornedElement.RenderSize;
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            return finalSize;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (AdornedElement is not System.Windows.Controls.ListBox listBox) return;

            var width = listBox.ActualWidth;

            // Рисуем горизонтальную линию вставки
            var lineY = Math.Max(0, Math.Min(_insertionY, listBox.ActualHeight));

            // Толстая синяя линия
            var pen = new Pen(new SolidColorBrush(Color.FromArgb(255, 0, 122, 204)), 3);
            drawingContext.DrawLine(pen, new Point(0, lineY), new Point(width, lineY));

            // Добавляем небольшой треугольник-индикатор слева
            var triangleSize = 8.0;
            var pathGeometry = new PathGeometry();
            var figure = new PathFigure
            {
                StartPoint = new Point(0, lineY - triangleSize)
            };
            figure.Segments.Add(new LineSegment(new Point(triangleSize, lineY), true));
            figure.Segments.Add(new LineSegment(new Point(0, lineY + triangleSize), true));
            figure.IsClosed = true;
            pathGeometry.Figures.Add(figure);

            drawingContext.DrawGeometry(
                new SolidColorBrush(Color.FromArgb(255, 0, 122, 204)),
                null,
                pathGeometry);
        }
    }
}
