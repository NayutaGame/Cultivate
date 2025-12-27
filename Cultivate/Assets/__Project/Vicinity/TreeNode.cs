// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class TreeNode<T>
// {
//     // 数据字段
//     public T Data { get; set; }
//     
//     // 树结构字段
//     public TreeNode<T> Parent { get; set; }
//     public List<TreeNode<T>> Children { get; private set; }
//
//     public int Depth { get; set; }
//     
//     // ========== 增 ==========
//     // 添加子节点
//     public void AddChild(T item);
//     public void AddChild(TreeNode<T> child);
//     
//     // 在指定位置插入子节点
//     public void InsertChild(int index, T item);
//     public void InsertChild(int index, TreeNode<T> child);
//     
//     // ========== 删 ==========
//     // 移除指定子节点
//     public bool RemoveChild(TreeNode<T> child);
//     public bool RemoveChild(int index);
//     public bool RemoveChild(T item);
//     
//     // 清空所有子节点
//     public void ClearChildren();
//     
//     // ========== 改 ==========
//     // 设置节点数据
//     public void SetData(T item);
//     
//     // ========== 查 ==========
//     // 查找子节点
//     public TreeNode<T> FindChild(T item);
//     public List<TreeNode<T>> FindAllChildren(System.Predicate<TreeNode<T>> match);
//     
//     // 获取子节点
//     public TreeNode<T> GetChild(int index);
//     public List<TreeNode<T>> GetChildren();
//     
//     // 获取父节点
//     public TreeNode<T> GetParent();
//     
//     // 获取根节点
//     public TreeNode<T> GetRoot();
//     
//     // 获取从根到当前节点的路径
//     public List<TreeNode<T>> GetPath();
//     
//     // 获取节点深度
//     public int GetDepth();
//     
//     // 获取子树高度
//     public int GetHeight();
//     
//     // 判断节点状态
//     public bool IsRoot();
//     public bool IsLeaf();
//     public bool HasChildren();
//     
//     // 获取子节点数量
//     public int GetChildCount();
//     
//     // 获取所有后代节点
//     public List<TreeNode<T>> GetAllDescendants();
//     
//     // 获取所有祖先节点
//     public List<TreeNode<T>> GetAllAncestors();
//     
//     // 遍历方法
//     public void TraversePreOrder(System.Action<TreeNode> action);
//     public void TraversePostOrder(System.Action<TreeNode> action);
//     public void TraverseLevelOrder(System.Action<TreeNode> action);
// }
