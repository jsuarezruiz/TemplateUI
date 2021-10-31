using System;

namespace TemplateUI.Controls
{
	public class ScrolledEventArgs : EventArgs
	{
		public double Offset { get; set; }
		public int FirstVisibleItemIndex { get; set; }
		public int CenterItemIndex { get; set; }
		public int LastVisibleItemIndex { get; set; }
	}
}