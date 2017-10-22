using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChamHuntApp.Engine.FigureBase;
using System.Windows.Controls;
using System.Windows;

namespace ChamHuntApp.Engine.Chameleon
{
    public class TongueFigure : Figure
    {
        List<Figure> Parts;

        public TongueFigure()
        {
            Parts = new List<Figure>();
            Parts.Add(this);
            Visible = false;
        }

        public TongueFigure(string _imgUri, Grid _canvasGrid)
            : base(_imgUri, _canvasGrid)
        {
            Parts = new List<Figure>();
            Parts.Add(this);
            Visible = false;
        }

        public TongueFigure(string _imgUri, Grid _canvasGrid, Size _size)
            : base(_imgUri, _canvasGrid, _size)
        {
            Parts = new List<Figure>();
            Parts.Add(this);
            Visible = false;
        }

        public TongueFigure(string _imgUri, Grid _canvasGrid, double _scale)
            : base(_imgUri, _canvasGrid, _scale)
        {
            Parts = new List<Figure>();
            Parts.Add(this);
            Visible = false;
        }

        public void AddPart(Figure Part)
        {
            Parts[Parts.Count - 1].AddChild(Part);
            Parts.Add(Part);
            Part.Visible = false;
            Part.Draw();
        }

        public override void Move(Vector _vector)
        {            
            Figure head = Ancestors[Ancestors.Count - 1].AncestorFigure;
            if ((AbsCoords.X - 8 * FigureCanvas.ActualWidth / 10 + _vector.X) < head.AbsCoords.X)
            {
                _vector.Y -= _vector.X * 0.2;
                base.Move(_vector);
            }
            SetVisibility();
        }

        public int Shot(double l)
        {
            l = Math.Min(l, 300);
            double T = 400;
            double pause = 200;
            if (Animations.Count == 0)
            {
                double V0 = 4*l/T;
                double a = -8*l/(T*T);
                Animations.Add(new Animation(0, T/2, (t, dt) => (V0*dt + a*t*dt), (t, dt) => 0, (t, dt) => 0));
                Animations.Add(new Animation(-pause, T/2, (t, dt) => (V0 * dt + a * (t+T/2) * dt), (t, dt) => 0, (t, dt) => 0));
                //Animations.Add(new Animation(0, , (t, dt) => length * Math.Cos(Math.PI * t / T), (t, dt) => 0, (t, dt) => 0));
                //Animations.Add(new Animation(2*T, 3*T, (t, dt) => length * Math.Cos(Math.PI * t / T), (t, dt) => 0, (t, dt) => 0));
                return 1;
            }
            else
            {
                return 0;
            }
            //Move(new Vector(x - AbsCoords.X, y - AbsCoords.Y));
        }

        private void SetVisibility()
        {
            Figure head = Ancestors[Ancestors.Count - 1].AncestorFigure;
            foreach (Figure tonguePart in Parts)
            {
                if (tonguePart.AbsCoords.X - 6 * tonguePart.FigureCanvas.ActualWidth / 10 < head.AbsCoords.X)
                {
                    tonguePart.Visible = false;
                }
                else
                {
                    tonguePart.Visible = true;
                }
            }
        }
    
    }

    public class HeadFigure : Figure
    {
        public HeadFigure()
        {
        }

        public HeadFigure(string _imgUri, Grid _canvasGrid)
            : base(_imgUri, _canvasGrid)
        {
        }

        public HeadFigure(string _imgUri, Grid _canvasGrid, Size _size)
            : base(_imgUri, _canvasGrid, _size)
        {
        }

        public HeadFigure(string _imgUri, Grid _canvasGrid, double _scale)
            : base(_imgUri, _canvasGrid, _scale)
        {
        }


        public override void SetAngle(double _angle)
        {
            _angle = Math.Max(_angle, -88);
            _angle = Math.Min(_angle, 55);
            Angle = _angle;

            ResetOffset();
            if (Angle < -45)
            {
                Move(new Vector(0.3 * -45, -0.5 * -45));
                Move(new Vector(-0.8 * (_angle + 45), 0.15 * (_angle + 45)));
            }
            else if (Angle < 45)
            {
                Move(new Vector(0.3 * _angle, -0.5 * _angle));
            }
            else
            {
                Move(new Vector(0.3 * 45, -0.5 * 45));
                Move(new Vector(0.8 * (_angle - 45), -0.35 * (_angle - 45)));
            }
            Draw();
        }
    }


    public class TailFigure : Figure
    {
        public TailFigure()
        {
        }

        public TailFigure(string _imgUri, Grid _canvasGrid)
            : base(_imgUri, _canvasGrid)
        {
        }

        public TailFigure(string _imgUri, Grid _canvasGrid, Size _size)
            : base(_imgUri, _canvasGrid, _size)
        {
        }

        public TailFigure(string _imgUri, Grid _canvasGrid, double _scale)
            : base(_imgUri, _canvasGrid, _scale)
        {
        }

        public override void Turn(double _angle)
        {
            _angle = _angle / 2;
            if ((Angle + _angle > -10) && (Angle + _angle < 15))
            {
                SetAngle(Angle + _angle);
                Draw();
                foreach (Figure child in Children)
                {
                    ((TailFigure)child).Turn(_angle * 1.5);
                    //child.Move(new Vector(-Math.Sign(_angle) * _angle * 1.05, Math.Sign(_angle) * _angle * 1.05));
                }
            }
        }


    }

    
    public class Chameleon
    {
        public Figure Body;
        public HeadFigure Head;
        public TongueFigure Tongue;
        
        
        public Figure UpperChin;
        public Figure LowerChin;
        public Figure TongueBorder;
        public Figure Eye;
        public Figure LeftHand;
        public Figure RightHand;
        public Figure LeftLeg;
        public Figure RightLeg;
        public Figure Scate;
        public Figure FrontWheel;
        public Figure RearWheel;

        public TailFigure Tail;
        public TailFigure Tail02;
        public TailFigure Tail03;
        public TailFigure Tail04;
        public TailFigure Tail05;
        public TailFigure Tail06;
        public TailFigure Tail07;
        public TailFigure Tail08;
        public TailFigure Tail09;
        public TailFigure Tail10;
        public TailFigure Tail11;


        private bool _MouthOpened;
        private bool _TongueProtruded;

        public Chameleon(Grid grid, double windowWidth, double windowHeight)
        {

            LeftHand = new Figure(@"Resources/chameleon_left_hand.png", grid, new Size(109, 88));
            LeftHand.SetWindowSize(new Size(windowWidth, windowHeight));
            LeftHand.SetOffset(new Vector(400-296+109/2, 135-187+88/2));
            LeftHand.SetRotatePoint(new Point(15,10));

            Scate = new Figure(@"Resources/scate.png", grid, new Size(489 / 2, 175 / 2));
            Scate.SetWindowSize(new Size(windowWidth, windowHeight));
            Scate.SetOffset(new Vector(-35,50));
            Scate.SetRotatePoint(new Point(0, 0));

            FrontWheel = new Figure(@"Resources/scate_wheel_front.png", grid, new Size(51 / 2, 50/ 2));
            FrontWheel.SetWindowSize(new Size(windowWidth, windowHeight));
            FrontWheel.SetOffset(new Vector(80,10));
            FrontWheel.SetRotatePoint(new Point(FrontWheel.FigureCanvas.Width / 2, FrontWheel.FigureCanvas.Height / 2));

            RearWheel = new Figure(@"Resources/scate_wheel_rear.png", grid, new Size(36 / 2, 37 / 2));
            RearWheel.SetWindowSize(new Size(windowWidth, windowHeight));
            RearWheel.SetOffset(new Vector(-90,10));
            RearWheel.SetRotatePoint(new Point(RearWheel.FigureCanvas.Width / 2, RearWheel.FigureCanvas.Height / 2));


            LeftLeg = new Figure(@"Resources/chameleon_left_leg.png", grid, new Size(134/2, 188/2));
            LeftLeg.SetWindowSize(new Size(windowWidth, windowHeight));
            LeftLeg.SetOffset(new Vector(682/2 - 296 + 134 / 4 - 20, 444/2 - 187 + 188 / 4));
            LeftLeg.SetRotatePoint(new Point(20, 7));


            Body = new Figure(@"Resources/chameleon_body.png", grid, new Size(592, 374));
            Body.SetWindowSize(new Size(windowWidth, windowHeight));
            Body.SetOffset(new Vector(-200, 100));
            Body.SetRotatePoint(new Point(Body.FigureCanvas.Width / 2, Body.FigureCanvas.Height / 2));


            Head = new HeadFigure(@"Resources/chameleon_head.png", grid, new Size(179, 124));
            Head.SetWindowSize(new Size(windowWidth, windowHeight));
            Head.SetOffset(new Vector(182, -125));
            Head.SetRotatePoint(new Point(0, Head.FigureCanvas.Height / 2));
            Head.Visible = false;



            //Tongue = new Figure(@"Resources/chameleon_tongue.png", grid, new Size(330, 93));
            //Tongue.SetWindowSize(new Size(windowWidth, windowHeight));
            //Tongue.SetOffset(new Vector(115, -3));
            //Tongue.SetRotatePoint(new Point(0, 80));
            //Tongue.Angle = -3;
            //Tongue.Visible = false;

 
            Tongue = new TongueFigure(@"Resources/Tongue/chameleon_tongue_1.png", grid, new Size(111/2, 186/2));
            Tongue.SetWindowSize(new Size(windowWidth, windowHeight));
            Tongue.SetOffset(new Vector(-350, 62));
            Tongue.SetRotatePoint(new Point(0, 80));
            Tongue.Angle = -3;
            //Tongue.Visible = false;
            Body.AddChild(Head);
            Head.AddChild(Tongue);

            Figure Tongue02 = new Figure(@"Resources/Tongue/chameleon_tongue_2.png", grid, new Size(111 / 2, 186 / 2));
            Tongue02.SetWindowSize(new Size(windowWidth, windowHeight));
            Tongue02.SetOffset(new Vector(110/2, 0));
            Tongue02.SetRotatePoint(new Point(0, 80));
            Tongue.AddPart(Tongue02);

            //Tongue.Angle = -3;
            //Tongue.Visible = false;

            Figure Tongue03 = new Figure(@"Resources/Tongue/chameleon_tongue_3.png", grid, new Size(111 / 2, 186 / 2));
            Tongue03.SetWindowSize(new Size(windowWidth, windowHeight));
            Tongue03.SetOffset(new Vector(110 / 2, 0));
            Tongue03.SetRotatePoint(new Point(0, 80));
            Tongue.AddPart(Tongue03);

            Figure Tongue04 = new Figure(@"Resources/Tongue/chameleon_tongue_4.png", grid, new Size(111 / 2, 186 / 2));
            Tongue04.SetWindowSize(new Size(windowWidth, windowHeight));
            Tongue04.SetOffset(new Vector(110 / 2, 0));
            Tongue04.SetRotatePoint(new Point(0, 80));
            Tongue.AddPart(Tongue04);

            Figure Tongue05 = new Figure(@"Resources/Tongue/chameleon_tongue_5.png", grid, new Size(111 / 2, 186 / 2));
            Tongue05.SetWindowSize(new Size(windowWidth, windowHeight));
            Tongue05.SetOffset(new Vector(110 / 2, 0));
            Tongue05.SetRotatePoint(new Point(0, 80));
            Tongue.AddPart(Tongue05);

            Figure Tongue06 = new Figure(@"Resources/Tongue/chameleon_tongue_6.png", grid, new Size(111 / 2, 186 / 2));
            Tongue06.SetWindowSize(new Size(windowWidth, windowHeight));
            Tongue06.SetOffset(new Vector(110 / 2, 0));
            Tongue06.SetRotatePoint(new Point(0, 80));
            Tongue.AddPart(Tongue06);


            UpperChin = new HeadFigure(@"Resources/chameleon_head_upperchin.png", grid, new Size(179, 124));
            UpperChin.SetWindowSize(new Size(windowWidth, windowHeight));
            UpperChin.SetOffset(new Vector(0, 0));
            UpperChin.SetRotatePoint(new Point(0, UpperChin.FigureCanvas.Height / 2));

            Eye = new Figure(@"Resources/chameleon_head_eye.png", grid, new Size(51/2, 48/2));
            Eye.SetWindowSize(new Size(windowWidth, windowHeight));
            Eye.SetOffset(new Vector(-8, -28));
            Eye.SetRotatePoint(new Point(51/4, 48/4));


            LowerChin = new Figure(@"Resources/chameleon_head_lowerchin.png", grid, new Size(179, 124));
            LowerChin.SetWindowSize(new Size(windowWidth, windowHeight));
            LowerChin.SetOffset(new Vector(0, 0));
            LowerChin.SetRotatePoint(new Point(34, 85));

            RightHand = new Figure(@"Resources/chameleon_right_hand.png", grid, new Size(275/2, 173/2));
            RightHand.SetWindowSize(new Size(windowWidth, windowHeight));
            RightHand.SetOffset(new Vector(786/2 - 296 + 275 / 4, 200/2 - 187 + 173 / 4));
            RightHand.SetRotatePoint(new Point(28/2, 66/2));

            RightLeg = new Figure(@"Resources/chameleon_right_leg.png", grid, new Size(184 / 2, 275 / 2));
            RightLeg.SetWindowSize(new Size(windowWidth, windowHeight));
            RightLeg.SetOffset(new Vector(681 / 2 - 296 + 184 / 4 - 100, 444 / 2 - 187 + 275 / 4));
            RightLeg.SetRotatePoint(new Point(59, 15));


            Tail = new TailFigure(@"Resources/Tail/chameleon_tail_1.png", grid, new Size(135 / 2, 130 / 2));
            Tail.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail.SetOffset(new Vector(-131, 82));
            Tail.SetRotatePoint(new Point(55, 25));

            Tail02 = new TailFigure(@"Resources/Tail/chameleon_tail_2.png", grid, new Size(109 / 2, 106 / 2));
            Tail02.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail02.SetOffset(new Vector(-54, 11));
            Tail02.SetRotatePoint(new Point(40, 25));

            Tail03 = new TailFigure(@"Resources/Tail/chameleon_tail_3.png", grid, new Size(97 / 2, 91 / 2));
            Tail03.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail03.SetOffset(new Vector(-44, -7));
            Tail03.SetRotatePoint(new Point(40, 25));

            Tail04 = new TailFigure(@"Resources/Tail/chameleon_tail_4.png", grid, new Size(92 / 2, 90 / 2));
            Tail04.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail04.SetOffset(new Vector(-34, -24));
            Tail04.SetRotatePoint(new Point(40, 35));

            Tail05 = new TailFigure(@"Resources/Tail/chameleon_tail_5.png", grid, new Size(88 / 2, 93 / 2));
            Tail05.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail05.SetOffset(new Vector(-13, -38));
            Tail05.SetRotatePoint(new Point(25, 35));

            Tail06 = new TailFigure(@"Resources/Tail/chameleon_tail_6.png", grid, new Size(77 / 2, 90 / 2));
            Tail06.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail06.SetOffset(new Vector(-5, -38));
            Tail06.SetRotatePoint(new Point(77/4, 37));

            Tail07 = new TailFigure(@"Resources/Tail/chameleon_tail_7.png", grid, new Size(86 / 2, 94 / 2));
            Tail07.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail07.SetOffset(new Vector(5, -30));
            Tail07.SetRotatePoint(new Point(22, 37));

            Tail08 = new TailFigure(@"Resources/Tail/chameleon_tail_8.png", grid, new Size(61 / 2, 55 / 2));
            Tail08.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail08.SetOffset(new Vector(22, -20));
            Tail08.SetRotatePoint(new Point(10, 20));

            Tail09 = new TailFigure(@"Resources/Tail/chameleon_tail_9.png", grid, new Size(57 / 2, 58 / 2));
            Tail09.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail09.SetOffset(new Vector(22, -11));
            Tail09.SetRotatePoint(new Point(6, 18));

            Tail10 = new TailFigure(@"Resources/Tail/chameleon_tail_10.png", grid, new Size(46 / 2, 49 / 2));
            Tail10.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail10.SetOffset(new Vector(21, -6));
            Tail10.SetRotatePoint(new Point(3, 14));

            Tail11 = new TailFigure(@"Resources/Tail/chameleon_tail_11.png", grid, new Size(36 / 2, 43 / 2));
            Tail11.SetWindowSize(new Size(windowWidth, windowHeight));
            Tail11.SetOffset(new Vector(17, -3));
            Tail11.SetRotatePoint(new Point(2, 12));


            TongueBorder = new Figure(@"Resources/scate_wheel_rear.png", grid, new Size(36 / 2, 37 / 2));
            TongueBorder.SetWindowSize(new Size(windowWidth, windowHeight));
            TongueBorder.SetOffset(new Vector(-50, 28));
            TongueBorder.SetRotatePoint(new Point(0, 0));
            TongueBorder.Visible = false;
            //Tongue.AddChild(Tongue02);
            //Tongue02.AddChild(Tongue03);
            //Tongue03.AddChild(Tongue04);
            //Tongue04.AddChild(Tongue05);
            //Tongue05.AddChild(Tongue06);

            Head.AddChild(TongueBorder);
            Head.AddChild(UpperChin);
            Head.AddChild(LowerChin);
            Head.AddChild(Eye);
            Body.AddChild(LeftHand);
            Body.AddChild(RightHand);
            Body.AddChild(LeftLeg);
            Body.AddChild(RightLeg);
            LeftLeg.AddChild(Scate);
            Scate.AddChild(FrontWheel);
            Scate.AddChild(RearWheel);

            Body.AddChild(Tail);
            Tail.AddChild(Tail02);
            Tail02.AddChild(Tail03);
            Tail03.AddChild(Tail04);
            Tail04.AddChild(Tail05);
            Tail05.AddChild(Tail06);
            Tail06.AddChild(Tail07);
            Tail07.AddChild(Tail08);
            Tail08.AddChild(Tail09);
            Tail09.AddChild(Tail10);
            Tail10.AddChild(Tail11);

            //Tongue.Move(new Vector(0, 0));
            Body.Draw();

        }

        public bool MouthOpened
        {
            get
            {
                return _MouthOpened;                   
            }
            set
            {
                _MouthOpened = value;
                if (_MouthOpened)
                {
                    LowerChin.Angle += 18;
                }
                else
                {
                    LowerChin.Angle -= 18;                
                }
                LowerChin.Draw();
            }
        }

        public bool TonguePrortuded
        {
            get
            {
                return _TongueProtruded;
            }
            set
            {
                _TongueProtruded = value;
                MouthOpened = _TongueProtruded;
                Tongue.Visible = _TongueProtruded;
                //Tongue02.Visible = _TongueProtruded;
                //Tongue03.Visible = _TongueProtruded;
                //Tongue04.Visible = _TongueProtruded;
                //Tongue05.Visible = _TongueProtruded;
                //Tongue06.Visible = _TongueProtruded;
                Head.Draw();
            }
        }

    }

    
}
