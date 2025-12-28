
using System.Collections.Generic;
using UnityEngine.Assertions;

public class TreeModel<T> : ITreeModel
{
    public TreeNode<T> Root { get; private set; }
    
    // ITreeModel 实现
    object ITreeModel.GetRoot()
    {
        return Root.Data;
    }
    
    IEnumerable<object> ITreeModel.TraversePreOrder()
    {
        if (Root == null) yield break;
        foreach (var node in Root.TraversePreOrder())
        {
            yield return node.Data;
        }
    }
    
    IEnumerable<object> ITreeModel.TraversePostOrder()
    {
        if (Root == null) yield break;
        foreach (var node in Root.TraversePostOrder())
        {
            yield return node.Data;
        }
    }
    
    IEnumerable<object> ITreeModel.TraverseLevelOrder()
    {
        if (Root == null) yield break;
        foreach (var node in Root.TraverseLevelOrder())
        {
            yield return node.Data;
        }
    }
    
    public void Clear()
    {
        if (Root != null)
        {
            Root.ClearChildren();
            Root = null;
        }
    }

    // indexer
    public T this[TreeIndex index]
    {
        get
        {
            TreeNode<T> node = GetNodeByIndex(index);
            return node.Data;
        }
    }
    
    // 根据 TreeIndex 获取节点
    private TreeNode<T> GetNodeByIndex(TreeIndex index)
    {
        TreeNode<T> current = Root;
        foreach (int idx in index.IndexList)
        {
            Assert.IsTrue(current != null && idx >= 0 && idx < current.Children.Count);
            current = current.Children[idx];
        }
        
        return current;
    }
    
    public void SetRoot(TreeNode<T> newRoot)
    {
        // 如果新根节点有父节点，先断开连接
        if (newRoot.Parent != null)
        {
            newRoot.Parent.RemoveChild(newRoot);
        }
        
        Root = newRoot;
        Root.Parent = null;
        Root.Depth = 0;
        UpdateAllDepths(Root);
    }
    
    public void SetRoot(T item)
    {
        Root = new TreeNode<T>(item);
        Root.Depth = 0;
    }
    
    public TreeNode<T> FindNode(T item)
    {
        if (Root == null) return null;
        
        return Root.FindChild(item);
    }
    
    public TreeNode<T> GetRoot()
    {
        return Root;
    }

    public int GetHeight()
    {
        if (Root == null) return -1;
        return Root.GetHeight();
    }
    
    public bool ContainsNode(TreeNode<T> node)
    {
        if (node == null || Root == null) return false;
        
        // 检查节点是否在树中（通过向上遍历到根节点）
        TreeNode<T> current = node;
        while (current.Parent != null)
        {
            current = current.Parent;
        }
        
        return current == Root;
    }
    
    public bool ContainsNode(T item)
    {
        return FindNode(item) != null;
    }
    
    public IEnumerable<TreeNode<T>> TraversePreOrder()
    {
        if (Root == null) yield break;
        foreach (var node in Root.TraversePreOrder())
        {
            yield return node;
        }
    }
    
    public IEnumerable<TreeNode<T>> TraversePostOrder()
    {
        if (Root == null) yield break;
        foreach (var node in Root.TraversePostOrder())
        {
            yield return node;
        }
    }
    
    public IEnumerable<TreeNode<T>> TraverseLevelOrder()
    {
        if (Root == null) yield break;
        foreach (var node in Root.TraverseLevelOrder())
        {
            yield return node;
        }
    }
    
    // 辅助方法：更新所有节点的深度
    private void UpdateAllDepths(TreeNode<T> node)
    {
        if (node == null) return;
        
        foreach (var child in node.Children)
        {
            child.Depth = node.Depth + 1;
            UpdateAllDepths(child);
        }
    }
}
