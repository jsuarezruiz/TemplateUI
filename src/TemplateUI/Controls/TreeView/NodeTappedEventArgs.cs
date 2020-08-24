using System;

namespace TemplateUI.Controls
{
    public class NodeTappedEventArgs : EventArgs
    {
        public NodeTappedEventArgs(TreeViewNode treeViewNode)
        {
            Node = treeViewNode;
        }

        public TreeViewNode Node { get; set; }
    }
}