using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
//using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using ChamHuntApp.Engine.FigureBase;
using ChamHuntApp.Engine.FigureBase;
using ChamHuntApp.Engine.Chameleon;
using System.Timers;
using System.Windows.Threading;

namespace ChamHuntApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

     public partial class MainWindow : Window
    {
        private Chameleon Cham;
        private double headAngleStep = 1;

        private List<Figure> FigureList;
        private List<Figure> AnimatedFigureList;
        private Timer AnimationTimer;
        private int SelectedFigureIndex = -1;
       

        //private Ellipse ChamHeadRotatePoint = new Ellipse() { Height = 5, Width = 5 };


        public MainWindow()
        {
            InitializeComponent();
            FigureList = new List<Figure>();
            AnimatedFigureList = new List<Figure>();
        }

        private void TheMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Cham = new Chameleon(MainGrid, ActualWidth, ActualHeight);

            FigureList.Add(Cham.Body);

            //FigureList.Add(Cham.Tail);
            //FigureList.Add(Cham.Tail02);
            //FigureList.Add(Cham.Tail03);
            //FigureList.Add(Cham.Tail04);
            //FigureList.Add(Cham.Tail05);
            //FigureList.Add(Cham.Tail06);
            //FigureList.Add(Cham.Tail07);
            //FigureList.Add(Cham.Tail08);
            //FigureList.Add(Cham.Tail09);
            //FigureList.Add(Cham.Tail10);
            //FigureList.Add(Cham.Tail11);


            FigureList.Add(Cham.Head);
            //FigureList.Add(ChamUpperChin);
            FigureList.Add(Cham.LowerChin);
            FigureList.Add(Cham.Tongue);

            FigureList.Add(Cham.LeftHand);
            FigureList.Add(Cham.RightHand);
            FigureList.Add(Cham.LeftLeg);
            FigureList.Add(Cham.RightLeg);
            FigureList.Add(Cham.Scate);
            FigureList.Add(Cham.FrontWheel);
            FigureList.Add(Cham.RearWheel);

            AnimatedFigureList.Add(Cham.Body);
            AnimatedFigureList.Add(Cham.Head);
            AnimatedFigureList.Add(Cham.Tongue);

            AnimationTimer = new Timer(20);
            //AnimationTimer.SynchronizingObject = this;
            AnimationTimer.Elapsed += new ElapsedEventHandler(Animate);
            AnimationTimer.Start();

        }

        private void Animate(object source, ElapsedEventArgs e)
        {
            this.Dispatcher.BeginInvoke(DispatcherPriority.Input, new Action(() => {
                foreach (Figure fig in AnimatedFigureList)
                {
                    Vector shift = new Vector(0, 0);
                    double angle = 0;
                    foreach (Animation animation in fig.Animations)
                    {
                        if (animation.t >= 0)
                        {
                            shift.X += animation.dX(animation.t, AnimationTimer.Interval);
                            shift.Y += animation.dY(animation.t, AnimationTimer.Interval);
                            angle += animation.dAlpha(animation.t, AnimationTimer.Interval);
                        }
                        animation.t += AnimationTimer.Interval;
                    }
                    for (int i = fig.Animations.Count - 1; i >= 0 ; i--)
                    {
                        if (fig.Animations[i].T < fig.Animations[i].t)
                        {
                            fig.Animations.RemoveAt(i);
                        }
                    }
                    fig.Move(shift);
                    fig.Turn(angle);
                }
            }));
        }


        private void TheMainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (SelectedFigureIndex >= 0)
            {
                if (Keyboard.IsKeyDown(Key.S)) FigureList[SelectedFigureIndex].Move(new Vector(0, 1));
                if (Keyboard.IsKeyDown(Key.W)) FigureList[SelectedFigureIndex].Move(new Vector(0, -1));
                if (Keyboard.IsKeyDown(Key.D)) FigureList[SelectedFigureIndex].Move(new Vector(1, 0));
                if (Keyboard.IsKeyDown(Key.A)) FigureList[SelectedFigureIndex].Move(new Vector(-1, 0));
                if (Keyboard.IsKeyDown(Key.E)) FigureList[SelectedFigureIndex].Turn(headAngleStep);
                if (Keyboard.IsKeyDown(Key.Q)) FigureList[SelectedFigureIndex].Turn(-headAngleStep);
            }

            if (Keyboard.IsKeyDown(Key.R)) Cham.MouthOpened = !Cham.MouthOpened;
            if (Keyboard.IsKeyDown(Key.T)) Cham.TonguePrortuded = !Cham.TonguePrortuded;

            //if (Keyboard.IsKeyDown(Key.P)) Cham.Head.Animations.Add(new Animation(0, 10000, (t,dt)=>0, (t,dt)=>0, (t,dt)=>0.01*dt));
            if (Keyboard.IsKeyDown(Key.L)) Cham.Body.Animations.Add(new Animation(0, 1000, (t, dt) => 0.1 * dt - 0.00005 * t * dt, (t, dt) => 0, (t, dt) => 0));
            if (Keyboard.IsKeyDown(Key.K)) Cham.Body.Animations.Add(new Animation(0, 1000, (t, dt) => -0.1 * dt + 0.00005 * t * dt, (t, dt) => 0, (t, dt) => 0));
            
            //if (Keyboard.IsKeyDown(Key.S)) ChamBody.Move(new Vector(0, 10));
            //if (Keyboard.IsKeyDown(Key.W)) ChamBody.Move(new Vector(0, -10));
            //if (Keyboard.IsKeyDown(Key.D)) ChamBody.Move(new Vector(10, 0));
            //if (Keyboard.IsKeyDown(Key.A)) ChamBody.Move(new Vector(-10, 0));
            //if (Keyboard.IsKeyDown(Key.E)) ChamBody.Turn(headAngleStep);
            //if (Keyboard.IsKeyDown(Key.Q)) ChamBody.Turn(-headAngleStep);
            
            //if (Keyboard.IsKeyDown(Key.Down))   ChamHead.Move(new Vector(0,10));
            //if (Keyboard.IsKeyDown(Key.Up))     ChamHead.Move(new Vector(0, -10));
            //if (Keyboard.IsKeyDown(Key.Right)) ChamHead.Move(new Vector(10, 0));
            //if (Keyboard.IsKeyDown(Key.Left)) ChamHead.Move(new Vector(-10,0));
            //if (Keyboard.IsKeyDown(Key.OemCloseBrackets)) ChamHead.Turn(headAngleStep);
            //if (Keyboard.IsKeyDown(Key.OemOpenBrackets)) ChamHead.Turn(-headAngleStep);

            //if (Keyboard.IsKeyDown(Key.K)) ChamLowerChin.Move(new Vector(0, 10));
            //if (Keyboard.IsKeyDown(Key.I)) ChamLowerChin.Move(new Vector(0, -10));
            //if (Keyboard.IsKeyDown(Key.L)) ChamLowerChin.Move(new Vector(10, 0));
            //if (Keyboard.IsKeyDown(Key.J)) ChamLowerChin.Move(new Vector(-10, 0));
            //if (Keyboard.IsKeyDown(Key.O)) ChamLowerChin.Turn(headAngleStep);
            //if (Keyboard.IsKeyDown(Key.U)) ChamLowerChin.Turn(-headAngleStep);

            if (Keyboard.IsKeyDown(Key.Tab))
            {
                if (SelectedFigureIndex >= 0) FigureList[SelectedFigureIndex].ShowRects = false;
                
                if (SelectedFigureIndex < FigureList.Count - 1)
                {
                    SelectedFigureIndex++;
                }
                else
                {
                    SelectedFigureIndex = 0;
                }
                FigureList[SelectedFigureIndex].ShowRects = true;
            }
            if (Keyboard.IsKeyDown(Key.Enter)) Cham.Body.ShowRects = !Cham.Body.ShowRects;
            if (Keyboard.IsKeyDown(Key.Escape)) TheMainWindow.Close();


            //ChamHead.Draw();
            //AngleLabel.Content = ChamHead.Angle.ToString();
        }



        private void TheMainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            Point pos = Mouse.GetPosition(this);
            double relX = Math.Abs(pos.X - Cham.Head.AbsCoords.X);
            double relY = pos.Y - Cham.Head.AbsCoords.Y;

            //if (relX > 0)
            //{
                //double previousAngle = Cham.Head.Angle;
                Cham.Head.SetAngle(Math.Atan(relY / relX) * 360 / 2 / Math.PI);
            //}

            double rel_eyeX = pos.X - Cham.Eye.AbsCoords.X;
            double rel_eyeY = pos.Y - Cham.Eye.AbsCoords.Y;
            if (rel_eyeX > 0)
                Cham.Eye.SetAngle(Math.Atan(rel_eyeY / rel_eyeX) * 360 / 2 / Math.PI);
            else
                Cham.Eye.SetAngle(Math.Atan(rel_eyeY / rel_eyeX) * 360 / 2 / Math.PI - 180);

        }

        private void TheMainWindow_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
                Point pos = Mouse.GetPosition(this);
                double relX = pos.X - Cham.TongueBorder.AbsCoords.X;
                double relY = pos.Y - Cham.TongueBorder.AbsCoords.Y;
                double len = Math.Sqrt(relX * relX + relY * relY);

                TestLabel.Content = len;
                Cham.Tongue.Shot(len);
        }


    }
}
