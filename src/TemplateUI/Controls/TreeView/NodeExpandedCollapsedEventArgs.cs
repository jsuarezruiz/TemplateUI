using System;

namespace TemplateUI.Controls
{
    public class NodeExpandedCollapsedEventArgs  : EventArgs
    {
        public NodeExpandedCollapsedEventArgs(TreeViewNode treeViewNode)
        {
            Node = treeViewNode;
        }

        public TreeViewNode Node { get; set; }
    }
}