using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
//using System.Threading;
//using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChamHuntApp.Engine.FigureBase
{
    public class Animation
    {
        public double t;
        public double T;
        public Func<double, double, double> dX;
        public Func<double, double, double> dY;
        public Func<double, double, double> dAlpha;

        public Animation(double start, double finish, Func<double, double, double> dXLambda, Func<double, double, double> dYLambda, Func<double, double, double> dAlphaLambda)
        {
            t = start;
            T = finish;
            dX = dXLambda;
            dY = dYLambda;
            dAlpha = dAlphaLambda;
        }
    }


    public class Ancestor
    {
        public Figure AncestorFigure;
        public RotateTransform AncestorTransform;

        public Ancestor(Figure _fig, RotateTransform _transf)
        {
            AncestorFigure = _fig;
            AncestorTransform = _transf;
        }
    }

    public class Figure
    {
        public double Angle = 0;

        public Point RotatePoint = new Point(0, 0);
        public Point AbsCoords = new Point(0, 0);
        public Vector InitialOffset = new Vector(0, 0);
        public Vector Offset = new Vector(0, 0);

        private Size WindowSize = new Size(0, 0);

        public Canvas FigureCanvas;
        public TransformGroup FigureTransformGroup;
        private RotateTransform FigureRotateTransform;
        private TranslateTransform FigureTranslateTransform;
        public List<Ancestor> Ancestors;
        public List<Figure> Children;
        public List<Animation> Animations; 

        public Ellipse FigureRotatePoint;
        private double RotatePointRadius = 3;
        public Border CanvasBorder;


        private bool _ShowRects = false;
        public bool ShowRects
        {
            get
            {
                return _ShowRects;
            }
            set
            {
                _ShowRects = value;
                if (_ShowRects)
                {
                    CanvasBorder.Visibility = Visibility.Visible;
                    FigureRotatePoint.Visibility = Visibility.Visible;
                }
                else 
                {
                    CanvasBorder.Visibility = Visibility.Hidden;
                    FigureRotatePoint.Visibility = Visibility.Hidden;
                }
                foreach (Figure f in Children)
                {
                    f.ShowRects = value;
                }
            }
        }

        private bool _Visible = true;
        public bool Visible
        {
            get
            {
                return _Visible;
            }
            set
            {
                _Visible = value;
                if (_Visible)
                {
                    FigureCanvas.Visibility = Visibility.Visible;
                }
                else
                {
                    FigureCanvas.Visibility = Visibility.Hidden;
                }
            }
        }


        public Figure()
        {
            Children = new List<Figure>();
            Ancestors = new List<Ancestor>();
            Animations = new List<Animation>();
        }

        public Figure(string _imgUri, Grid _canvasGrid)
            : this()
        {
            FigureCanvas = new Canvas();
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri(_imgUri, UriKind.Relative));
            FigureCanvas.Width = ib.ImageSource.Width;
            FigureCanvas.Height = ib.ImageSource.Height;
            FigureCanvas.HorizontalAlignment = HorizontalAlignment.Left;
            FigureCanvas.VerticalAlignment = VerticalAlignment.Top;
            FigureCanvas.Background = ib;
        
            _canvasGrid.Children.Add(FigureCanvas);

            FigureTransformGroup = new TransformGroup();
            FigureRotateTransform = new RotateTransform(0);
            FigureTranslateTransform = new TranslateTransform();
            FigureTransformGroup.Children.Add(FigureRotateTransform);
            FigureTransformGroup.Children.Add(FigureTranslateTransform);
            FigureCanvas.RenderTransform = FigureTransformGroup;

            CanvasBorder = new Border();
            CanvasBorder.BorderThickness = new Thickness(2);
            CanvasBorder.BorderBrush = Brushes.Red;
            CanvasBorder.Width = ib.ImageSource.Width;
            CanvasBorder.Height = ib.ImageSource.Height;
            FigureCanvas.Children.Add(CanvasBorder);

            FigureRotatePoint = new Ellipse() { Height = RotatePointRadius * 2, Width = RotatePointRadius * 2 };
            FigureRotatePoint.StrokeThickness = 3;
            FigureRotatePoint.Stroke = Brushes.Red;
            FigureCanvas.Children.Add(FigureRotatePoint);

            ShowRects = false;
        }

        public Figure(string _imgUri, Grid _canvasGrid, Size _size)
            : this(_imgUri, _canvasGrid)
        {
            FigureCanvas.Width = _size.Width;
            FigureCanvas.Height = _size.Height;

            CanvasBorder.Width = _size.Width;
            CanvasBorder.Height = _size.Height;
        }

        public Figure(string _imgUri, Grid _canvasGrid, double _scale)
            : this(_imgUri, _canvasGrid)
        {
            FigureCanvas.Width *= _scale;
            FigureCanvas.Height *= _scale;

            CanvasBorder.Width *= _scale;
            CanvasBorder.Height *= _scale;
        }

        public void AddChild(Figure _child)
        {
            Children.Add(_child);
            foreach (Ancestor Anc in Ancestors)
            {
                _child.Ancestors.Add(new Ancestor(Anc.AncestorFigure, new RotateTransform(0)));
            }
            _child.Ancestors.Add(new Ancestor(this, new RotateTransform(0)));
            //_child.HasParent = true;
        }

        public void SetWindowSize(Size _size)
        {
            WindowSize = _size;
        }

        public void SetOffset(Vector _vector)
        {
            InitialOffset = _vector;
            Offset = _vector;
        }

        public void SetRotatePoint(Point _point)
        {
            RotatePoint = _point;
        }

        public virtual void Move(Vector _vector)
        {
            Offset = Offset + _vector;
            Draw();
        }

        public void ResetOffset()
        {
            Offset = InitialOffset;
        }


        public virtual void Turn(double _angle)
        {
            SetAngle(Angle + _angle);
            Draw();
        }

        public virtual void SetAngle(double _angle)
        {
            Angle = _angle;
            Draw();
        }

        public void Rotate(double _angle, double _x, double _y)
        {
            FigureCanvas.RenderTransform = new RotateTransform(_angle, _x, _y);
        }

        public void Draw()
        {
            //remove old transforms
            FigureTransformGroup.Children.Remove(FigureTranslateTransform);
            FigureTransformGroup.Children.Remove(FigureRotateTransform);
            foreach (Ancestor Anc in Ancestors)
            {
                FigureTransformGroup.Children.Remove(Anc.AncestorTransform);
            }

            // count all ancestors translate transforms
            AbsCoords.X = Offset.X - FigureCanvas.Width / 2 + WindowSize.Width / 2;
            AbsCoords.Y = Offset.Y - FigureCanvas.Height / 2 + WindowSize.Height / 2;
            foreach (Ancestor Anc in Ancestors)
            {
                Figure FigureParent = Anc.AncestorFigure;
                AbsCoords = Point.Add(AbsCoords, FigureParent.Offset);
            }

            // assign translate transform
            FigureTranslateTransform = new TranslateTransform(AbsCoords.X, AbsCoords.Y);

            // count and assign ancestors rotate transforms
            for (int i = Ancestors.Count - 1; i >= 0; i--)
            {
                Figure FigureParent = Ancestors[i].AncestorFigure;
                Point ParentRotatePoint = new Point(
                    -AbsCoords.X + FigureParent.AbsCoords.X + FigureParent.RotatePoint.X,
                    -AbsCoords.Y + FigureParent.AbsCoords.Y + FigureParent.RotatePoint.Y);
                Ancestors[i].AncestorTransform = new RotateTransform(FigureParent.Angle, ParentRotatePoint.X, ParentRotatePoint.Y);
                FigureTransformGroup.Children.Add(Ancestors[i].AncestorTransform);
            }

            //find the rotate point to apply self transform
            Point RelativeRotatePoint = RotatePoint;
            for (int i = Ancestors.Count - 1; i >= 0; i--)
            {
                RelativeRotatePoint = Ancestors[i].AncestorTransform.Transform(RelativeRotatePoint);
            }
            RelativeRotatePoint = FigureTranslateTransform.Transform(RelativeRotatePoint);

            //assign self transform
            FigureRotateTransform = new RotateTransform(Angle, RelativeRotatePoint.X, RelativeRotatePoint.Y);

            //draw the rotate point
            Canvas.SetLeft(FigureRotatePoint, RotatePoint.X - RotatePointRadius);
            Canvas.SetTop(FigureRotatePoint, RotatePoint.Y - RotatePointRadius);

            //apply the transforms
            FigureTransformGroup.Children.Add(FigureTranslateTransform);
            FigureTransformGroup.Children.Add(FigureRotateTransform);
            FigureCanvas.RenderTransform = FigureTransformGroup;

            //redraw the children
            foreach (Figure f in Children)
            {
                f.Draw();
            }

        }


    }

}
