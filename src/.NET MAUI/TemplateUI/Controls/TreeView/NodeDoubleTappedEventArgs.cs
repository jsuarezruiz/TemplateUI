using System;

namespace TemplateUI.Controls
{
    public class NodeDoubleTappedEventArgs : EventArgs
    {
        public NodeDoubleTappedEventArgs(TreeViewNode treeViewNode)
        {
            Node = treeViewNode;
        }

        public TreeViewNode Node { get; set; }
    }
}