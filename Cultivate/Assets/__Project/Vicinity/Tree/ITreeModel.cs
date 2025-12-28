
using System.Collections.Generic;

public interface ITreeModel
{
    object GetRoot();
    IEnumerable<object> TraversePreOrder();
    IEnumerable<object> TraversePostOrder();
    IEnumerable<object> TraverseLevelOrder();
}