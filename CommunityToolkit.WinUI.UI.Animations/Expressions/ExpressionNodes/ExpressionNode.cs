// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.UI;
using Microsoft.UI.Composition;

namespace CommunityToolkit.WinUI.UI.Animations.Expressions
{
    /// <summary>
    /// Class ExpressionNode.
    /// </summary>
    public abstract class ExpressionNode : IDisposable
    {
        private List<ReferenceInfo> _objRefList = null;
        private Dictionary<CompositionObject, string> _compObjToParamNameMap = null;
        private Dictionary<string, object> _constParamMap = new Dictionary<string, object>(StringComparer.CurrentCultureIgnoreCase);

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionNode"/> class.
        /// </summary>
        internal ExpressionNode()
        {
        }

        /// <summary>
        /// Resolve a named reference parameter to the CompositionObject it will refer to.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="compObj">The composition object that the parameter should resolve to.</param>
        public void SetReferenceParameter(string parameterName, CompositionObject compObj)
        {
            // Make sure we have our reference list populated
            EnsureReferenceInfo();

            for (int i = 0; i < _objRefList.Count; i++)
            {
                if (string.Compare(_objRefList[i].ParameterName, parameterName, true /*ignoreCase*/) == 0)
                {
                    var item = _objRefList[i];
                    item.CompObject = compObj;
                    _objRefList[i] = item;
                }
            }
        }

        /// <summary>
        /// Resolve a named parameter to the boolean value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetBooleanParameter(string parameterName, bool value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the float value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetScalarParameter(string parameterName, float value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Vector2 value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetVector2Parameter(string parameterName, Vector2 value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Vector3 value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetVector3Parameter(string parameterName, Vector3 value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Vector4 value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetVector4Parameter(string parameterName, Vector4 value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Color value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetColorParameter(string parameterName, Windows.UI.Color value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Quaternion value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetQuaternionParameter(string parameterName, Quaternion value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Matrix3x2 value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetMatrix3x2Parameter(string parameterName, Matrix3x2 value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Resolve a named parameter to the Matrix4x4 value it will use.
        /// </summary>
        /// <param name="parameterName">The string name of the parameter to be resolved.</param>
        /// <param name="value">The value that the parameter should resolve to.</param>
        public void SetMatrix4x4Parameter(string parameterName, Matrix4x4 value)
        {
            _constParamMap[parameterName] = value;
        }

        /// <summary>
        /// Releases all resources used by this ExpressionNode.
        /// </summary>
        public void Dispose()
        {
            _objRefList = null;
            _compObjToParamNameMap = null;
            _constParamMap = null;
            Subchannels = null;
            PropertyName = null;
            NodeType = ExpressionNodeType.Count;

            // Note: we don't recursively dispose all child nodes, as those nodes could be in use by a different Expression
            Children = null;

            if (ExpressionAnimation != null)
            {
                ExpressionAnimation.Dispose();
                ExpressionAnimation = null;
            }
        }

        /// <summary>
        /// Creates the expression node.
        /// </summary>
        /// <typeparam name="T">A class that derives from ExpressionNode</typeparam>
        /// <returns>T.</returns>
        /// <exception cref="global::System.Exception">unexpected type</exception>
        internal static T CreateExpressionNode<T>()
            where T : ExpressionNode
        {
            T newNode;

            if (typeof(T) == typeof(BooleanNode))
            {
                newNode = new BooleanNode() as T;
            }
            else if (typeof(T) == typeof(ScalarNode))
            {
                newNode = new ScalarNode() as T;
            }
            else if (typeof(T) == typeof(Vector2Node))
            {
                newNode = new Vector2Node() as T;
            }
            else if (typeof(T) == typeof(Vector3Node))
            {
                newNode = new Vector3Node() as T;
            }
            else if (typeof(T) == typeof(Vector4Node))
            {
                newNode = new Vector4Node() as T;
            }
            else if (typeof(T) == typeof(ColorNode))
            {
                newNode = new ColorNode() as T;
            }
            else if (typeof(T) == typeof(QuaternionNode))
            {
                newNode = new QuaternionNode() as T;
            }
            else if (typeof(T) == typeof(Matrix3x2Node))
            {
                newNode = new Matrix3x2Node() as T;
            }
            else if (typeof(T) == typeof(Matrix4x4Node))
            {
                newNode = new Matrix4x4Node() as T;
            }
            else
            {
                throw new Exception("unexpected type");
            }

            return newNode;
        }

        /// <summary>
        /// Creates the value keyword.
        /// </summary>
        /// <typeparam name="T">A class that derives from ExpressionNode</typeparam>
        /// <param name="keywordKind">Kind of the keyword.</param>
        /// <returns>T.</returns>
        /// <exception cref="global::System.Exception">Invalid ValueKeywordKind</exception>
        internal static T CreateValueKeyword<T>(ValueKeywordKind keywordKind)
            where T : ExpressionNode
        {
            T node = CreateExpressionNode<T>();

            (node as ExpressionNode).ParamName = null;

            switch (keywordKind)
            {
                case ValueKeywordKind.CurrentValue:
                    (node as ExpressionNode).NodeType = ExpressionNodeType.CurrentValueProperty;
                    break;

                case ValueKeywordKind.StartingValue:
                    (node as ExpressionNode).NodeType = ExpressionNodeType.StartingValueProperty;
                    break;

                default:
                    throw new Exception("Invalid ValueKeywordKind");
            }

            return node;
        }

        /// <summary>
        /// To the expression string.
        /// </summary>
        /// <returns>System.String.</returns>
        internal string ToExpressionString()
        {
            if (_objRefList == null)
            {
                EnsureReferenceInfo();
            }

            return ToExpressionStringInternal();
        }

        /// <summary>
        /// Clears the reference information.
        /// </summary>
        /// <exception cref="System.Exception">Reference and paramName can't both be null</exception>
        internal void ClearReferenceInfo()
        {
            _objRefList = null;
            ParamName = null;
            foreach (var child in Children)
            {
                child.ClearReferenceInfo();
            }
        }

        /// <summary>
        /// Ensures the reference information.
        /// </summary>
        /// <exception cref="global::System.Exception">Reference and paramName can't both be null</exception>
        internal void EnsureReferenceInfo()
        {
            if (_objRefList == null)
            {
                // Get all ReferenceNodes in this expression
                HashSet<ReferenceNode> referenceNodes = new HashSet<ReferenceNode>();
                PopulateParameterNodes(ref _constParamMap, ref referenceNodes);

                // Find all CompositionObjects across all referenceNodes that need a paramName to be created
                HashSet<CompositionObject> compObjects = new HashSet<CompositionObject>();
                foreach (var refNode in referenceNodes)
                {
                    if ((refNode.Reference != null) && (refNode.GetReferenceParamString() == null))
                    {
                        compObjects.Add(refNode.Reference);
                    }
                }

                // Create a map to store the generated paramNames for each CompObj
                _compObjToParamNameMap = new Dictionary<CompositionObject, string>();
                var paramCount = 0u;
                foreach (var compObj in compObjects)
                {
                    string paramName = CreateUniqueParamNameFromIndex(paramCount++);

                    _compObjToParamNameMap.Add(compObj, paramName);
                }

                // Go through all reference nodes again to create our full list of referenceInfo. This time, if
                // the param name is null, look it up from our new map and store it.
                _objRefList = new List<ReferenceInfo>();
                foreach (var refNode in referenceNodes)
                {
                    string paramName = refNode.GetReferenceParamString();

                    if ((refNode.Reference == null) && (paramName == null))
                    {
                        // This can't happen - if the ref is null it must be because it's a named param
                        throw new Exception("Reference and paramName can't both be null");
                    }

                    if (paramName == null)
                    {
                        paramName = _compObjToParamNameMap[refNode.Reference];
                    }

                    _objRefList.Add(new ReferenceInfo(paramName, refNode.Reference));
                    refNode.ParamName = paramName;
                }
            }

            // Generates Excel-column-like identifiers, e.g. A, B, ..., Z, AA, BA...
            // This implementation aggregates characters in reverse order to avoid having to
            // precompute the exact number of characters in the resulting string. This is not
            // important in this context as the only critical property to maintain is to have
            // a unique mapping to each input value to the resulting sequence of letters.
            [SkipLocalsInit]
            static unsafe string CreateUniqueParamNameFromIndex(uint i)
            {
                const int alphabetLength = 'Z' - 'A' + 1;

                // The total length of the resulting sequence is guaranteed to always
                // be less than 8, given that log26(4294967295) ≈ 6.8. In this case we
                // are just allocating the immediate next power of two following that.
                // Note: this is using a char* buffer instead of Span<char> as the latter
                // is not referenced here, and we don't want to pull in an extra package.
                char* characters = stackalloc char[8];

                characters[0] = (char)('A' + (i % alphabetLength));

                int totalCharacters = 1;

                while ((i /= alphabetLength) > 0)
                {
                    i--;

                    characters[totalCharacters++] = (char)('A' + (i % alphabetLength));
                }

                return new string(characters, 0, totalCharacters);
            }
        }

        /// <summary>
        /// Sets all parameters.
        /// </summary>
        /// <param name="animation">The animation.</param>
        internal void SetAllParameters(CompositionAnimation animation)
        {
            // Make sure the list is populated
            EnsureReferenceInfo();

            foreach (var refInfo in _objRefList)
            {
                animation.SetReferenceParameter(refInfo.ParameterName, refInfo.CompObject);
            }

            foreach (var constParam in _constParamMap)
            {
                if (constParam.Value.GetType() == typeof(bool))
                {
                    animation.SetBooleanParameter(constParam.Key, (bool)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(float))
                {
                    animation.SetScalarParameter(constParam.Key, (float)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Vector2))
                {
                    animation.SetVector2Parameter(constParam.Key, (Vector2)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Vector3))
                {
                    animation.SetVector3Parameter(constParam.Key, (Vector3)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Vector4))
                {
                    animation.SetVector4Parameter(constParam.Key, (Vector4)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Windows.UI.Color))
                {
                    animation.SetColorParameter(constParam.Key, (Windows.UI.Color)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Quaternion))
                {
                    animation.SetQuaternionParameter(constParam.Key, (Quaternion)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Matrix3x2))
                {
                    animation.SetMatrix3x2Parameter(constParam.Key, (Matrix3x2)constParam.Value);
                }
                else if (constParam.Value.GetType() == typeof(Matrix4x4))
                {
                    animation.SetMatrix4x4Parameter(constParam.Key, (Matrix4x4)constParam.Value);
                }
                else
                {
                    throw new Exception($"Unexpected constant parameter datatype ({constParam.Value.GetType()})");
                }
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <returns>System.String.</returns>
        protected internal abstract string GetValue();

        /// <summary>
        /// Subchannelses the internal.
        /// </summary>
        /// <typeparam name="T">A class that derives from ExpressionNode.</typeparam>
        /// <param name="subchannels">The subchannels.</param>
        /// <returns>T.</returns>
        protected internal T SubchannelsInternal<T>(params string[] subchannels)
            where T : ExpressionNode
        {
            ExpressionNodeType swizzleNodeType = ExpressionNodeType.Swizzle;
            T newNode;

            switch (subchannels.GetLength(0))
            {
                case 1:
                    newNode = ExpressionFunctions.Function<ScalarNode>(swizzleNodeType, this) as T;
                    break;

                case 2:
                    newNode = ExpressionFunctions.Function<Vector2Node>(swizzleNodeType, this) as T;
                    break;

                case 3:
                    newNode = ExpressionFunctions.Function<Vector3Node>(swizzleNodeType, this) as T;
                    break;

                case 4:
                    newNode = ExpressionFunctions.Function<Vector4Node>(swizzleNodeType, this) as T;
                    break;

                case 6:
                    newNode = ExpressionFunctions.Function<Matrix3x2Node>(swizzleNodeType, this) as T;
                    break;

                case 16:
                    newNode = ExpressionFunctions.Function<Matrix4x4Node>(swizzleNodeType, this) as T;
                    break;

                default:
                    throw new Exception($"Invalid subchannel count ({subchannels.GetLength(0)})");
            }

            (newNode as ExpressionNode).Subchannels = subchannels;

            return newNode;
        }

        /// <summary>
        /// Populates the parameter nodes.
        /// </summary>
        /// <param name="constParamMap">The constant parameter map.</param>
        /// <param name="referenceNodes">The reference nodes.</param>
        protected internal void PopulateParameterNodes(ref Dictionary<string, object> constParamMap, ref HashSet<ReferenceNode> referenceNodes)
        {
            var refNode = this as ReferenceNode;
            if ((refNode != null) && (refNode.NodeType != ExpressionNodeType.TargetReference))
            {
                referenceNodes.Add(refNode);
            }

            if ((_constParamMap != null) && (_constParamMap != constParamMap))
            {
                foreach (var entry in _constParamMap)
                {
                    // If this parameter hasn't already been set on the root, use this node's parameter info
                    if (!constParamMap.ContainsKey(entry.Key))
                    {
                        constParamMap[entry.Key] = entry.Value;
                    }
                }
            }

            foreach (var child in Children)
            {
                child.PopulateParameterNodes(ref constParamMap, ref referenceNodes);
            }
        }

        private OperationType GetOperationKind()
        {
            return ExpressionFunctions.GetNodeInfoFromType(NodeType).NodeOperationKind;
        }

        private string GetOperationString()
        {
            return ExpressionFunctions.GetNodeInfoFromType(NodeType).OperationString;
        }

        private string ToExpressionStringInternal()
        {
            string ret;

            // Do a recursive depth-first traversal of the node tree to print out the full expression string
            switch (GetOperationKind())
            {
                case OperationType.Function:
                    if (Children.Count == 0)
                    {
                        throw new Exception("Can't have an expression function with no params");
                    }

                    ret = $"{GetOperationString()}({Children[0].ToExpressionStringInternal()}";
                    for (int i = 1; i < Children.Count; i++)
                    {
                        ret += "," + Children[i].ToExpressionStringInternal();
                    }

                    ret += ")";
                    break;

                case OperationType.Operator:
                    if (Children.Count != 2)
                    {
                        throw new Exception("Can't have an operator that doesn't have 2 exactly params");
                    }

                    ret = $"({Children[0].ToExpressionStringInternal()} {GetOperationString()} {Children[1].ToExpressionStringInternal()})";
                    break;

                case OperationType.UnaryOperator:
                    if (Children.Count != 1)
                    {
                        throw new Exception("Can't have an unary operator that doesn't have exactly one params");
                    }

                    ret = $"( {GetOperationString()} {Children[0].ToExpressionStringInternal()} )";
                    break;

                case OperationType.Constant:
                    if (Children.Count == 0)
                    {
                        // If a parameterName was specified, use it. Otherwise write the value.
                        ret = ParamName ?? GetValue();
                    }
                    else
                    {
                        throw new Exception("Constants must have 0 children");
                    }

                    break;

                case OperationType.Swizzle:
                    if (Children.Count != 1)
                    {
                        throw new Exception("Swizzles should have exactly 1 child");
                    }

                    string swizzleString = string.Empty;
                    foreach (var sub in Subchannels)
                    {
                        swizzleString += sub;
                    }

                    ret = $"{Children[0].ToExpressionStringInternal()}.{swizzleString}";
                    break;

                case OperationType.Reference:
                    if ((NodeType == ExpressionNodeType.Reference) ||
                        (NodeType == ExpressionNodeType.TargetReference))
                    {
                        // This is the reference node itself
                        if (Children.Count != 0)
                        {
                            throw new Exception("References cannot have children");
                        }

                        ret = (this as ReferenceNode).GetReferenceParamString();
                    }
                    else if (NodeType == ExpressionNodeType.ReferenceProperty)
                    {
                        // This is the property node of the reference
                        if (Children.Count != 1)
                        {
                            throw new Exception("Reference properties must have exactly one child");
                        }

                        if (PropertyName == null)
                        {
                            throw new Exception("Reference properties must have a property name");
                        }

                        ret = $"{Children[0].ToExpressionStringInternal()}.{PropertyName}";
                    }
                    else if (NodeType == ExpressionNodeType.StartingValueProperty)
                    {
                        // This is a "this.StartingValue" node
                        if (Children.Count != 0)
                        {
                            throw new Exception("StartingValue references Cannot have children");
                        }

                        ret = "this.StartingValue";
                    }
                    else if (NodeType == ExpressionNodeType.CurrentValueProperty)
                    {
                        // This is a "this.CurrentValue" node
                        if (Children.Count != 0)
                        {
                            throw new Exception("CurrentValue references Cannot have children");
                        }

                        ret = "this.CurrentValue";
                    }
                    else
                    {
                        throw new Exception("Unexpected NodeType for OperationType.Reference");
                    }

                    break;

                case OperationType.Conditional:
                    if (Children.Count != 3)
                    {
                        throw new Exception("Conditionals must have exactly 3 children");
                    }

                    ret = $"(({Children[0].ToExpressionStringInternal()}) ? ({Children[1].ToExpressionStringInternal()}) : ({Children[2].ToExpressionStringInternal()}))";
                    break;

                default:
                    throw new Exception($"Unexpected operation type ({GetOperationKind()}), nodeType = {NodeType}");
            }

            return ret;
        }

        /// <summary>
        /// Struct ReferenceInfo
        /// </summary>
        internal struct ReferenceInfo
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ReferenceInfo"/> struct.
            /// </summary>
            /// <param name="paramName">Name of the parameter.</param>
            /// <param name="compObj">The comp object.</param>
            public ReferenceInfo(string paramName, CompositionObject compObj)
            {
                ParameterName = paramName;
                CompObject = compObj;
            }

            /// <summary>
            /// Gets or sets the name of the parameter.
            /// </summary>
            /// <value>The name of the parameter.</value>
            public string ParameterName { get; set; }

            /// <summary>
            /// Gets or sets the comp object.
            /// </summary>
            /// <value>The comp object.</value>
            public CompositionObject CompObject { get; set; }
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        internal string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        /// <value>The type of the node.</value>
        internal ExpressionNodeType NodeType { get; set; }

        /// <summary>
        /// Gets or sets the children.
        /// </summary>
        /// <value>The children.</value>
        internal List<ExpressionNode> Children { get; set; } = new List<ExpressionNode>();

        /// <summary>
        /// Gets or sets the name of the parameter.
        /// </summary>
        /// <value>The name of the parameter.</value>
        internal string ParamName { get; set; }

        /// <summary>
        /// Gets or sets the expression animation.
        /// </summary>
        /// <value>The expression animation.</value>
        internal ExpressionAnimation ExpressionAnimation { get; set; }

        /// <summary>
        /// Gets or sets the subchannels.
        /// </summary>
        /// <value>The subchannels.</value>
        protected internal string[] Subchannels { get; set; }
    }
}
