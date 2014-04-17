using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Generic;

namespace Models
{
	public class GesturePoint
	{
		private double _x;
		private double _y;
		private GesturePoint _left;
		private GesturePoint _right;
		private GesturePoint _top;
		private GesturePoint _bottom;

		public GesturePoint Left
		{
			get { return _left; }
			set { _left = value; }
		}
		public GesturePoint Right
		{
			get { return _right; }
			set { _right = value; }
		}
		public GesturePoint Top
		{
			get { return _top; }
			set { _top = value; }
		}
		public GesturePoint Bottom
		{
			get { return _bottom; }
			set { _bottom = value; }
		}

		public GesturePoint()
		{
		}
		public GesturePoint(double x,double y)
		{
			_x = x;
			_y = y;
		}

		public override bool Equals(object obj)
		{
			try
			{
				GesturePoint objPoint = (GesturePoint)obj;
				if (this._x == objPoint._x && this._y == objPoint._y)
					return true;
				else
					return false;
			}
			catch
			{
				return base.Equals(obj);
			}
		}

	}

	public class GestureList<GesturePoint>
	{
		private int _capacity;

		List<GesturePoint> items;

		public GestureList(int capacity=10)
		{
			_capacity = capacity;
		}

		public void AddPoint(GesturePoint item)
		{
		}

		public bool Contains(GesturePoint item) //查找图中是否包含某项
		{
			foreach (GesturePoint v in items)
			{
				if (v.Equals(item))
				{
					return true;
				}
			}
			return false;
		}
	}
}
