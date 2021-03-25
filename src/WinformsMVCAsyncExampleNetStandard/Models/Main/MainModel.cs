using System;
using System.Collections.Generic;
using System.Text;

namespace WinformsMVCAsyncExampleNetStandard.Models.Main
{
    public class MainModel
    {
        #region Singleton
        static MainModel instance;
        public static MainModel Instance
        {
            get
            {
                if ( instance == null )
                    instance = new MainModel();

                return instance;
            }
        }
        #endregion

        private List<Point> points;
        public Point[] Points
        {
            set
            {
                points = new List<Point>( value );
            }
            get
            {
                return points.ToArray();
            }
        }

        public void AddPoint( int y )
        {
            points.Add( new Point( points.Count, y ) );
        }

        public MainModel()
        {
            points = new List<Point>();
        }

        public void Initialize( int Points, int MaxValue )
        {
            for ( int p = 0; p < Points; p++ )
                AddPoint( (int)( MaxValue / 2 + Math.Sin( 2 * Math.PI * p / Points ) * MaxValue / 2 ) );
        }
    }

    public class Point
    {
        private int x;
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        private int y;
        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        private Point() { }
        public Point( int x, int y )
        {
            this.x = x;
            this.y = y;
        }
    }

    class Consts
    {
        public const int POINTS = 100;
        public const int MAXH = 200;
    }
}
