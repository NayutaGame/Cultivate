// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
//
// public class TreeModel<T>
// {
//     public TreeNode<T> Root { get; private set; }
//     
//     public void Clear();
//     
//     public void SetRoot(TreeNode<T> newRoot);
//     public void SetRoot(T item);
//     
//     public TreeNode<T> FindNode(T item);
//     public List<TreeNode<T>> FindAllNodes(Predicate<TreeNode<T>> match);
//     
//     public TreeNode<T> GetRoot();
//
//     public int GetHeight();
//     
//     public bool ContainsNode(TreeNode<T> node);
//     public bool ContainsNode(T item);
//     
//     public List<TreeNode<T>> GetAllNodes();
//     
//     public List<TreeNode<T>> GetAllLeafNodes();
//     
//     public void TraversePreOrder(Action<TreeNode<T>> action);
//     public void TraversePostOrder(Action<TreeNode<T>> action);
//     public void TraverseLevelOrder(Action<TreeNode<T>> action);
// }
