using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Xamarin.Forms;

namespace TemplateUI.Controls
{
    public class TreeViewNodes : Element, IList<TreeViewNode>, INotifyCollectionChanged
	{
		readonly ObservableCollection<TreeViewNode> _treeViewNodes;

		public TreeViewNodes(IEnumerable<TreeViewNode> treeViewNodes)
		{
			_treeViewNodes = new ObservableCollection<TreeViewNode>(treeViewNodes) ?? throw new ArgumentNullException(nameof(treeViewNodes));
			_treeViewNodes.CollectionChanged += OnTreeViewNodesChanged;
		}

		public TreeViewNodes() : this(Enumerable.Empty<TreeViewNode>())
		{

		}

		public event NotifyCollectionChangedEventHandler CollectionChanged
		{
			add { _treeViewNodes.CollectionChanged += value; }
			remove { _treeViewNodes.CollectionChanged -= value; }
		}

		public TreeViewNode this[int index]
		{
			get => _treeViewNodes.Count > index ? _treeViewNodes[index] : null;
			set => _treeViewNodes[index] = value;
		}

		public int Count => _treeViewNodes.Count;

		public bool IsReadOnly => false;

		public void Add(TreeViewNode item)
		{
			_treeViewNodes.Add(item);
		}

		public void Clear()
		{
			_treeViewNodes.Clear();
		}

		public bool Contains(TreeViewNode item)
		{
			return _treeViewNodes.Contains(item);
		}

		public void CopyTo(TreeViewNode[] array, int arrayIndex)
		{
			_treeViewNodes.CopyTo(array, arrayIndex);
		}

		public IEnumerator<TreeViewNode> GetEnumerator()
		{
			return _treeViewNodes.GetEnumerator();
		}

		public int IndexOf(TreeViewNode item)
		{
			return _treeViewNodes.IndexOf(item);
		}

		public void Insert(int index, TreeViewNode item)
		{
			_treeViewNodes.Insert(index, item);
		}

		public bool Remove(TreeViewNode item)
		{
			return _treeViewNodes.Remove(item);
		}

		public void RemoveAt(int index)
		{
			_treeViewNodes.RemoveAt(index);
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			object bc = BindingContext;

			foreach (BindableObject item in _treeViewNodes)
				SetInheritedBindingContext(item, bc);
		}

		void OnTreeViewNodesChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
		{
			if (notifyCollectionChangedEventArgs.NewItems == null)
				return;

			object bc = BindingContext;

			foreach (BindableObject item in notifyCollectionChangedEventArgs.NewItems)
				SetInheritedBindingContext(item, bc);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _treeViewNodes.GetEnumerator();
		}
	}
}