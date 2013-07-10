using System;
using System.Collections.Generic;

namespace Utils
{
	/// <summary>
	/// Reference tree.
	/// 
	/// A tree that can be shared between multiple instances in different versions.
	/// Each node has a reference counter that keeps track of the number of instances.
	/// If there is only one reference, the changes are applied directly to the tree.
	/// If there is more than one reference, the changes returns a new tree.
	/// Nodes are shared as many as possible to save memory.
	/// It can be used to compute and manipulate large structures with small changes.
	/// 
	/// References are only increased to top nodes that are shared,
	/// which means the references of sub nodes does not decrease until
	/// the parent is released.
	/// </summary>
	public class ReferenceTree<T> where T : IComparable<T>
	{
		public int References;
		public T Data;
		private ReferenceTree<T> m_left;
		public ReferenceTree<T> m_right;
		public ReferenceTree<T> m_parent;

		private ReferenceTree<T> LeftMost () {
			if (m_left != null) {
				return m_left.LeftMost ();
			} else {
				return this;
			}
		}

		private ReferenceTree<T> RightMost () {
			if (m_right != null) {
				return m_right.RightMost ();
			} else {
				return this;
			}
		}

		public ReferenceTree<T> SetData (T data) {
			// Check if the new value is in same range.
			var isLarger = m_left == null ? true : data.CompareTo (m_left.RightMost ().Data) > 0;
			var isSmaller = m_right == null ? true : data.CompareTo (m_right.LeftMost ().Data) < 0;
			if (isLarger && isSmaller) {
				if (References == 1) {
					// Just manipulate the tree directly since there is only one reference.
					this.Data = data;
					return this;
				} else {
					// Create a new node that points to the same sub nodes.
					return new ReferenceTree<T> (data) {
						Left = m_left,
						Right = m_right
					};
				}
			}

			throw new Exception ("Data is not within order");
		}

		public ReferenceTree<T> InsertCopy (T data) {
			var res = data.CompareTo (this.Data);
			// Need to create copy.
			if (res == -1) {
				if (this.Left == null) {
					return new ReferenceTree<T> (data) {
						Parent = this
					};
				} else {
					return new ReferenceTree<T> (this.Data) {
						Left = this.Left.InsertCopy (data),
						Right = this.Right
					};
				}
			} else if (res == 1) {
				if (this.Right == null) {
					return new ReferenceTree<T> (data) {
						Parent = this
					};
				} else {
					return new ReferenceTree<T> (this.Data) {
						Right = this.Right.InsertCopy (data),
						Left = this.Left
					};
				}
			} else {
				throw new Exception ("Data already exists");
			}
		}

		public static ReferenceTree<T> InsertCopy (ReferenceTree<T> tree, T data) {
			if (tree == null) {
				return new ReferenceTree<T> (data);
			}
			
			return tree.InsertCopy (data);
		}

		public static ReferenceTree<T> Insert (ReferenceTree<T> tree, T data) {
			if (tree == null) {
				return new ReferenceTree<T> (data);
			}

			return tree.Insert (data);
		}

		public ReferenceTree<T> Insert (T data) {
			if (References <= 1) {
				var res = data.CompareTo (this.Data);
				// Can manipulate tree directly.
				if (res == -1) {
					if (this.Left == null) {
						this.Left = new ReferenceTree<T> (data);
						return this;
					} else {
						var ret = this.Left.Insert (data);
						if (ret != this.Left) {
							return new ReferenceTree<T> (this.Data) {
								Left = ret,
								Right = this.Right
							};
						} else {
							return this;
						}
					}
				} else if (res == 1) {
					if (this.Right == null) {
						this.Right = new ReferenceTree<T> (data);
						return this;
					} else {
						var ret = this.Right.Insert (data);
						if (ret != this.Right) {
							return new ReferenceTree<T> (this.Data) {
								Right = ret,
								Left = this.Left
							};
						} else {
							return this;
						}
					}
				}

				throw new Exception ("Data already exists");
			} else {
				// This node is shared, so we need to create a copy.
				return InsertCopy (data);
			}
		}

		private static void IncreaseReference (ReferenceTree<T> tree) {
			if (tree == null) {
				return;
			}

			tree.References++;
		}

		private static void DecreaseReference (ReferenceTree<T> tree) {
			if (tree == null) {
				return;
			}

			tree.References--;
			if (tree.References <= 0) {
				var disp = tree.Data as IDisposable;
				if (disp != null) {
					disp.Dispose ();
				}

				tree.Data = default(T);

				// Decrease the references to the sub nodes.
				if (tree.m_left != null) {
					DecreaseReference (tree.m_left);
					if (tree.m_left.References <= 0) {
						tree.m_left = null;
					}
				}
				if (tree.m_right != null) {
					DecreaseReference (tree.m_right);
					if (tree.m_right.References <= 0) {
						tree.m_right = null;
					}
				}
				if (tree.m_parent != null) {
					DecreaseReference (tree.m_parent);
					if (tree.m_parent.References <= 0) {
						tree.m_parent = null;
					}
				}
			}

		}

		public ReferenceTree<T> Left {
			get {
				return m_left;
			} set {
				IncreaseReference (value);
				DecreaseReference (m_left);
				m_left = value;
			}
		}

		public ReferenceTree<T> Right {
			get {
				return m_right;
			} set {
				IncreaseReference (value);
				DecreaseReference (m_right);
				m_right = value;
			}
		}

		public ReferenceTree<T> Parent {
			get {
				return m_parent;
			} set {
				IncreaseReference (value);
				DecreaseReference (m_parent);
				m_parent = value;
			}
		}

		public ReferenceTree (T data)
		{
			this.Data = data;
			this.References = 0;
		}

		public bool Contains (T data) {
			return Find (data) != null;
		}

		public ReferenceTree<T> Find (T data) {
			var res = data.CompareTo (this.Data);
			if (res == 0) {
				return this;
			}

			if (Parent == null) {
				if (res == -1) {
					if (this.Left == null) {
						return null;
					} else {
						return this.Left.Find (data);
					}
				} else {
					if (this.Right == null) {
						return null;
					} else {
						return this.Right.Find (data);
					}
				}
			} else {
				var cmpThisToParent = this.Data.CompareTo (this.Parent.Data);
				if (cmpThisToParent == -1) {
					// This node overrides the left side of parent.
					if (res == -1) {
						if (this.Left == null) {
							return null;
						} else {
							return this.Left.Find (data);
						}
					} else {
						var cmpDataToParent = data.CompareTo (this.Parent.Data);
						if (cmpDataToParent == 0) {
							return this.Parent;
						} else if (cmpDataToParent == -1) {
							if (this.Right == null) {
								return null;
							} else {
								return this.Right.Find (data);
							}
						} else {
							if (this.Parent.Right == null) {
								return null;
							} else {
								return this.Parent.Right.Find (data);
							}
						}
					}
				} else {
					// This node overrides the right side of parent.
					if (res == 1) {
						if (this.Right == null) {
							return null;
						} else {
							return this.Right.Find (data);
						}
					} else {
						var cmpDataToParent = data.CompareTo (this.Parent.Data);
						if (cmpDataToParent == 0) {
							return this.Parent;
						} else if (cmpDataToParent == -1) {
							if (this.Parent.Left == null) {
								return null;
							} else {
								return this.Parent.Left.Find (data);
							}
						} else {
							if (this.Left == null) {
								return null;
							} else {
								return this.Left.Find (data);
							}
						}
					}
				}
			}
		}

	}
}

