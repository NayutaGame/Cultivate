using System;
using System.Collections.Generic;

public class TreeNode<T>
{
    // 数据字段
    public T Data { get; set; }
    
    // 树结构字段
    public TreeNode<T> Parent { get; set; }
    public List<TreeNode<T>> Children { get; private set; }

    public int Depth { get; set; }
    
    // 构造函数
    public TreeNode(T data = default(T))
    {
        Data = data;
        Children = new List<TreeNode<T>>();
        Depth = 0;
    }
    
    // ========== 增 ==========
    // 添加子节点
    public void AddChild(T item)
    {
        TreeNode<T> child = new TreeNode<T>(item);
        child.Parent = this;
        child.Depth = this.Depth + 1;
        Children.Add(child);
    }
    
    public void AddChild(TreeNode<T> child)
    {
        if (child == null) return;
        
        // 如果子节点已有父节点，先从原父节点移除
        if (child.Parent != null)
        {
            child.Parent.RemoveChild(child);
        }
        
        child.Parent = this;
        child.Depth = this.Depth + 1;
        UpdateChildrenDepth(child);
        Children.Add(child);
    }
    
    // 在指定位置插入子节点
    public void InsertChild(int index, T item)
    {
        if (index < 0 || index > Children.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        TreeNode<T> child = new TreeNode<T>(item);
        child.Parent = this;
        child.Depth = this.Depth + 1;
        Children.Insert(index, child);
    }
    
    public void InsertChild(int index, TreeNode<T> child)
    {
        if (index < 0 || index > Children.Count)
            throw new ArgumentOutOfRangeException(nameof(index));
        
        if (child == null) return;
        
        // 如果子节点已有父节点，先从原父节点移除
        if (child.Parent != null)
        {
            child.Parent.RemoveChild(child);
        }
        
        child.Parent = this;
        child.Depth = this.Depth + 1;
        UpdateChildrenDepth(child);
        Children.Insert(index, child);
    }
    
    // ========== 删 ==========
    // 移除指定子节点
    public bool RemoveChild(TreeNode<T> child)
    {
        if (child == null) return false;
        
        bool removed = Children.Remove(child);
        if (removed)
        {
            child.Parent = null;
        }
        return removed;
    }
    
    public bool RemoveChild(int index)
    {
        if (index < 0 || index >= Children.Count)
            return false;
        
        TreeNode<T> child = Children[index];
        Children.RemoveAt(index);
        child.Parent = null;
        return true;
    }
    
    public bool RemoveChild(T item)
    {
        TreeNode<T> child = FindChild(item);
        if (child != null)
        {
            return RemoveChild(child);
        }
        return false;
    }
    
    // 清空所有子节点
    public void ClearChildren()
    {
        foreach (var child in Children)
        {
            child.Parent = null;
        }
        Children.Clear();
    }
    
    // ========== 改 ==========
    // 设置节点数据
    public void SetData(T item)
    {
        Data = item;
    }
    
    // ========== 查 ==========
    // 查找子节点
    public TreeNode<T> FindChild(T item)
    {
        foreach (var child in Children)
        {
            if (EqualityComparer<T>.Default.Equals(child.Data, item))
            {
                return child;
            }
        }
        return null;
    }
    
    // 获取子节点
    public TreeNode<T> GetChild(int index)
    {
        if (index < 0 || index >= Children.Count)
            return null;
        return Children[index];
    }
    
    public List<TreeNode<T>> GetChildren()
    {
        return new List<TreeNode<T>>(Children);
    }
    
    // 获取父节点
    public TreeNode<T> GetParent()
    {
        return Parent;
    }
    
    // 获取根节点
    public TreeNode<T> GetRoot()
    {
        TreeNode<T> current = this;
        while (current.Parent != null)
        {
            current = current.Parent;
        }
        return current;
    }
    
    // 获取从根到当前节点的路径
    public List<TreeNode<T>> GetPath()
    {
        List<TreeNode<T>> path = new List<TreeNode<T>>();
        TreeNode<T> current = this;
        
        while (current != null)
        {
            path.Insert(0, current);
            current = current.Parent;
        }
        
        return path;
    }
    
    // 获取节点深度
    public int GetDepth()
    {
        return Depth;
    }
    
    // 获取子树高度
    public int GetHeight()
    {
        if (IsLeaf())
        {
            return 0;
        }
        
        int maxHeight = 0;
        foreach (var child in Children)
        {
            int childHeight = child.GetHeight();
            if (childHeight > maxHeight)
            {
                maxHeight = childHeight;
            }
        }
        
        return maxHeight + 1;
    }
    
    // 判断节点状态
    public bool IsRoot()
    {
        return Parent == null;
    }
    
    public bool IsLeaf()
    {
        return Children.Count == 0;
    }
    
    public bool HasChildren()
    {
        return Children.Count > 0;
    }
    
    // 获取子节点数量
    public int GetChildCount()
    {
        return Children.Count;
    }

    public IEnumerable<TreeNode<T>> TraversePreOrder()
    {
        yield return this;
        foreach (TreeNode<T> child in Children)
        foreach (TreeNode<T> node in child.TraversePreOrder())
            yield return node;
    }
    
    public IEnumerable<TreeNode<T>> TraversePostOrder()
    {
        foreach (TreeNode<T> child in Children)
        foreach (TreeNode<T> node in child.TraversePostOrder())
            yield return node;
        yield return this;
    }
    
    public IEnumerable<TreeNode<T>> TraverseLevelOrder()
    {
        Queue<TreeNode<T>> queue = new Queue<TreeNode<T>>();
        queue.Enqueue(this);
        
        while (queue.Count > 0)
        {
            TreeNode<T> current = queue.Dequeue();
            yield return current;
            
            foreach (var child in current.Children)
            {
                queue.Enqueue(child);
            }
        }
    }
    
    // 辅助方法：更新子节点深度
    private void UpdateChildrenDepth(TreeNode<T> node)
    {
        foreach (var child in node.Children)
        {
            child.Depth = node.Depth + 1;
            UpdateChildrenDepth(child);
        }
    }
}
