using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game2048
{
    public class PointOnBoard
    {
        public PointOnBoard(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public PointOnBoard()
        {
        }

        public int Row { get; set; }

        public int Column { get; set; }

        public static bool operator ==(PointOnBoard point1, PointOnBoard point2)
        {
            return point1.Equals(point2);
        }

        public static bool operator !=(PointOnBoard point1, PointOnBoard point2)
        {
            return !point1.Equals(point2);
        }

        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            if (!(obj is PointOnBoard))
            {
                return false;
            }

            var objAsPoint = (PointOnBoard)obj;
            return (objAsPoint.Row == this.Row) && (objAsPoint.Column == this.Column);
        }
    }
}

